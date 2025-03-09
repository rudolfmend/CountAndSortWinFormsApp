using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CountAndSortWinFormsAppNetFr4
{
    /// <summary>
    /// Trieda na odstránenie duplicitných záznamov z dávkových súborov
    /// </summary>
    public class BatchFileProcessor
    {
        // Vlastnosti pre sledovanie počtu odstránených duplicít a štatistík
        public int DuplicatesRemoved { get; private set; }
        public List<string> ProcessingLog { get; private set; } = new List<string>();

        /// <summary>
        /// Spracuje dávkový súbor a odstráni duplicitné riadky
        /// </summary>
        /// <param name="inputFilePath">Cesta k vstupnému súboru</param>
        /// <param name="outputFilePath">Cesta pre výstupný súbor (voliteľné)</param>
        /// <returns>True ak bolo spracovanie úspešné</returns>
        public bool ProcessBatchFileAndRemoveDuplicates(string inputFilePath, string outputFilePath = null)
        {
            DuplicatesRemoved = 0;
            ProcessingLog.Clear();

            // Ak nie je zadaný výstupný súbor, vytvoríme nový názov
            if (string.IsNullOrEmpty(outputFilePath))
            {
                outputFilePath = Path.Combine(
                    Path.GetDirectoryName(inputFilePath),
                    Path.GetFileNameWithoutExtension(inputFilePath) + "_noDuplicates" + Path.GetExtension(inputFilePath));
            }

            try
            {
                // Kontrola existencie súboru
                if (!File.Exists(inputFilePath))
                {
                    ProcessingLog.Add($"Súbor sa nenašiel: {inputFilePath}");
                    return false;
                }

                // Načítanie všetkých riadkov zo súboru
                string[] lines = File.ReadAllLines(inputFilePath);
                ProcessingLog.Add($"Načítaných {lines.Length} riadkov zo súboru");

                if (lines.Length < 3)
                {
                    ProcessingLog.Add("Súbor neobsahuje dostatok riadkov na spracovanie");
                    return false;
                }

                // Extrakcia hlavičky (prvý riadok)
                string headerLine = lines[0];
                string[] headerParts = headerLine.Split('|');

                // Extrakcia počtu položiek v hlavičke (na 5. pozícii, index 4)
                int recordCount = 0;
                if (headerParts.Length > 4 && !string.IsNullOrEmpty(headerParts[4]))
                {
                    int.TryParse(headerParts[4], out recordCount);
                }
                ProcessingLog.Add($"Pôvodný počet záznamov v hlavičke: {recordCount}");

                // Druhý riadok obsahuje údaje o dávke, necháme ho bez zmeny
                string batchInfoLine = lines[1];

                // Spracovanie riadkov s údajmi klientov (od 3. riadku)
                var uniqueRecords = new Dictionary<string, int>(); // Kľúč -> index riadku
                var duplicateIndices = new List<int>();

                // Vytvoríme zoznam duplicít podľa kombinácie dňa, rodného čísla a kódu výkonu
                for (int i = 2; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    string[] parts = line.Split('|');
                    if (parts.Length < 6) continue; // Potrebujeme minimálne 6 častí

                    // Vytvorenie kľúča z kombinácie dňa, rodného čísla a kódu výkonu
                    string day = parts[1];           // Deň (index 1)
                    string personalId = parts[2];    // Rodné číslo (index 2) 
                    string serviceCode = parts[5];   // Kód výkonu (index 5)

                    string uniqueKey = $"{day}|{personalId}|{serviceCode}";

                    // Kontrola, či tento kľúč už bol zaznamenaný
                    if (uniqueRecords.ContainsKey(uniqueKey))
                    {
                        // Našli sme duplicitu
                        duplicateIndices.Add(i);
                        DuplicatesRemoved++;

                        int originalLineIndex = uniqueRecords[uniqueKey];
                        ProcessingLog.Add($"Nájdená duplicita: Riadok {i - 1} je duplikát riadku {originalLineIndex - 1}, kľúč: {uniqueKey}");
                    }
                    else
                    {
                        // Unikátny záznam, pridáme ho do slovníka
                        uniqueRecords[uniqueKey] = i;
                    }
                }

                ProcessingLog.Add($"Celkový počet nájdených duplicít: {DuplicatesRemoved}");

                // Ak nemáme žiadne duplicity, nemusíme vytvárať nový súbor
                if (DuplicatesRemoved == 0)
                {
                    ProcessingLog.Add("Žiadne duplicity neboli nájdené, súbor zostáva nezmenený.");
                    return true;
                }

                // Aktualizácia počtu záznamov v hlavičke
                int newRecordCount = recordCount - DuplicatesRemoved;
                headerParts[4] = newRecordCount.ToString();
                string newHeaderLine = string.Join("|", headerParts);
                ProcessingLog.Add($"Aktualizovaný počet záznamov v hlavičke: {newRecordCount}");

                // Vytvorenie nového zoznamu riadkov bez duplicít a s prečíslovaním
                var outputLines = new List<string>();
                outputLines.Add(newHeaderLine);   // Aktualizovaná hlavička
                outputLines.Add(batchInfoLine);   // Informácie o dávke

                int newLineNumber = 1;
                for (int i = 2; i < lines.Length; i++)
                {
                    // Ak je tento index v zozname duplicít, preskočíme ho
                    if (duplicateIndices.Contains(i))
                        continue;

                    string line = lines[i];
                    string[] parts = line.Split('|');

                    // Prečíslovanie riadkov - prvý stĺpec je poradové číslo
                    if (parts.Length > 0)
                    {
                        parts[0] = newLineNumber.ToString();
                        line = string.Join("|", parts);
                        newLineNumber++;
                    }

                    outputLines.Add(line);
                }

                // Zápis výsledku do súboru
                File.WriteAllLines(outputFilePath, outputLines);
                ProcessingLog.Add($"Výsledný súbor uložený do: {outputFilePath}");

                return true;
            }
            catch (Exception ex)
            {
                ProcessingLog.Add($"Chyba pri spracovaní súboru: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Vráti protokol o spracovaní ako jeden textový reťazec
        /// </summary>
        public string GetLogAsString()
        {
            return string.Join(Environment.NewLine, ProcessingLog);
        }
    }
}
