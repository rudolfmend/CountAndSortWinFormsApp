using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CountAndSortWinFormsAppNetFr4
{
    public partial class ClientAnalysisForm : Form
    {
        private List<ClientInfo> clients;

        public ClientAnalysisForm(List<ClientInfo> clients)
        {
            this.clients = clients;
            InitializeComponent();
            InitializeUI();
            PopulateChartsAndTables();
        }

        private void InitializeUI()
        {
            // Presun PointsButton do ViewPanel
            ViewPanel.Controls.Add(PointsButton);

            // Pridanie event handlerov pre tlačidlá
            PointsButton.Click += (s, e) => { viewMode = "points"; UpdateCharts(); };
            ValueButton.Click += (s, e) => { viewMode = "value"; UpdateCharts(); };
            ButtonService.Click += (s, e) => { viewMode = "services"; UpdateCharts(); };
            ExportButton.Click += ExportButton_Click;

            // Pridanie StatusLabel do StatusStrip
            var statusLabel = new ToolStripStatusLabel();
            statusLabel.Text = $"Total clients: {clients.Count}";
            //System.Windows.Forms.StatusStrip.Items.Add(statusLabel);
        }

        private void PopulateChartsAndTables()
        {
            // Implement your logic to populate charts and tables
            UpdateClientsSummaryTable();
            UpdatePointsChart();
            UpdateServiceCountChart();
        }

        private string viewMode = "points"; // Predvolený režim zobrazenia

        private void UpdateCharts()
        {
            // Aktualizácia grafov podľa aktuálneho režimu zobrazenia
            switch (viewMode)
            {
                case "points":
                    UpdatePointsChart();
                    break;
                case "value":
                    UpdateValueChart();
                    break;
                case "services":
                    UpdateServiceCountChart();
                    break;
            }
        }

        private void UpdateClientsSummaryTable()
        {
            DataGridView.Rows.Clear();

            // Zotriedenie klientov podľa počtu bodov (zostupne)
            var sortedClients = clients.OrderByDescending(c => c.TotalPoints).ToList();

            int id = 1;
            foreach (var client in sortedClients)
            {
                DataGridView.Rows.Add(
                    id++,
                    $"{client.Name} ({client.PersonalId})",
                    client.ServiceCount,
                    client.TotalPoints,
                    $"{client.TotalValue:F2} €",
                    $"{client.AveragePointsPerService:F0}"
                );
            }

            // Pridanie súhrnného riadku
            int totalServices = clients.Sum(c => c.ServiceCount);
            int totalPoints = clients.Sum(c => c.TotalPoints);
            decimal totalValue = clients.Sum(c => c.TotalValue);

            // Špeciálny riadok pre súhrn
            int rowIndex = DataGridView.Rows.Add(
                "",
                "TOTAL",
                totalServices,
                totalPoints,
                $"{totalValue:F2} €",
                ""
            );

            // Zvýraznenie súhrnného riadku
            DataGridView.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            DataGridView.Rows[rowIndex].DefaultCellStyle.Font = new Font(DataGridView.Font, FontStyle.Bold);
        }

        private void UpdatePointsChart()
        {
            // Tu by ste implementovali vytvorenie stĺpcového grafu pre body
            BarChart.Series.Clear();

            var series = new Series("Total Points");
            series.ChartType = SeriesChartType.Column;

            // Získame top 10 klientov podľa bodov
            var topClients = clients
                .OrderByDescending(c => c.TotalPoints)
                .Take(10)
                .ToList();

            foreach (var client in topClients)
            {
                // Získanie skráteného mena pre graf
                string shortName = client.Name.Length > 15
                    ? client.Name.Substring(0, 12) + "..."
                    : client.Name;

                series.Points.AddXY(shortName, client.TotalPoints);
            }

            BarChart.Series.Add(series);
            BarChart.Titles.Clear();
            BarChart.Titles.Add("Top 10 Clients by Points");

            // Konfigurácia koláčového grafu pre podiel bodov
            UpdatePieChart(topClients, c => c.TotalPoints, "Points Distribution");
        }

        private void UpdateValueChart()
        {
            // Implementácia podobná ako UpdatePointsChart, ale pre hodnoty v eurách
            BarChart.Series.Clear();

            var series = new Series("Total Value (€)");
            series.ChartType = SeriesChartType.Column;

            // Získame top 10 klientov podľa hodnoty
            var topClients = clients
                .OrderByDescending(c => c.TotalValue)
                .Take(10)
                .ToList();

            foreach (var client in topClients)
            {
                string shortName = client.Name.Length > 15
                    ? client.Name.Substring(0, 12) + "..."
                    : client.Name;

                series.Points.AddXY(shortName, (double)client.TotalValue);
            }

            BarChart.Series.Add(series);
            BarChart.Titles.Clear();
            BarChart.Titles.Add("Top 10 Clients by Value (€)");

            // Konfigurácia koláčového grafu pre podiel hodnôt
            UpdatePieChart(topClients, c => c.TotalValue, "Value Distribution (€)");
        }

        private void UpdateServiceCountChart()
        {
            // Implementácia pre počty služieb
            BarChart.Series.Clear();

            var series = new Series("Service Count");
            series.ChartType = SeriesChartType.Column;

            // Získame top 10 klientov podľa počtu služieb
            var topClients = clients
                .OrderByDescending(c => c.ServiceCount)
                .Take(10)
                .ToList();

            foreach (var client in topClients)
            {
                string shortName = client.Name.Length > 15
                    ? client.Name.Substring(0, 12) + "..."
                    : client.Name;

                series.Points.AddXY(shortName, client.ServiceCount);
            }

            BarChart.Series.Add(series);
            BarChart.Titles.Clear();
            BarChart.Titles.Add("Top 10 Clients by Services");

            // Konfigurácia koláčového grafu pre podiel služieb
            UpdatePieChart(topClients, c => c.ServiceCount, "Services Distribution");
        }

        private void ExportToCsv(string filePath)
        {
            // Implementácia exportu do CSV
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Hlavička
                writer.WriteLine("ID;Client;Personal ID;Services;Total Points;Value (€);Average Points");

                // Zotriedenie klientov podľa počtu bodov (zostupne)
                var sortedClients = clients.OrderByDescending(c => c.TotalPoints).ToList();

                int id = 1;
                foreach (var client in sortedClients)
                {
                    writer.WriteLine($"{id};{client.Name};{client.PersonalId};{client.ServiceCount};{client.TotalPoints};{client.TotalValue:F2};{client.AveragePointsPerService:F0}");
                    id++;
                }

                // Súhrnný riadok
                int totalServices = clients.Sum(c => c.ServiceCount);
                int totalPoints = clients.Sum(c => c.TotalPoints);
                decimal totalValue = clients.Sum(c => c.TotalValue);

                writer.WriteLine($";TOTAL;;{totalServices};{totalPoints};{totalValue:F2};");
            }
        }

        private void ExportToExcel(string filePath)
        {
            // Pre úplnú implementáciu exportu do Excelu by ste potrebovali referenciu na Excel knižnicu
            // Toto je zjednodušená verzia, ktorá uloží súbor ako CSV, ktorý Excel dokáže otvoriť
            ExportToCsv(filePath);
            MessageBox.Show("Excel export requires Excel library. CSV file was created instead, which can be opened in Excel.",
                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdatePieChart<T>(List<ClientInfo> topClients, Func<ClientInfo, T> valueSelector, string title)
        {
            PieChart.Series.Clear();

            var series = new Series("Distribution");
            series.ChartType = SeriesChartType.Pie;

            foreach (var client in topClients)
            {
                string shortName = client.Name.Length > 15
                    ? client.Name.Substring(0, 12) + "..."
                    : client.Name;

                var point = series.Points.AddXY(shortName, Convert.ToDouble(valueSelector(client)));
            }

            PieChart.Series.Add(series);
            PieChart.Titles.Clear();
            PieChart.Titles.Add(title);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            // Implementácia exportu dát
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.FilterIndex = 1;
                saveDialog.RestoreDirectory = true;

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveDialog.FileName;
                    try
                    {
                        if (Path.GetExtension(filePath).ToLower() == ".csv")
                        {
                            ExportToCsv(filePath);
                        }
                        else if (Path.GetExtension(filePath).ToLower() == ".xlsx")
                        {
                            ExportToExcel(filePath);
                        }
                        MessageBox.Show("Data exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
