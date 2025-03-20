using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System.IO;

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
            result["AveragePoints"] = files.Count > 0 ? (double)files.Sum(f => f.Points) / files.Count : 0;
            result["MaxPoints"] = files.Count > 0 ? files.Max(f => f.Points) : 0;
            result["MinPoints"] = files.Count > 0 ? files.Min(f => f.Points) : 0;

            // Štatistiky podľa dátumov
            var byMonth = files
                .GroupBy(f => new { Month = f.ProcessedTime.Month, Year = f.ProcessedTime.Year })
                .Select(g => new {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Count = g.Count(),
                    Points = g.Sum(f => f.Points)
                })
                .OrderBy(x => x.Period)
                .ToList();

            result["PointsByMonth"] = byMonth;

            return result;
        }

        // Metóda pre porovnanie súborov
        public Dictionary<string, object> CompareFiles(List<ProcessedFileInfo> files)
        {
            var result = new Dictionary<string, object>();

            if (files == null || files.Count < 2)
                return result;

            // Porovnanie základných parametrov
            var comparison = new List<Dictionary<string, object>>();

            foreach (var file in files)
            {
                comparison.Add(new Dictionary<string, object> {
                    { "FileName", file.FileName },
                    { "ProcessedTime", file.ProcessedTime },
                    { "Points", file.Points },
                    { "PointsDiff", file.Points - files.Average(f => f.Points) },
                    { "PercentOfTotal", files.Sum(f => f.Points) > 0
                        ? (double)file.Points / files.Sum(f => f.Points) * 100
                        : 0 }
                });
            }

            result["Comparison"] = comparison;
            return result;
        }

        // Metóda pre analýzu trendov
        public Dictionary<string, object> AnalyzeTrends(List<ProcessedFileInfo> files, int months = 12)
        {
            var result = new Dictionary<string, object>();

            if (files == null || files.Count == 0)
                return result;

            // Získanie dát za posledných X mesiacov
            DateTime cutoffDate = DateTime.Now.AddMonths(-months);

            // Vytvorenie objektov s dátumom, periódou, počtom a bodmi
            var monthlyDataTemp = files
                .Where(f => f.ProcessedTime >= cutoffDate)
                .GroupBy(g => new { Month = g.ProcessedTime.Month, Year = g.ProcessedTime.Year })
                .Select(g => new {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Count = g.Count(),
                    Points = g.Sum(f => f.Points),
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1)
                })
                .OrderBy(x => x.Date)
                .ToList();

            // Konverzia na zoznam Dictionary pre jednoduchšiu serializáciu
            var monthlyData = monthlyDataTemp.Select(item => new Dictionary<string, object> {
                { "Period", item.Period },
                { "Count", item.Count },
                { "Points", item.Points },
                { "Date", item.Date }
            }).ToList();

            result["MonthlyTrendData"] = monthlyData;

            // Trend analýza - lineárna regresia
            if (monthlyDataTemp.Count > 1)
            {
                double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
                int n = monthlyDataTemp.Count;

                for (int i = 0; i < n; i++)
                {
                    double x = i;
                    double y = monthlyDataTemp[i].Points;

                    sumX += x;
                    sumY += y;
                    sumXY += x * y;
                    sumX2 += x * x;
                }

                double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
                double intercept = (sumY - slope * sumX) / n;

                result["TrendSlope"] = slope;
                result["TrendIntercept"] = intercept;
                result["TrendDirection"] = slope > 0 ? "Stúpajúci" : (slope < 0 ? "Klesajúci" : "Stabilný");

                // Predikcia na nasledujúce 3 mesiace
                var predictions = new List<Dictionary<string, object>>();
                DateTime lastDate = monthlyDataTemp.Last().Date;

                for (int i = 1; i <= 3; i++)
                {
                    DateTime predictionDate = lastDate.AddMonths(i);
                    double predictionValue = intercept + slope * (n + i - 1);

                    predictions.Add(new Dictionary<string, object> {
                        { "Period", $"{predictionDate.Year}-{predictionDate.Month:D2}" },
                        { "Points", Math.Max(0, predictionValue) }
                    });
                }

                result["Predictions"] = predictions;
            }

            return result;
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

                worksheet.Cells[5, 1].Value = "Priemerný počet bodov:";
                worksheet.Cells[5, 2].Value = files.Count > 0 ? files.Average(f => f.Points) : 0;

                worksheet.Cells[6, 1].Value = "Maximálny počet bodov:";
                worksheet.Cells[6, 2].Value = files.Count > 0 ? files.Max(f => f.Points) : 0;

                worksheet.Cells[7, 1].Value = "Minimálny počet bodov:";
                worksheet.Cells[7, 2].Value = files.Count > 0 ? files.Min(f => f.Points) : 0;

                // Formátovanie čísel
                worksheet.Cells[3, 2, 7, 2].Style.Numberformat.Format = "#,##0";

                // Detailný zoznam súborov
                worksheet.Cells[9, 1].Value = "Zoznam súborov";
                worksheet.Cells[9, 1, 9, 6].Merge = true;
                worksheet.Cells[9, 1, 9, 6].Style.Font.Bold = true;
                worksheet.Cells[9, 1, 9, 6].Style.Font.Size = 12;

                // Hlavička tabuľky
                worksheet.Cells[10, 1].Value = "Názov súboru";
                worksheet.Cells[10, 2].Value = "Dátum spracovania";
                worksheet.Cells[10, 3].Value = "Počet bodov";
                worksheet.Cells[10, 4].Value = "% z celku";
                worksheet.Cells[10, 5].Value = "Adresár";

                // Formátovanie hlavičky
                var headerRange = worksheet.Cells[10, 1, 10, 5];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                // Dáta
                int row = 11;
                int totalPoints = files.Sum(f => f.Points);

                foreach (var file in files.OrderByDescending(f => f.Points))
                {
                    worksheet.Cells[row, 1].Value = file.FileName;
                    worksheet.Cells[row, 2].Value = file.ProcessedTime;
                    worksheet.Cells[row, 3].Value = file.Points;
                    worksheet.Cells[row, 4].Value = totalPoints > 0 ? (double)file.Points / totalPoints : 0;
                    worksheet.Cells[row, 5].Value = file.SourceDirectory;

                    // Formátovanie
                    worksheet.Cells[row, 2].Style.Numberformat.Format = "dd.MM.yyyy HH:mm:ss";
                    worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[row, 4].Style.Numberformat.Format = "0.00%";

                    row++;
                }

                // Automatické prispôsobenie stĺpcov
                worksheet.Cells.AutoFitColumns();

                // Vytvorenie grafu
                var chartSheet = package.Workbook.Worksheets.Add("Grafy");

                // Stĺpcový graf pre body
                var chart = chartSheet.Drawings.AddChart("PointsChart", eChartType.ColumnClustered);
                chart.SetPosition(1, 0, 0, 0);
                chart.SetSize(800, 400);
                chart.Title.Text = "Počet bodov podľa súborov";

                // Dáta pre graf - najskôr pripravíme dáta na hárok
                chartSheet.Cells[1, 1].Value = "Súbor";
                chartSheet.Cells[1, 2].Value = "Body";

                int chartRow = 2;
                foreach (var file in files.OrderByDescending(f => f.Points).Take(10)) // Top 10 súborov
                {
                    chartSheet.Cells[chartRow, 1].Value = file.FileName;
                    chartSheet.Cells[chartRow, 2].Value = file.Points;
                    chartRow++;
                }

                // Pridanie série do grafu
                var series = chart.Series.Add(chartSheet.Cells[2, 2, chartRow - 1, 2],
                                            chartSheet.Cells[2, 1, chartRow - 1, 1]);
                series.Header = "Počet bodov";

                // Uloženie súboru
                package.SaveAs(new FileInfo(filePath));
            }
        }
    }
}
