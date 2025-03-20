using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CountAndSortWinFormsAppNetFr4
{
    public class ProcessedFileInfo
    {
        // Základné vlastnosti pre identifikáciu
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public DateTime ProcessedTime { get; set; }
        public int Points { get; set; }
        public string UniqueIdentifier { get; set; }
        public DateTime FileCreationTime { get; set; }
        public long FileSize { get; set; }
        public string SourceDirectory { get; set; }

        // Vlastnosti pre duplicity
        public int RemovedDuplicates { get; set; }
        public int DuplicatesRemoved { get; set; }

        // Nové vlastnosti pre rozšírenú štatistiku
        public Dictionary<string, int> DiagnosisCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ServiceCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> DoctorCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> FacilityCounts { get; set; } = new Dictionary<string, int>();

        // Hodnota v EUR (kalkulovaná z bodov)
        public decimal TotalValue { get; set; }

        public ProcessedFileInfo(string fileName, string fullPath, DateTime time = default, int points = 0, string sourceDirectory = "")
        {
            FileName = fileName;
            FullPath = Path.GetFullPath(fullPath); // Konverzia na absolútnu cestu
            ProcessedTime = time == default ? DateTime.Now : time;
            Points = points;
            SourceDirectory = sourceDirectory;

            // Inicializácia slovníkov
            DiagnosisCounts = new Dictionary<string, int>();
            ServiceCounts = new Dictionary<string, int>();
            DoctorCounts = new Dictionary<string, int>();
            FacilityCounts = new Dictionary<string, int>();

            try
            {
                var fileInfo = new FileInfo(FullPath);
                FileCreationTime = fileInfo.CreationTime;
                FileSize = fileInfo.Length;
                UniqueIdentifier = $"{fileName}_{FileCreationTime.Ticks}_{FileSize}";
            }
            catch (Exception)
            {
                // Elegantné zvládnutie, ak súbor nie je nájdený
                FileCreationTime = DateTime.Now;
                FileSize = 0;
                UniqueIdentifier = $"{fileName}_{DateTime.Now.Ticks}";
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is ProcessedFileInfo other)
            {
                return UniqueIdentifier == other.UniqueIdentifier;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return UniqueIdentifier.GetHashCode();
        }

        // Metóda pre analýzu obsahu súboru a extrakciu štatistík
        public void AnalyzeContent(string filePath, int pointsColumnIndex, int diagnosisColumnIndex,
            int serviceColumnIndex, int doctorColumnIndex, int facilityColumnIndex, int pointPriceColumnIndex)
        {
            if (!File.Exists(filePath))
                return;

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                string separator = "|"; // Prednastavený oddeľovač - môžete upraviť podľa vašich nastavení

                foreach (string line in lines.Skip(2)) // Preskočenie prvých dvoch riadkov (hlavička)
                {
                    string[] parts = line.Split(new[] { separator }, StringSplitOptions.None);

                    // Kontrola, či má riadok dostatok stĺpcov
                    if (parts.Length <= Math.Max(pointsColumnIndex,
                        Math.Max(diagnosisColumnIndex,
                        Math.Max(serviceColumnIndex,
                        Math.Max(doctorColumnIndex, facilityColumnIndex)))))
                        continue;

                    // Extrakcia diagnózy
                    if (diagnosisColumnIndex >= 0 && parts.Length > diagnosisColumnIndex)
                    {
                        string diagnosis = parts[diagnosisColumnIndex].Trim();
                        if (!string.IsNullOrEmpty(diagnosis))
                        {
                            if (DiagnosisCounts.ContainsKey(diagnosis))
                                DiagnosisCounts[diagnosis]++;
                            else
                                DiagnosisCounts[diagnosis] = 1;
                        }
                    }

                    // Extrakcia kódu služby
                    if (serviceColumnIndex >= 0 && parts.Length > serviceColumnIndex)
                    {
                        string service = parts[serviceColumnIndex].Trim();
                        if (!string.IsNullOrEmpty(service))
                        {
                            if (ServiceCounts.ContainsKey(service))
                                ServiceCounts[service]++;
                            else
                                ServiceCounts[service] = 1;
                        }
                    }

                    // Extrakcia lekára
                    if (doctorColumnIndex >= 0 && parts.Length > doctorColumnIndex)
                    {
                        string doctor = parts[doctorColumnIndex].Trim();
                        if (!string.IsNullOrEmpty(doctor))
                        {
                            if (DoctorCounts.ContainsKey(doctor))
                                DoctorCounts[doctor]++;
                            else
                                DoctorCounts[doctor] = 1;
                        }
                    }

                    // Extrakcia zariadenia
                    if (facilityColumnIndex >= 0 && parts.Length > facilityColumnIndex)
                    {
                        string facility = parts[facilityColumnIndex].Trim();
                        if (!string.IsNullOrEmpty(facility))
                        {
                            if (FacilityCounts.ContainsKey(facility))
                                FacilityCounts[facility]++;
                            else
                                FacilityCounts[facility] = 1;
                        }
                    }

                    // Výpočet hodnoty
                    if (pointsColumnIndex >= 0 && pointPriceColumnIndex >= 0 &&
                        parts.Length > Math.Max(pointsColumnIndex, pointPriceColumnIndex))
                    {
                        if (int.TryParse(parts[pointsColumnIndex].Trim(), out int points) &&
                            decimal.TryParse(parts[pointPriceColumnIndex].Trim().Replace(',', '.'),
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out decimal pointPrice))
                        {
                            // Pridanie hodnoty k celkovej sume
                            TotalValue += points * pointPrice;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Chyba pri analýze súboru {filePath}: {ex.Message}");
            }
        }
    }
}
