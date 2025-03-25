using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountAndSortWinFormsAppNetFr4
{
    /// <summary>
    /// Trieda na spracovanie dávkových súborov, odstránenie duplicít, 
    /// prečíslovanie a zoradenie záznamov
    /// </summary>
    public class BatchFileProcessor
    {
        #region Properties

        // Konfiguračné parametre
        private readonly string columnSeparator;
        private readonly int dayColumnIndex;
        private readonly int idColumnIndex;
        private readonly int serviceCodeColumnIndex;
        private readonly int nameColumnIndex;
        private readonly int pointsColumnIndex;
        private readonly bool sortByName;
        private readonly bool renumberRows;
        private readonly bool removeDuplicates;
        private readonly int headerTotalLinesIndex; // Index pre počet záznamov v hlavičke

        // Výsledky spracovania
        public int DuplicatesRemoved { get; private set; }
        public List<string> ProcessingLog { get; private set; } = new List<string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Vytvorí novú inštanciu procesora dávkových súborov s definovanými parametrami
        /// </summary>
        public BatchFileProcessor(
            string separator,
            int dayColIndex,
            int idColIndex,
            int serviceCodeColIndex,
            int nameColIndex = -1,
            int pointsColIndex = -1,
            int totalLinesColIndex = 5,
            bool sortByName = true,
            bool renumberRows = true,
            bool removeDuplicates = true)
        {
            columnSeparator = separator;
            dayColumnIndex = dayColIndex;
            idColumnIndex = idColIndex;
            serviceCodeColumnIndex = serviceCodeColIndex;
            nameColumnIndex = nameColIndex;
            pointsColumnIndex = pointsColIndex;
            headerTotalLinesIndex = totalLinesColIndex; // Uloženie indexu
            this.sortByName = sortByName;
            this.renumberRows = renumberRows;
            this.removeDuplicates = removeDuplicates;

            ProcessingLog = new List<string>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Asynchrónne spracuje dávkový súbor podľa nakonfigurovaných parametrov
        /// </summary>
        public async Task<ProcessingResult> ProcessBatchFileAsync(string inputFilePath, string outputFilePath = null)
        {
            return await Task.Factory.StartNew(() => ProcessBatchFile(inputFilePath, outputFilePath));
        }

        /// <summary>
        /// Spracuje dávkový súbor podľa nakonfigurovaných parametrov
        /// </summary>
        public ProcessingResult ProcessBatchFile(string inputFilePath, string outputFilePath = null)
        {
            DuplicatesRemoved = 0;
            ProcessingLog.Clear();

            var result = new ProcessingResult
            {
                InputFilePath = inputFilePath,
                FileName = Path.GetFileName(inputFilePath),
                ProcessingLog = new List<string>()
            };

            // Ak nie je zadaný výstupný súbor, vytvoríme nový názov
            if (string.IsNullOrEmpty(outputFilePath))
            {
                outputFilePath = Path.Combine(
                    Path.GetDirectoryName(inputFilePath),
                    "processed_" + Path.GetFileName(inputFilePath));
            }
            result.OutputFilePath = outputFilePath;

            try
            {
                // Kontrola existencie súboru
                if (!File.Exists(inputFilePath))
                {
                    LogMessage($"Súbor sa nenašiel: {inputFilePath}", result);
                    result.Success = false;
                    return result;
                }

                // Načítanie všetkých riadkov zo súboru
                string[] lines = File.ReadAllLines(inputFilePath);
                LogMessage($"Načítaných {lines.Length} riadkov zo súboru", result);

                if (lines.Length < 3)
                {
                    LogMessage("Súbor neobsahuje dostatok riadkov na spracovanie", result);
                    result.Success = false;
                    return result;
                }

                // Počítanie pôvodných bodov (ešte pred akýmkoľvek spracovaním)
                if (pointsColumnIndex >= 0)
                {
                    result.OriginalPointsCount = CalculateTotalPoints(lines);
                    LogMessage($"Celkový počet bodov v pôvodnom súbore: {result.OriginalPointsCount}", result);
                }

                // Extrakcia hlavičky (prvý riadok) a počtu záznamov
                string headerLine = lines[0];
                string[] headerParts = headerLine.Split(new[] { columnSeparator }, StringSplitOptions.None);

                // Extrakcia počtu položiek v hlavičke (na 6. pozícii, index 5)
                int headerRecordCount = 0;
                if (headerParts.Length > headerTotalLinesIndex && !string.IsNullOrEmpty(headerParts[headerTotalLinesIndex]))
                {
                    int.TryParse(headerParts[headerTotalLinesIndex], out headerRecordCount);
                }
                LogMessage($"Pôvodný počet záznamov v hlavičke: {headerRecordCount}", result);

                // Nastavíme skutočný počet riadkov (bez hlavičky a info o dávke)
                int actualDataRowCount = lines.Length - 2;
                result.OriginalRecordCount = actualDataRowCount;
                LogMessage($"Skutočný počet dátových riadkov: {actualDataRowCount}", result);

                // Druhý riadok obsahuje údaje o dávke, necháme ho bez zmeny
                string batchInfoLine = lines[1];

                LogMessage($"Skutočný počet dátových riadkov: {actualDataRowCount}", result);

                // Spracovanie riadkov s údajmi (od 3. riadku)
                List<string> processedLines = new List<string>
                {
                    headerLine,  // Pridáme hlavičku (aktualizujeme ju neskôr)
                    batchInfoLine // Pridáme informácie o dávke
                };

                // Spracovanie dátových riadkov
                List<string[]> dataRows = new List<string[]>();
                for (int i = 2; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    string[] parts = line.Split(new[] { columnSeparator }, StringSplitOptions.None);
                    if (parts.Length < Math.Max(Math.Max(dayColumnIndex, idColumnIndex), serviceCodeColumnIndex) + 1)
                    {
                        LogMessage($"Riadok {i + 1} nemá dostatok stĺpcov, preskakujem", result);
                        continue;
                    }

                    dataRows.Add(parts);
                }

                // Spracovanie dátových riadkov podľa nakonfigurovaných nastavení
                List<string[]> processedDataRows;

                // 1. Odstránenie duplicít (ak je nastavené)
                if (removeDuplicates)
                {
                    processedDataRows = RemoveDuplicates(dataRows, result);
                }
                else
                {
                    processedDataRows = dataRows;
                }

                // 2. Zoradenie podľa mena (ak je nastavené)
                if (sortByName && nameColumnIndex >= 0)
                {
                    processedDataRows = SortByName(processedDataRows, result);
                }

                // 3. Prečíslovanie riadkov (ak je nastavené)
                if (renumberRows)
                {
                    processedDataRows = RenumberRows(processedDataRows, result);
                }

                // Aktualizácia počtu záznamov v hlavičke
                int newRecordCount = processedDataRows.Count;
                result.ProcessedRecordCount = newRecordCount;
                result.HeaderRecordCount = headerRecordCount;

                if (headerParts.Length > headerTotalLinesIndex)
                {
                    headerParts[headerTotalLinesIndex] = newRecordCount.ToString();
                    string newHeaderLine = string.Join(columnSeparator, headerParts);
                    processedLines[0] = newHeaderLine;
                    LogMessage($"Aktualizovaný počet záznamov v hlavičke na indexe {headerTotalLinesIndex}: {newRecordCount}", result);
                }

                // Konverzia spracovaných dátových riadkov späť na reťazce
                foreach (var parts in processedDataRows)
                {
                    processedLines.Add(string.Join(columnSeparator, parts));
                }

                // Počítanie nových bodov (po spracovaní)
                if (pointsColumnIndex >= 0)
                {
                    result.ProcessedPointsCount = CalculateTotalPoints(processedLines.ToArray());
                    LogMessage($"Celkový počet bodov po spracovaní: {result.ProcessedPointsCount}", result);
                }

                // Zápis výsledku do súboru
                File.WriteAllLines(outputFilePath, processedLines);
                LogMessage($"Výsledný súbor uložený do: {outputFilePath}", result);

                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                LogMessage($"Chyba pri spracovaní súboru: {ex.Message}", result);
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// Vypočíta celkový počet bodov v súbore na základe určeného stĺpca
        /// </summary>
        public int CalculateTotalPoints(string[] lines)
        {
            if (pointsColumnIndex < 0) return 0;

            int totalPoints = 0;
            int errorCount = 0;

            // Prechádzame všetky riadky okrem prvých dvoch (hlavička a info o dávke)
            for (int i = 2; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrEmpty(line)) continue;

                string[] parts = line.Split(new[] { columnSeparator }, StringSplitOptions.None);

                if (parts.Length <= pointsColumnIndex)
                {
                    errorCount++;
                    continue;
                }

                string pointsValue = parts[pointsColumnIndex].Trim();
                if (string.IsNullOrEmpty(pointsValue))
                {
                    errorCount++;
                    continue;
                }

                if (int.TryParse(pointsValue, out int points))
                {
                    totalPoints += points;
                }
                else
                {
                    errorCount++;
                }
            }

            LogMessage($"Celkový počet bodov: {totalPoints}, chybných riadkov: {errorCount}");
            return totalPoints;
        }

        /// <summary>
        /// Vráti protokol o spracovaní ako jeden textový reťazec
        /// </summary>
        public string GetLogAsString()
        {
            return string.Join(Environment.NewLine, ProcessingLog);
        }

        #endregion

        #region Private Methods

        private List<string[]> RemoveDuplicates(List<string[]> rows, ProcessingResult result)
        {
            var uniqueRecords = new Dictionary<string, string[]>();
            int duplicateCount = 0;

            foreach (var parts in rows)
            {
                // Kontrola, či má riadok dostatočný počet stĺpcov
                if (parts.Length <= Math.Max(Math.Max(dayColumnIndex, idColumnIndex), serviceCodeColumnIndex))
                {
                    LogMessage("Preskakujem riadok s nedostatočným počtom stĺpcov", result);
                    continue;
                }

                // Vytvorenie kľúča z kombinácie dňa, rodného čísla a kódu výkonu
                string day = parts.Length > dayColumnIndex ? parts[dayColumnIndex] : "";
                string personalId = parts.Length > idColumnIndex ? parts[idColumnIndex] : "";
                string serviceCode = parts.Length > serviceCodeColumnIndex ? parts[serviceCodeColumnIndex] : "";

                string uniqueKey = $"{day}|{personalId}|{serviceCode}";

                if (!uniqueRecords.ContainsKey(uniqueKey))
                {
                    uniqueRecords[uniqueKey] = parts;
                }
                else
                {
                    duplicateCount++;
                }
            }

            DuplicatesRemoved = duplicateCount;
            LogMessage($"Odstránených duplicít: {duplicateCount}", result);

            return uniqueRecords.Values.ToList();
        }

        private List<string[]> SortByName(List<string[]> rows, ProcessingResult result)
        {
            if (nameColumnIndex < 0 || rows.Count == 0)
            {
                LogMessage("Neplatný index mena alebo prázdny zoznam, zoradenie preskočené", result);
                return rows;
            }

            // Zoradenie podľa mena a potom podľa ID
            var sortedRows = rows
                .OrderBy(parts => parts.Length > nameColumnIndex ? parts[nameColumnIndex] : "")
                .ThenBy(parts => parts.Length > idColumnIndex && idColumnIndex >= 0 ?
                    (int.TryParse(parts[idColumnIndex], out int id) ? id : 0) : 0)
                .ToList();

            LogMessage($"Riadky zoradené podľa mena (stĺpec {nameColumnIndex + 1})", result);
            return sortedRows;
        }

        private List<string[]> RenumberRows(List<string[]> rows, ProcessingResult result)
        {
            int newNumber = 1;
            var renumberedRows = new List<string[]>();

            foreach (var parts in rows)
            {
                if (parts.Length > 0)
                {
                    parts[0] = newNumber++.ToString();
                }
                renumberedRows.Add(parts);
            }

            LogMessage("Riadky prečíslované", result);
            return renumberedRows;
        }

        private void LogMessage(string message, ProcessingResult result = null)
        {
            ProcessingLog.Add(message);
            if (result != null)
            {
                result.ProcessingLog.Add(message);
            }
        }

        #endregion
    }

    /// <summary>
    /// Trieda pre výsledky spracovania dávkového súboru
    /// </summary>
    public class ProcessingResult
    {
        public bool Success { get; set; }
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }
        public string FileName { get; set; }
        public int DuplicatesRemoved { get; set; }
        public int OriginalRecordCount { get; set; }
        public int ProcessedRecordCount { get; set; }
        public int OriginalPointsCount { get; set; }
        public int ProcessedPointsCount { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ProcessingLog { get; set; } = new List<string>();
        public int HeaderRecordCount { get; set; }

        public int GetRemovedRowsCount()
        {
            return Math.Max(0, OriginalRecordCount - ProcessedRecordCount);
        }
    }
}
