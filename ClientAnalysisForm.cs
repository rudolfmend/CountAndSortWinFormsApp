using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Text;

// EPPlus pre .NET Framework 4.0
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing.Chart;

namespace CountAndSortWinFormsAppNetFr4
{
    public partial class ClientAnalysisForm
    {
        // V starších verziách EPPlus nie je LicenseContext v rovnakom namespace
        // Použijeme alternatívny spôsob nastavenia licencie, ktorý funguje pre verziu 4.5.3.3

        // Metóda pre export do Excel s EPPlus
        private void ExportWithEPPlus(string filePath)
        {
            using (var package = new ExcelPackage())
            {
                // Hlavný hárok s údajmi
                var worksheet = package.Workbook.Worksheets.Add("Client Analysis");

                // Nastavenie hlavičky
                string[] headers = GetColumnHeaders();

                // Vizuálne formátovanie
                var headerFont = worksheet.Cells[1, 1, 1, headers.Length].Style.Font;
                headerFont.Bold = true;
                headerFont.Size = 12;

                // Pozadie hlavičky
                var headerCells = worksheet.Cells[1, 1, 1, headers.Length];
                headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                // Ohraničenie hlavičky
                var headerBorder = headerCells.Style.Border;
                headerBorder.Top.Style = headerBorder.Right.Style =
                headerBorder.Bottom.Style = headerBorder.Left.Style = ExcelBorderStyle.Thin;

                // Vloženie názvov stĺpcov
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                // Zotriedenie klientov - použijeme lokálnu premennú clients
                var sortedClients = this.clients.OrderByDescending(c => c.TotalPoints).ToList();

                // Naplnenie dát
                for (int i = 0; i < sortedClients.Count; i++)
                {
                    var client = sortedClients[i];
                    int row = i + 2; // Začíname od riadku 2 (po hlavičke)

                    worksheet.Cells[row, 1].Value = i + 1; // ID
                    worksheet.Cells[row, 2].Value = $"{client.Name} ({client.PersonalId})"; // Klient
                    worksheet.Cells[row, 3].Value = client.ServiceCount; // Služby
                    worksheet.Cells[row, 4].Value = client.TotalPoints; // Body
                    worksheet.Cells[row, 5].Value = client.TotalValue; // Hodnota
                    worksheet.Cells[row, 6].Value = client.AveragePointsPerService; // Priemer

                    // Formátovanie čísel
                    worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00 €";
                    worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.0";
                }

                // Pridanie súhrnného riadku
                //int totalRow = sortedClients.Count + 2;
                int totalRow = sortedClients.Count() + 2;

                // Formátovanie súhrnného riadku
                var totalRowCells = worksheet.Cells[totalRow, 1, totalRow, headers.Length];
                totalRowCells.Style.Font.Bold = true;
                totalRowCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                totalRowCells.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                worksheet.Cells[totalRow, 2].Value = Properties.Strings.LabelTotalSum ?? "SPOLU";
                worksheet.Cells[totalRow, 3].Value = clients.Sum(c => c.ServiceCount);
                worksheet.Cells[totalRow, 4].Value = clients.Sum(c => c.TotalPoints);
                worksheet.Cells[totalRow, 5].Value = clients.Sum(c => c.TotalValue);

                // Formátovanie súhrnného riadku
                worksheet.Cells[totalRow, 3].Style.Numberformat.Format = "#,##0";
                worksheet.Cells[totalRow, 4].Style.Numberformat.Format = "#,##0";
                worksheet.Cells[totalRow, 5].Style.Numberformat.Format = "#,##0.00 €";

                // Automatické prispôsobenie šírky stĺpcov
                worksheet.Cells.AutoFitColumns();

                // Hárok s údajmi pre grafy
                var chartDataSheet = package.Workbook.Worksheets.Add("Chart Data");

                // Vloženie dát pre grafy
                var top10Clients = sortedClients.Take(10).ToList();

                // Hlavička pre dáta grafu
                chartDataSheet.Cells[1, 1].Value = "Client";
                chartDataSheet.Cells[1, 2].Value = "Points";
                chartDataSheet.Cells[1, 3].Value = "Value (€)";
                chartDataSheet.Cells[1, 4].Value = "Services";

                // Vloženie dát pre top 10 klientov
                for (int i = 0; i < top10Clients.Count; i++)
                {
                    var client = top10Clients[i];
                    int row = i + 2;

                    // Skrátený názov pre lepšiu čitateľnosť v grafe
                    string shortName = client.Name.Length > 15
                        ? client.Name.Substring(0, 12) + "..."
                        : client.Name;

                    chartDataSheet.Cells[row, 1].Value = shortName;
                    chartDataSheet.Cells[row, 2].Value = client.TotalPoints;
                    chartDataSheet.Cells[row, 3].Value = client.TotalValue;
                    chartDataSheet.Cells[row, 4].Value = client.ServiceCount;
                }

                // Automatické prispôsobenie stĺpcov pre dáta grafu
                chartDataSheet.Cells.AutoFitColumns();

                try
                {
                    // Vytvorenie hárku pre grafy
                    var chartsSheet = package.Workbook.Worksheets.Add("Charts");

                    // Vytvorenie stĺpcového grafu pre body
                    var pointsChart = chartsSheet.Drawings.AddChart("PointsChart", eChartType.ColumnClustered);
                    pointsChart.SetPosition(1, 0, 0, 0);
                    pointsChart.SetSize(700, 400);

                    // Názov grafu
                    pointsChart.Title.Text = "Top 10 Clients by Points";
                    pointsChart.Title.Font.Size = 14;
                    pointsChart.Title.Font.Bold = true;

                    // Dáta pre graf
                    var pointsSeries = pointsChart.Series.Add(
                        chartDataSheet.Cells[2, 2, Math.Min(11, sortedClients.Count + 1), 2],
                        chartDataSheet.Cells[2, 1, Math.Min(11, sortedClients.Count + 1), 1]
                    );
                    pointsSeries.Header = "Total Points";

                    // Vytvorenie koláčového grafu
                    var pieChart = chartsSheet.Drawings.AddChart("PieChart", eChartType.Pie);
                    pieChart.SetPosition(25, 0, 0, 0);  // Pod stĺpcovým grafom
                    pieChart.SetSize(500, 400);

                    // Názov grafu
                    pieChart.Title.Text = "Points Distribution";
                    pieChart.Title.Font.Size = 14;
                    pieChart.Title.Font.Bold = true;

                    // Dáta pre koláčový graf

                    int clientsCount = sortedClients.Count(); // Explicitne voláme metódu Count() // Explicitne získame počet
                    var pieSeries = pieChart.Series.Add(
                        chartDataSheet.Cells[2, 2, Math.Min(11, clientsCount + 1), 2],
                        chartDataSheet.Cells[2, 1, Math.Min(11, clientsCount + 1), 1]
                    );

                    // Nastavenia pre koláčový graf - opatrne s API, ktoré nemusí existovať v starších verziách
                    try
                    {
                        var dataLabel = pieChart.DataLabel;
                        if (dataLabel != null)
                        {
                            dataLabel.ShowPercent = true;
                        }
                    }
                    catch { /* Ignorujeme chyby s DataLabel, ak API nefunguje */ }

                    try
                    {
                        pieChart.Legend.Position = eLegendPosition.Right;
                    }
                    catch { /* Ignorujeme chyby s legendou, ak API nefunguje */ }

                    // Vytvorenie stĺpcového grafu pre hodnoty
                    var valueChart = chartsSheet.Drawings.AddChart("ValueChart", eChartType.ColumnClustered);
                    valueChart.SetPosition(50, 0, 0, 0);  // Pod koláčovým grafom
                    valueChart.SetSize(700, 400);

                    // Názov grafu
                    valueChart.Title.Text = "Top 10 Clients by Value (€)";
                    valueChart.Title.Font.Size = 14;
                    valueChart.Title.Font.Bold = true;

                    // Dáta pre graf
                    var valueSeries = valueChart.Series.Add(
                        chartDataSheet.Cells[2, 3, Math.Min(11, sortedClients.Count + 1), 3],
                        chartDataSheet.Cells[2, 1, Math.Min(11, sortedClients.Count + 1), 1]
                    );
                    valueSeries.Header = "Total Value (€)";
                }
                catch (Exception graphEx)
                {
                    // Ak sa vyskytne chyba pri vytváraní grafov, len ju zaznamenáme, ale pokračujeme v exporte
                    System.Diagnostics.Debug.WriteLine("Chyba pri vytváraní grafov: " + graphEx.Message);
                }

                // Uloženie súboru
                package.SaveAs(new FileInfo(filePath));
            }
        }

        // Aktualizovaná metóda ExportButton_Click pre použitie s EPPlus
        private void ExportButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveDialog.FilterIndex = 1;
                saveDialog.RestoreDirectory = true;
                saveDialog.Title = Properties.Strings.MessageExportDataDialogTitle;
                saveDialog.FileName = $"ClientAnalysis_{DateTime.Now:yyyy-MM-dd}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveDialog.FileName;
                    this.Cursor = Cursors.WaitCursor; // Správny spôsob nastavenia kurzora na formulári

                    try
                    {
                        string extension = Path.GetExtension(filePath).ToLower();

                        switch (extension)
                        {
                            case ".xlsx":
                                // Kontrola, či je dostupný EPPlus
                                Type excelPackageType = Type.GetType("OfficeOpenXml.ExcelPackage, EPPlus", false);

                                if (excelPackageType != null)
                                {
                                    // Použiť EPPlus implementáciu
                                    ExportWithEPPlus(filePath);
                                }
                                else
                                {
                                    MessageBox.Show(
                                        "Knižnica EPPlus pre Excel export nie je dostupná. Údaje budú exportované do CSV formátu.",
                                        "Informácia",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Fallback na CSV
                                    ExportToCsv(Path.ChangeExtension(filePath, ".csv"));
                                }
                                break;
                            case ".csv":
                                ExportToCsv(filePath);
                                break;
                            default:
                                MessageBox.Show(
                                    Properties.Strings.MessageUnsupportedFormat ??
                                    "Nepodporovaný formát súboru. Prosím, vyberte .xlsx alebo .csv.",
                                    Properties.Strings.MessageWarning ?? "Upozornenie",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                        }

                        MessageBox.Show(
                            Properties.Strings.MessageExportSuccess ?? "Údaje boli úspešne exportované!",
                            Properties.Strings.MessageInfo ?? "Informácia",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            string.Format(Properties.Strings.MessageExportError ?? "Chyba pri exporte údajov: {0}", ex.Message),
                            Properties.Strings.MessageError ?? "Chyba",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default; // Obnovenie kurzora
                    }
                }
            }
        }

        // Metóda pre export do CSV
        private void ExportToCsv(string filePath)
        {
            // Použijeme StringBuilder pre efektívne budovanie CSV obsahu
            StringBuilder csvContent = new StringBuilder();

            // Pridanie hlavičky s oddeľovačom ";"
            string[] headers = GetColumnHeaders();
            csvContent.AppendLine(string.Join(";", headers));

            // Získanie zotriedených klientov - použijeme premennú clients
            var sortedClients = clients.OrderByDescending(c => c.TotalPoints).ToList();

            // Pridanie riadkov pre každého klienta
            int id = 1;
            foreach (var client in sortedClients)
            {
                csvContent.AppendLine(string.Format("{0};{1};{2};{3};{4:F2};{5}",
                    id++,
                    $"{client.Name} ({client.PersonalId})",
                    client.ServiceCount,
                    client.TotalPoints,
                    client.TotalValue,
                    client.AveragePointsPerService));
            }

            // Pridanie súhrnného riadku
            int totalServices = clients.Sum(c => c.ServiceCount);
            int totalPoints = clients.Sum(c => c.TotalPoints);
            decimal totalValue = clients.Sum(c => c.TotalValue);

            csvContent.AppendLine(string.Format(";{0};{1};{2};{3:F2};",
                Properties.Strings.LabelTotalSum ?? "SPOLU",
                totalServices,
                totalPoints,
                totalValue));

            // Uloženie do súboru s kódovaním UTF-8
            File.WriteAllText(filePath, csvContent.ToString(), Encoding.UTF8);
        }

        // Pomocná metóda pre získanie názvov stĺpcov
        private string[] GetColumnHeaders()
        {
            return new string[]
            {
                "ID",
                Properties.Strings.ListViewColumnFile ?? "Klient",
                Properties.Strings.ListViewColumnPoints ?? "Služby",
                Properties.Strings.HistoryTotalPoints ?? "Celkové body",
                "Hodnota (€)",
                Properties.Strings.HistoryAveragePoints ?? "Priemerné body"
            };
        }
    }
}
