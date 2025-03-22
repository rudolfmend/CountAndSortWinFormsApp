using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CountAndSortWinFormsAppNetFr4
{
    // Pomocná trieda pre ukladanie údajov o lekárovi
    public class DoctorCounts // DoctorData / DoctorCounts
    {
        public int TotalPoints { get; set; }
        public int RecordCount { get; set; }
        public decimal TotalValue { get; set; }
        public Dictionary<string, int> DiagnosisCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ServiceCounts { get; set; } = new Dictionary<string, int>();
    }

    // Pomocná trieda pre ukladanie údajov o zariadení
    public class FacilityData
    {
        public int TotalPoints { get; set; }
        public int RecordCount { get; set; }
        public decimal TotalValue { get; set; }
        public Dictionary<string, int> DoctorCounts { get; set; } = new Dictionary<string, int>();
    }

    public class ProcessedFileInfo
    {
        // Základné vlastnosti pre identifikáciu a spracovanie súboru 
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

        // Vylepšené vlastnosti pre rozšírenú štatistiku
        public Dictionary<string, int> DiagnosisCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ServiceCounts { get; set; } = new Dictionary<string, int>();

        // Nahradené pôvodné slovníky s počtami za komplexnejšie údaje
        public Dictionary<string, DoctorCounts> DoctorCounts { get; set; } = new Dictionary<string, DoctorCounts>();
        public Dictionary<string, FacilityData> FacilityData { get; set; } = new Dictionary<string, FacilityData>();

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
            DoctorCounts = new Dictionary<string, DoctorCounts>();
            FacilityData = new Dictionary<string, FacilityData>();

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

                    // Extrakcia bodov a ceny
                    int points = 0;
                    decimal pointPrice = 0;

                    if (pointsColumnIndex >= 0 && parts.Length > pointsColumnIndex &&
                        int.TryParse(parts[pointsColumnIndex].Trim(), out int p))
                    {
                        points = p;
                    }

                    if (pointPriceColumnIndex >= 0 && parts.Length > pointPriceColumnIndex &&
                        decimal.TryParse(parts[pointPriceColumnIndex].Trim().Replace(',', '.'),
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out decimal price))
                    {
                        pointPrice = price;
                    }

                    // Výpočet hodnoty za záznam
                    decimal recordValue = points * pointPrice;
                    TotalValue += recordValue;

                    // Extrakcia diagnózy
                    string diagnosis = "";
                    if (diagnosisColumnIndex >= 0 && parts.Length > diagnosisColumnIndex)
                    {
                        diagnosis = parts[diagnosisColumnIndex].Trim();
                        if (!string.IsNullOrEmpty(diagnosis))
                        {
                            if (DiagnosisCounts.ContainsKey(diagnosis))
                                DiagnosisCounts[diagnosis]++;
                            else
                                DiagnosisCounts[diagnosis] = 1;
                        }
                    }

                    // Extrakcia kódu služby
                    string service = "";
                    if (serviceColumnIndex >= 0 && parts.Length > serviceColumnIndex)
                    {
                        service = parts[serviceColumnIndex].Trim();
                        if (!string.IsNullOrEmpty(service))
                        {
                            if (ServiceCounts.ContainsKey(service))
                                ServiceCounts[service]++;
                            else
                                ServiceCounts[service] = 1;
                        }
                    }

                    // Extrakcia lekára s rozšírenými údajmi
                    if (doctorColumnIndex >= 0 && parts.Length > doctorColumnIndex)
                    {
                        string doctor = parts[doctorColumnIndex].Trim();
                        if (!string.IsNullOrEmpty(doctor))
                        {
                            if (!DoctorCounts.ContainsKey(doctor))
                            {
                                DoctorCounts[doctor] = new DoctorCounts
                                {
                                    TotalPoints = 0,
                                    RecordCount = 0,
                                    TotalValue = 0,
                                    DiagnosisCounts = new Dictionary<string, int>(),
                                    ServiceCounts = new Dictionary<string, int>()
                                };
                            }

                            // Aktualizácia údajov o lekárovi
                            DoctorCounts[doctor].TotalPoints += points;
                            DoctorCounts[doctor].RecordCount++;
                            DoctorCounts[doctor].TotalValue += recordValue;

                            // Pridanie diagnózy
                            if (!string.IsNullOrEmpty(diagnosis))
                            {
                                if (DoctorCounts[doctor].DiagnosisCounts.ContainsKey(diagnosis))
                                    DoctorCounts[doctor].DiagnosisCounts[diagnosis]++;
                                else
                                    DoctorCounts[doctor].DiagnosisCounts[diagnosis] = 1;
                            }

                            // Pridanie služby
                            if (!string.IsNullOrEmpty(service))
                            {
                                if (DoctorCounts[doctor].ServiceCounts.ContainsKey(service))
                                    DoctorCounts[doctor].ServiceCounts[service]++;
                                else
                                    DoctorCounts[doctor].ServiceCounts[service] = 1;
                            }
                        }
                    }

                    // Extrakcia zariadenia s rozšírenými údajmi
                    if (facilityColumnIndex >= 0 && parts.Length > facilityColumnIndex)
                    {
                        string facility = parts[facilityColumnIndex].Trim();
                        if (!string.IsNullOrEmpty(facility))
                        {
                            if (!FacilityData.ContainsKey(facility))
                            {
                                FacilityData[facility] = new FacilityData
                                {
                                    TotalPoints = 0,
                                    RecordCount = 0,
                                    TotalValue = 0,
                                    DoctorCounts = new Dictionary<string, int>()
                                };
                            }

                            // Aktualizácia údajov o zariadení
                            FacilityData[facility].TotalPoints += points;
                            FacilityData[facility].RecordCount++;
                            FacilityData[facility].TotalValue += recordValue;

                            // Pridanie lekára k zariadeniu
                            if (doctorColumnIndex >= 0 && parts.Length > doctorColumnIndex)
                            {
                                string doctor = parts[doctorColumnIndex].Trim();
                                if (!string.IsNullOrEmpty(doctor))
                                {
                                    if (FacilityData[facility].DoctorCounts.ContainsKey(doctor))
                                        FacilityData[facility].DoctorCounts[doctor]++;
                                    else
                                        FacilityData[facility].DoctorCounts[doctor] = 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Chyba pri analýze súboru {filePath}: {ex.Message}");
            }
        }

        // Metóda pre získanie slovníkov s počtami pre spätnú kompatibilitu
        public Dictionary<string, int> GetDoctorCounts()
        {
            return DoctorCounts.ToDictionary(kv => kv.Key, kv => kv.Value.RecordCount);
        }

        public Dictionary<string, int> GetFacilityCounts()
        {
            return FacilityData.ToDictionary(kv => kv.Key, kv => kv.Value.RecordCount);
        }
    }
}