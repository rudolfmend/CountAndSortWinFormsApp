using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using CountAndSortWinFormsAppNetFr4;
using System.Web.UI.DataVisualization.Charting;

namespace CountAndSortWinFormsAppNetFr4
{
    public class StatisticsModule
    {
        private SelectFileForm parentForm;

        // Stĺpce pre agregačné štatistiky
        private int facilityColumnIndex = 18; // Zariadenie
        private int doctorColumnIndex = 19;   // Lekár
        private int typeColumnIndex = 17;     // Typ vystavenia
        private int pointPriceColumnIndex = 33; // Cena za bod
        private int servicePriceColumnIndex = 34; // Cena za výkon

        public StatisticsModule(SelectFileForm parent)
        {
            this.parentForm = parent;
            LoadColumnSettings();
        }

        private void LoadColumnSettings()
        {
            // Tu môžeme získať uložené nastavenia stĺpcov z Settings
            // alebo ponechať predvolené hodnoty
            try
            {
                facilityColumnIndex = Properties.Settings.Default.FacilityColumnIndex - 1;
                doctorColumnIndex = Properties.Settings.Default.DoctorColumnIndex - 1;
                typeColumnIndex = Properties.Settings.Default.TypeColumnIndex - 1;
                pointPriceColumnIndex = Properties.Settings.Default.PointPriceColumnIndex - 1;
                servicePriceColumnIndex = Properties.Settings.Default.ServicePriceColumnIndex - 1;
            }
            catch
            {
                // Ak nastavenia neexistujú, použijeme predvolené hodnoty
            }
        }

        // Metóda pre generovanie rozšírenej štatistiky
        public Dictionary<string, object> GenerateExtendedStatistics(List<ProcessedFileInfo> files)
        {
            var result = new Dictionary<string, object>();

            if (files == null || files.Count == 0)
                return result;

            // Základné štatistiky
            result["TotalFiles"] = files.Count;
            result["TotalPoints"] = files.Sum(f => f.Points);
            result["TotalValue"] = files.Sum(f => f.TotalValue);
            result["AveragePoints"] = files.Count > 0 ? (double)files.Sum(f => f.Points) / files.Count : 0;
            result["AverageValue"] = files.Count > 0 ? (decimal)files.Sum(f => f.TotalValue) / files.Count : 0;
            result["MaxPoints"] = files.Count > 0 ? files.Max(f => f.Points) : 0;
            result["MinPoints"] = files.Count > 0 ? files.Min(f => f.Points) : 0;

            // Štatistiky podľa dátumov
            var byMonth = files
                .GroupBy(f => new { Month = f.ProcessedTime.Month, Year = f.ProcessedTime.Year })
                .Select(g => new {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Count = g.Count(),
                    Points = g.Sum(f => f.Points),
                    Value = g.Sum(f => f.TotalValue)
                })
                .OrderBy(x => x.Period)
                .ToList();

            result["PointsByMonth"] = byMonth;

            // Štatistiky podľa lekárov
            var doctorStats = GenerateDoctorStatistics(files);
            result["DoctorStatistics"] = doctorStats;

            // Štatistiky podľa zariadení
            var facilityStats = GenerateFacilityStatistics(files);
            result["FacilityStatistics"] = facilityStats;

            // Štatistiky podľa diagnóz
            var diagnosisStats = GenerateDiagnosisStatistics(files);
            result["DiagnosisStatistics"] = diagnosisStats;

            return result;
        }

        // Metóda pre získanie štatistík podľa lekárov
        public List<Dictionary<string, object>> GenerateDoctorStatistics(List<ProcessedFileInfo> files)
        {
            // Slovník pre agregáciu údajov o lekároch zo všetkých súborov
            var doctorAggregate = new Dictionary<string, DoctorStat>();

            // Zhromaždenie údajov o lekároch zo všetkých súborov
            foreach (var file in files)
            {
                foreach (var doctorKV in file.DoctorCounts)
                {
                    string doctorName = doctorKV.Key;
                    int doctorCount = doctorKV.Value.RecordCount;

                    if (!doctorAggregate.ContainsKey(doctorName))
                    {
                        doctorAggregate[doctorName] = new DoctorStat
                        {
                            Name = doctorName,
                            TotalPoints = 0,
                            TotalRecords = 0,
                            TotalValue = 0,
                            DiagnosisCounts = new Dictionary<string, int>(),
                            ServiceCounts = new Dictionary<string, int>()
                        };
                    }

                    // Aktualizácia súhrnných štatistík
                    // V pôvodnej ProcessedFileInfo máme len počet výskytov lekára, nie komplexný objekt
                    doctorAggregate[doctorName].TotalRecords += doctorCount;

                    // Pre body a hodnotu musíme použiť iný prístup, keďže tieto údaje nemáme priamo
                    // Môžeme odhadnúť body na základe počtu výskytov alebo priamo z file.Points
                    // (Toto je len príklad, upravte podľa vašej logiky)
                    doctorAggregate[doctorName].TotalPoints += file.Points / Math.Max(1, file.DoctorCounts.Count);
                    doctorAggregate[doctorName].TotalValue += file.TotalValue / Math.Max(1, file.DoctorCounts.Count);

                    // Pre diagnózy a služby potrebujeme prístup k údajom, ktoré ešte nemáme
                    // Musíme upraviť ProcessedFileInfo alebo použiť iný prístup
                }

                // Pre každú diagnózu skontrolujeme, ktorému lekárovi patrí
                // (Toto je len príklad, upravte podľa vašej logiky)
                foreach (var diagnosisKV in file.DiagnosisCounts)
                {
                    // Priradíme diagnózu k lekárom proporčne
                    foreach (var doctorKV in file.DoctorCounts)
                    {
                        string doctorName = doctorKV.Key;

                        if (!doctorAggregate.ContainsKey(doctorName))
                            continue;

                        if (!doctorAggregate[doctorName].DiagnosisCounts.ContainsKey(diagnosisKV.Key))
                            doctorAggregate[doctorName].DiagnosisCounts[diagnosisKV.Key] = 0;

                        // Jednoduché proporcionálne rozdelenie
                        doctorAggregate[doctorName].DiagnosisCounts[diagnosisKV.Key] +=
                            diagnosisKV.Value / Math.Max(1, file.DoctorCounts.Count);
                    }
                }

                // Podobne pre služby
                foreach (var serviceKV in file.ServiceCounts)
                {
                    foreach (var doctorKV in file.DoctorCounts)
                    {
                        string doctorName = doctorKV.Key;

                        if (!doctorAggregate.ContainsKey(doctorName))
                            continue;

                        if (!doctorAggregate[doctorName].ServiceCounts.ContainsKey(serviceKV.Key))
                            doctorAggregate[doctorName].ServiceCounts[serviceKV.Key] = 0;

                        doctorAggregate[doctorName].ServiceCounts[serviceKV.Key] +=
                            serviceKV.Value / Math.Max(1, file.DoctorCounts.Count);
                    }
                }
            }

            // Konverzia na zoznam Dictionary pre export
            var result = doctorAggregate.Values
                .OrderByDescending(d => d.TotalPoints)
                .Select(d => new Dictionary<string, object>
                {
            { "Name", d.Name },
            { "TotalPoints", d.TotalPoints },
            { "TotalRecords", d.TotalRecords },
            { "TotalValue", d.TotalValue },
            { "AverageValuePerRecord", d.TotalRecords > 0 ? d.TotalValue / d.TotalRecords : 0 },
            { "TopDiagnoses", d.DiagnosisCounts.OrderByDescending(kv => kv.Value).Take(5).ToDictionary(kv => kv.Key, kv => kv.Value) },
            { "TopServices", d.ServiceCounts.OrderByDescending(kv => kv.Value).Take(5).ToDictionary(kv => kv.Key, kv => kv.Value) }
                })
                .ToList();

            return result;
        }

        // Metóda pre získanie štatistík podľa zariadení
        public List<Dictionary<string, object>> GenerateFacilityStatistics(List<ProcessedFileInfo> files)
        {
            // Slovník pre agregáciu údajov o zariadeniach zo všetkých súborov
            var facilityAggregate = new Dictionary<string, FacilityStat>();

            // Zhromaždenie údajov o zariadeniach zo všetkých súborov
            foreach (var file in files)
            {
                foreach (var facilityKV in file.FacilityData)
                {
                    string facilityName = facilityKV.Key;
                    var facilityData = facilityKV.Value; // Je to objekt FacilityData, nie int

                    if (!facilityAggregate.ContainsKey(facilityName))
                    {
                        facilityAggregate[facilityName] = new FacilityStat
                        {
                            Name = facilityName,
                            TotalPoints = 0,
                            TotalRecords = 0,
                            TotalValue = 0,
                            DoctorCounts = new Dictionary<string, int>()
                        };
                    }

                    // Aktualizácia súhrnných štatistík - použijeme údaje priamo z FacilityData
                    facilityAggregate[facilityName].TotalRecords += facilityData.RecordCount;
                    facilityAggregate[facilityName].TotalPoints += facilityData.TotalPoints;
                    facilityAggregate[facilityName].TotalValue += facilityData.TotalValue;

                    // Zlúčenie lekárov - použijeme DoctorCounts z FacilityData
                    foreach (var doctorKV in facilityData.DoctorCounts)
                    {
                        string doctorName = doctorKV.Key;
                        int doctorCount = doctorKV.Value; // Toto je už int

                        if (!facilityAggregate[facilityName].DoctorCounts.ContainsKey(doctorName))
                            facilityAggregate[facilityName].DoctorCounts[doctorName] = 0;

                        facilityAggregate[facilityName].DoctorCounts[doctorName] += doctorCount;
                    }
                }
            }

            // Konverzia na zoznam Dictionary pre export
            var result = facilityAggregate.Values
                .OrderByDescending(f => f.TotalPoints)
                .Select(f => new Dictionary<string, object>
                {
            { "Name", f.Name },
            { "TotalPoints", f.TotalPoints },
            { "TotalRecords", f.TotalRecords },
            { "TotalValue", f.TotalValue },
            { "AverageValuePerRecord", f.TotalRecords > 0 ? f.TotalValue / f.TotalRecords : 0 },
            { "TopDoctors", f.DoctorCounts.OrderByDescending(kv => kv.Value).Take(5).ToDictionary(kv => kv.Key, kv => kv.Value) }
                })
                .ToList();

            return result;
        }

        // Metóda pre získanie štatistík podľa diagnóz
        public Dictionary<string, object> GenerateDiagnosisStatistics(List<ProcessedFileInfo> files)
        {
            // Slovník pre agregáciu diagnóz zo všetkých súborov
            var diagnosisAggregate = new Dictionary<string, int>();

            // Zhromaždenie diagnóz zo všetkých súborov
            foreach (var file in files)
            {
                foreach (var diagnosis in file.DiagnosisCounts)
                {
                    if (!diagnosisAggregate.ContainsKey(diagnosis.Key))
                        diagnosisAggregate[diagnosis.Key] = 0;
                    diagnosisAggregate[diagnosis.Key] += diagnosis.Value;
                }
            }

            // Zostupné zoradenie podľa počtu
            var topDiagnoses = diagnosisAggregate
                .OrderByDescending(kv => kv.Value)
                .Take(20)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            // Vrátenie výsledkov
            return new Dictionary<string, object>
            {
                { "TotalDiagnosesCount", diagnosisAggregate.Count },
                { "TopDiagnoses", topDiagnoses }
            };
        }

        // Metóda pre porovnanie súborov
        public Dictionary<string, object> CompareFiles(List<ProcessedFileInfo> files)
        {
            // Existujúci kód...
            return new Dictionary<string, object>();
        }

        // Metóda pre analýzu trendov
        public Dictionary<string, object> AnalyzeTrends(List<ProcessedFileInfo> files, int months = 12)
        {
            // Existujúci kód...
            return new Dictionary<string, object>();
        }

        // Metóda pre export rozšírených štatistík do Excelu
        public void ExportExtendedStatistics(List<ProcessedFileInfo> files, string filePath)
        {
            using (var package = new ExcelPackage())
            {
                // Hlavný hárok so základnými dátami
                var worksheet = package.Workbook.Worksheets.Add("Štatistiky");

                // Základné informácie o spracovaných súboroch
                worksheet.Cells[1, 1].Value = "Rozšírená štatistika";
                worksheet.Cells[1, 1, 1, 6].Merge = true;
                worksheet.Cells[1, 1, 1, 6].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 1, 6].Style.Font.Size = 14;
                worksheet.Cells[1, 1, 1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells[3, 1].Value = "Celkový počet súborov:";
                worksheet.Cells[3, 2].Value = files.Count;

                worksheet.Cells[4, 1].Value = "Celkový počet bodov:";
                worksheet.Cells[4, 2].Value = files.Sum(f => f.Points);

                worksheet.Cells[5, 1].Value = "Celková hodnota:";
                worksheet.Cells[5, 2].Value = files.Sum(f => f.TotalValue);

                worksheet.Cells[6, 1].Value = "Priemerný počet bodov:";
                worksheet.Cells[6, 2].Value = files.Count > 0 ? files.Average(f => f.Points) : 0;

                worksheet.Cells[7, 1].Value = "Maximálny počet bodov:";
                worksheet.Cells[7, 2].Value = files.Count > 0 ? files.Max(f => f.Points) : 0;

                worksheet.Cells[8, 1].Value = "Minimálny počet bodov:";
                worksheet.Cells[8, 2].Value = files.Count > 0 ? files.Min(f => f.Points) : 0;

                // Formátovanie čísel
                worksheet.Cells[3, 2, 4, 2].Style.Numberformat.Format = "#,##0";
                worksheet.Cells[5, 2].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[6, 2, 8, 2].Style.Numberformat.Format = "#,##0";

                // Detailný zoznam súborov
                worksheet.Cells[10, 1].Value = "Zoznam súborov";
                worksheet.Cells[10, 1, 10, 6].Merge = true;
                worksheet.Cells[10, 1, 10, 6].Style.Font.Bold = true;
                worksheet.Cells[10, 1, 10, 6].Style.Font.Size = 12;

                // Hlavička tabuľky
                worksheet.Cells[11, 1].Value = "Názov súboru";
                worksheet.Cells[11, 2].Value = "Dátum spracovania";
                worksheet.Cells[11, 3].Value = "Počet bodov";
                worksheet.Cells[11, 4].Value = "Hodnota";
                worksheet.Cells[11, 5].Value = "% z celku";
                worksheet.Cells[11, 6].Value = "Adresár";

                // Formátovanie hlavičky
                var headerRange = worksheet.Cells[11, 1, 11, 6];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                // Dáta
                int row = 12;
                int totalPoints = files.Sum(f => f.Points);

                foreach (var file in files.OrderByDescending(f => f.Points))
                {
                    worksheet.Cells[row, 1].Value = file.FileName;
                    worksheet.Cells[row, 2].Value = file.ProcessedTime;
                    worksheet.Cells[row, 3].Value = file.Points;
                    worksheet.Cells[row, 4].Value = file.TotalValue;
                    worksheet.Cells[row, 5].Value = totalPoints > 0 ? (double)file.Points / totalPoints : 0;
                    worksheet.Cells[row, 6].Value = file.SourceDirectory;

                    // Formátovanie
                    worksheet.Cells[row, 2].Style.Numberformat.Format = "dd.MM.yyyy HH:mm:ss";
                    worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                    worksheet.Cells[row, 5].Style.Numberformat.Format = "0.00%";

                    row++;
                }

                // Automatické prispôsobenie stĺpcov
                worksheet.Cells.AutoFitColumns();

                // Hárok pre lekárov
                ExportDoctorStatistics(package, files);

                // Hárok pre zariadenia
                ExportFacilityStatistics(package, files);

                // Hárok pre diagnózy
                ExportDiagnosisStatistics(package, files);

                // Vytvorenie grafov
                var chartSheet = CreateCharts(package, files);

                // Uloženie súboru
                package.SaveAs(new FileInfo(filePath));
            }
        }

        // Metóda pre export štatistík lekárov
        private void ExportDoctorStatistics(ExcelPackage package, List<ProcessedFileInfo> files)
        {
            var doctorSheet = package.Workbook.Worksheets.Add("Lekári");
            var doctorStats = GenerateDoctorStatistics(files);

            // Hlavička
            doctorSheet.Cells[1, 1].Value = "Štatistiky podľa lekárov";
            doctorSheet.Cells[1, 1, 1, 7].Merge = true;
            doctorSheet.Cells[1, 1, 1, 7].Style.Font.Bold = true;
            doctorSheet.Cells[1, 1, 1, 7].Style.Font.Size = 14;
            doctorSheet.Cells[1, 1, 1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            // Hlavička tabuľky
            doctorSheet.Cells[3, 1].Value = "Lekár";
            doctorSheet.Cells[3, 2].Value = "Počet bodov";
            doctorSheet.Cells[3, 3].Value = "Počet záznamov";
            doctorSheet.Cells[3, 4].Value = "Celková hodnota";
            doctorSheet.Cells[3, 5].Value = "Priemerná hodnota";
            doctorSheet.Cells[3, 6].Value = "Top diagnózy";
            doctorSheet.Cells[3, 7].Value = "Top výkony";

            // Formátovanie hlavičky
            var headerRange = doctorSheet.Cells[3, 1, 3, 7];
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            // Dáta
            int row = 4;
            foreach (var doctor in doctorStats)
            {
                doctorSheet.Cells[row, 1].Value = doctor["Name"].ToString();
                doctorSheet.Cells[row, 2].Value = Convert.ToInt32(doctor["TotalPoints"]);
                doctorSheet.Cells[row, 3].Value = Convert.ToInt32(doctor["TotalRecords"]);
                doctorSheet.Cells[row, 4].Value = Convert.ToDecimal(doctor["TotalValue"]);
                doctorSheet.Cells[row, 5].Value = Convert.ToDecimal(doctor["AverageValuePerRecord"]);

                // Top diagnózy
                var topDiagnoses = doctor["TopDiagnoses"] as Dictionary<string, int>;
                if (topDiagnoses != null)
                {
                    var diagnosisText = string.Join(", ", topDiagnoses.Take(3).Select(d => $"{d.Key} ({d.Value})"));
                    doctorSheet.Cells[row, 6].Value = diagnosisText;
                }

                // Top výkony
                var topServices = doctor["TopServices"] as Dictionary<string, int>;
                if (topServices != null)
                {
                    var servicesText = string.Join(", ", topServices.Take(3).Select(s => $"{s.Key} ({s.Value})"));
                    doctorSheet.Cells[row, 7].Value = servicesText;
                }

                // Formátovanie
                doctorSheet.Cells[row, 2].Style.Numberformat.Format = "#,##0";
                doctorSheet.Cells[row, 3].Style.Numberformat.Format = "#,##0";
                doctorSheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                doctorSheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";

                row++;
            }

            // Automatické prispôsobenie stĺpcov
            doctorSheet.Cells.AutoFitColumns();
        }

        // Metóda pre export štatistík zariadení
        private void ExportFacilityStatistics(ExcelPackage package, List<ProcessedFileInfo> files)
        {
            var facilitySheet = package.Workbook.Worksheets.Add("Zariadenia");
            var facilityStats = GenerateFacilityStatistics(files);

            // Hlavička
            facilitySheet.Cells[1, 1].Value = "Štatistiky podľa zariadení";
            facilitySheet.Cells[1, 1, 1, 6].Merge = true;
            facilitySheet.Cells[1, 1, 1, 6].Style.Font.Bold = true;
            facilitySheet.Cells[1, 1, 1, 6].Style.Font.Size = 14;
            facilitySheet.Cells[1, 1, 1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            // Hlavička tabuľky
            facilitySheet.Cells[3, 1].Value = "Zariadenie";
            facilitySheet.Cells[3, 2].Value = "Počet bodov";
            facilitySheet.Cells[3, 3].Value = "Počet záznamov";
            facilitySheet.Cells[3, 4].Value = "Celková hodnota";
            facilitySheet.Cells[3, 5].Value = "Priemerná hodnota";
            facilitySheet.Cells[3, 6].Value = "Top lekári";

            // Formátovanie hlavičky
            var headerRange = facilitySheet.Cells[3, 1, 3, 6];
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            // Dáta
            int row = 4;
            foreach (var facility in facilityStats)
            {
                facilitySheet.Cells[row, 1].Value = facility["Name"].ToString();
                facilitySheet.Cells[row, 2].Value = Convert.ToInt32(facility["TotalPoints"]);
                facilitySheet.Cells[row, 3].Value = Convert.ToInt32(facility["TotalRecords"]);
                facilitySheet.Cells[row, 4].Value = Convert.ToDecimal(facility["TotalValue"]);
                facilitySheet.Cells[row, 5].Value = Convert.ToDecimal(facility["AverageValuePerRecord"]);

                // Top lekári
                var topDoctors = facility["TopDoctors"] as Dictionary<string, int>;
                if (topDoctors != null)
                {
                    var doctorsText = string.Join(", ", topDoctors.Take(3).Select(d => $"{d.Key} ({d.Value})"));
                    facilitySheet.Cells[row, 6].Value = doctorsText;
                }

                // Formátovanie
                facilitySheet.Cells[row, 2].Style.Numberformat.Format = "#,##0";
                facilitySheet.Cells[row, 3].Style.Numberformat.Format = "#,##0";
                facilitySheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                facilitySheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";

                row++;
            }

            // Automatické prispôsobenie stĺpcov
            facilitySheet.Cells.AutoFitColumns();
        }

        // Metóda pre export štatistík diagnóz
        private void ExportDiagnosisStatistics(ExcelPackage package, List<ProcessedFileInfo> files)
        {
            var diagnosisSheet = package.Workbook.Worksheets.Add("Diagnózy");
            var diagnosisStats = GenerateDiagnosisStatistics(files);

            // Hlavička
            diagnosisSheet.Cells[1, 1].Value = "Štatistiky podľa diagnóz";
            diagnosisSheet.Cells[1, 1, 1, 3].Merge = true;
            diagnosisSheet.Cells[1, 1, 1, 3].Style.Font.Bold = true;
            diagnosisSheet.Cells[1, 1, 1, 3].Style.Font.Size = 14;
            diagnosisSheet.Cells[1, 1, 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            // Základná informácia
            diagnosisSheet.Cells[3, 1].Value = "Celkový počet diagnóz:";
            diagnosisSheet.Cells[3, 2].Value = diagnosisStats["TotalDiagnosesCount"];
            diagnosisSheet.Cells[3, 2].Style.Numberformat.Format = "#,##0";

            // Hlavička tabuľky
            diagnosisSheet.Cells[5, 1].Value = "Diagnóza";
            diagnosisSheet.Cells[5, 2].Value = "Počet výskytov";
            diagnosisSheet.Cells[5, 3].Value = "Percento";

            // Formátovanie hlavičky
            var headerRange = diagnosisSheet.Cells[5, 1, 5, 3];
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

            // Dáta
            int row = 6;
            var topDiagnoses = diagnosisStats["TopDiagnoses"] as Dictionary<string, int>;
            int totalDiagnoses = topDiagnoses.Values.Sum();

            foreach (var diagnosis in topDiagnoses)
            {
                diagnosisSheet.Cells[row, 1].Value = diagnosis.Key;
                diagnosisSheet.Cells[row, 2].Value = diagnosis.Value;
                diagnosisSheet.Cells[row, 3].Value = (double)diagnosis.Value / totalDiagnoses;

                // Formátovanie
                diagnosisSheet.Cells[row, 2].Style.Numberformat.Format = "#,##0";
                diagnosisSheet.Cells[row, 3].Style.Numberformat.Format = "0.00%";

                row++;
            }

            // Automatické prispôsobenie stĺpcov
            diagnosisSheet.Cells.AutoFitColumns();
        }

        // Metóda pre vytvorenie grafov
        private ExcelWorksheet CreateCharts(ExcelPackage package, List<ProcessedFileInfo> files)
        {
            var chartSheet = package.Workbook.Worksheets.Add("Grafy");

            // 1. Graf pre top lekárov
            AddDoctorChart(chartSheet, files);

            // 2. Graf pre top zariadenia
            AddFacilityChart(chartSheet, files);

            // 3. Graf pre mesačné trendy
            AddMonthlyTrendsChart(chartSheet, files);

            // 4. Graf pre diagnózy
            AddDiagnosisChart(chartSheet, files);

            return chartSheet;
        }

        // Metóda pre pridanie grafu lekárov
        private void AddDoctorChart(ExcelWorksheet chartSheet, List<ProcessedFileInfo> files)
        {
            // Získame top 10 lekárov
            var doctorStats = GenerateDoctorStatistics(files).Take(10).ToList();

            // Pripravíme dáta pre graf
            chartSheet.Cells[1, 1].Value = "Lekár";
            chartSheet.Cells[1, 2].Value = "Body";

            for (int i = 0; i < doctorStats.Count; i++)
            {
                chartSheet.Cells[2 + i, 1].Value = doctorStats[i]["Name"].ToString();
                chartSheet.Cells[2 + i, 2].Value = Convert.ToInt32(doctorStats[i]["TotalPoints"]);
            }

            // Vytvoríme graf
            var doctorChart = chartSheet.Drawings.AddChart("DoctorChart", eChartType.ColumnClustered);
            doctorChart.SetPosition(1, 0, 3, 0);
            doctorChart.SetSize(800, 400);
            doctorChart.Title.Text = "Top 10 lekárov podľa bodov";

            // Pridáme sériu
            var series = doctorChart.Series.Add(chartSheet.Cells[2, 2, 1 + doctorStats.Count, 2],
                                             chartSheet.Cells[2, 1, 1 + doctorStats.Count, 1]);
            series.Header = "Počet bodov";
        }

        // Metóda pre pridanie grafu zariadení
        private void AddFacilityChart(ExcelWorksheet chartSheet, List<ProcessedFileInfo> files)
        {
            // Získame top 10 zariadení
            var facilityStats = GenerateFacilityStatistics(files).Take(10).ToList();

            // Pripravíme dáta pre graf
            chartSheet.Cells[15, 1].Value = "Zariadenie";
            chartSheet.Cells[15, 2].Value = "Body";

            for (int i = 0; i < facilityStats.Count; i++)
            {
                chartSheet.Cells[16 + i, 1].Value = facilityStats[i]["Name"].ToString();
                chartSheet.Cells[16 + i, 2].Value = Convert.ToInt32(facilityStats[i]["TotalPoints"]);
            }

            // Vytvoríme graf
            var facilityChart = chartSheet.Drawings.AddChart("FacilityChart", eChartType.ColumnClustered);
            facilityChart.SetPosition(21, 0, 3, 0);
            facilityChart.SetSize(800, 400);
            facilityChart.Title.Text = "Top 10 zariadení podľa bodov";

            // Pridáme sériu
            var series = facilityChart.Series.Add(chartSheet.Cells[16, 2, 15 + facilityStats.Count, 2],
                                              chartSheet.Cells[16, 1, 15 + facilityStats.Count, 1]);
            series.Header = "Počet bodov";
        }

        // Metóda pre pridanie grafu mesačných trendov
        private void AddMonthlyTrendsChart(ExcelWorksheet chartSheet, List<ProcessedFileInfo> files)
        {
            // Získame mesačné dáta
            var byMonth = files
                .GroupBy(f => new { Month = f.ProcessedTime.Month, Year = f.ProcessedTime.Year })
                .Select(g => new {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Points = g.Sum(f => f.Points),
                    Value = g.Sum(f => f.TotalValue)
                })
                .OrderBy(x => x.Date)
                .ToList();

            // Pripravíme dáta pre graf
            chartSheet.Cells[30, 1].Value = "Obdobie";
            chartSheet.Cells[30, 2].Value = "Body";
            chartSheet.Cells[30, 3].Value = "Hodnota";

            for (int i = 0; i < byMonth.Count; i++)
            {
                chartSheet.Cells[31 + i, 1].Value = byMonth[i].Period;
                chartSheet.Cells[31 + i, 2].Value = byMonth[i].Points;
                chartSheet.Cells[31 + i, 3].Value = byMonth[i].Value;
            }

            // Vytvoríme graf
            var trendChart = chartSheet.Drawings.AddChart("TrendChart", eChartType.LineMarkers);
            trendChart.SetPosition(41, 0, 3, 0);
            trendChart.SetSize(800, 400);
            trendChart.Title.Text = "Mesačný trend bodov a hodnoty";

            // Nastavíme sekundárnu os
            trendChart.UseSecondaryAxis = true;

            // Pridáme série
            var pointsSeries = trendChart.Series.Add(chartSheet.Cells[31, 2, 30 + byMonth.Count, 2],
                                                 chartSheet.Cells[31, 1, 30 + byMonth.Count, 1]);
            pointsSeries.Header = "Počet bodov";

            var valueSeries = trendChart.Series.Add(chartSheet.Cells[31, 3, 30 + byMonth.Count, 3],
                                                chartSheet.Cells[31, 1, 30 + byMonth.Count, 1]);
            valueSeries.Header = "Hodnota";

            //valueSeries.UseSecondaryAxis = true;

            // Nastavenie série na sekundárnu os
            //valueSeries.YAxisType = eAxisType.Secondary; // alebo AxisType.Secondary podľa vašej verzie

            // Diagnostické informácie o type
            System.Diagnostics.Debug.WriteLine($"Typ série: {valueSeries.GetType().FullName}");
            var properties = valueSeries.GetType().GetProperties();
            System.Diagnostics.Debug.WriteLine("Dostupné vlastnosti:");
            foreach (var prop in properties)
            {
                System.Diagnostics.Debug.WriteLine($" - {prop.Name}: {prop.PropertyType.Name}");
            }
        }

        // Metóda pre pridanie grafu diagnóz
        private void AddDiagnosisChart(ExcelWorksheet chartSheet, List<ProcessedFileInfo> files)
        {
            // Získame top diagnózy
            var diagnosisStats = GenerateDiagnosisStatistics(files);
            var topDiagnoses = (diagnosisStats["TopDiagnoses"] as Dictionary<string, int>)?.Take(10).ToList();

            if (topDiagnoses != null && topDiagnoses.Any())
            {
                // Pripravíme dáta pre graf
                chartSheet.Cells[60, 1].Value = "Diagnóza";
                chartSheet.Cells[60, 2].Value = "Počet";

                for (int i = 0; i < topDiagnoses.Count; i++)
                {
                    chartSheet.Cells[61 + i, 1].Value = topDiagnoses[i].Key;
                    chartSheet.Cells[61 + i, 2].Value = topDiagnoses[i].Value;
                }

                // Vytvoríme graf
                var diagnosisChart = chartSheet.Drawings.AddChart("DiagnosisChart", eChartType.Pie);
                diagnosisChart.SetPosition(61, 0, 3, 0);
                diagnosisChart.SetSize(800, 600);
                diagnosisChart.Title.Text = "Top 10 diagnóz";

                // Pridáme sériu
                var series = diagnosisChart.Series.Add(chartSheet.Cells[61, 2, 60 + topDiagnoses.Count, 2],
                                                   chartSheet.Cells[61, 1, 60 + topDiagnoses.Count, 1]);
                series.Header = "Počet výskytov";
            }
        }

        // Pomocné triedy pre agregáciu štatistík
        private class DoctorStat
        {
            public string Name { get; set; }
            public int TotalPoints { get; set; }
            public int TotalRecords { get; set; }
            public decimal TotalValue { get; set; }
            public Dictionary<string, int> DiagnosisCounts { get; set; } = new Dictionary<string, int>();
            public Dictionary<string, int> ServiceCounts { get; set; } = new Dictionary<string, int>();
        }

        private class FacilityStat
        {
            public string Name { get; set; }
            public int TotalPoints { get; set; }
            public int TotalRecords { get; set; }
            public decimal TotalValue { get; set; }
            public Dictionary<string, int> DoctorCounts { get; set; } = new Dictionary<string, int>();
        }
    }
}
