using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CountAndSortWinFormsAppNetFr4
{
    public partial class ExtendedStatisticsForm : Form
    {
        private StatisticsModule statisticsModule;
        private List<ProcessedFileInfo> files;
        private Dictionary<string, object> statistics;
        private TabControl tabControl;
        private Chart mainChart;
        private Chart trendChart;

        public ExtendedStatisticsForm(List<ProcessedFileInfo> processedFiles)
        {
            InitializeComponent();
            this.files = processedFiles;
            this.statisticsModule = new StatisticsModule(null);
            this.Text = "Rozšírená štatistika";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeComponents();
            LoadStatistics();
        }

        private void InitializeComponents()
        {
            // Vytvorenie záložiek
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font(Font.FontFamily, 10)
            };

            TabPage summaryTab = new TabPage("Súhrn");
            TabPage chartsTab = new TabPage("Grafy");
            TabPage trendTab = new TabPage("Trendy");
            TabPage comparisonTab = new TabPage("Porovnanie");

            tabControl.TabPages.Add(summaryTab);
            tabControl.TabPages.Add(chartsTab);
            tabControl.TabPages.Add(trendTab);
            tabControl.TabPages.Add(comparisonTab);

            // Vytvorenie grafov
            mainChart = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };

            // Nastavenie grafu
            ChartArea chartArea = new ChartArea("MainArea");
            mainChart.ChartAreas.Add(chartArea);
            mainChart.Legends.Add(new Legend("MainLegend"));

            // Pridanie grafu do záložky
            chartsTab.Controls.Add(mainChart);

            // Vytvorenie grafu trendov
            trendChart = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };

            // Nastavenie grafu trendov
            ChartArea trendArea = new ChartArea("TrendArea");
            trendChart.ChartAreas.Add(trendArea);
            trendChart.Legends.Add(new Legend("TrendLegend"));

            // Pridanie grafu trendov do záložky
            trendTab.Controls.Add(trendChart);

            // Tlačidlo exportu
            Button exportButton = new Button
            {
                Text = "Exportovať štatistiku",
                Dock = DockStyle.Bottom,
                Height = 40,
                Font = new Font(Font.FontFamily, 10, FontStyle.Bold)
            };

            exportButton.Click += ExportButton_Click;

            this.Controls.Add(tabControl);
            this.Controls.Add(exportButton);
        }

        private void LoadStatistics()
        {
            if (files == null || files.Count == 0)
            {
                MessageBox.Show("Nie sú k dispozícii žiadne dáta pre štatistiku.",
                    "Upozornenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Generovanie štatistík
            statistics = statisticsModule.GenerateExtendedStatistics(files);

            // Naplnenie súhrnnej záložky
            TabPage summaryTab = tabControl.TabPages[0];

            TableLayoutPanel summaryPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                RowCount = 6,
                ColumnCount = 2
            };

            // Definícia šírky stĺpcov
            summaryPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            summaryPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));

            // Pridanie súhrnných údajov
            AddLabelPair(summaryPanel, "Celkový počet súborov:", statistics["TotalFiles"].ToString(), 0);
            AddLabelPair(summaryPanel, "Celkový počet bodov:", statistics["TotalPoints"].ToString("N0"), 1);
            AddLabelPair(summaryPanel, "Priemerný počet bodov:", ((double)statistics["AveragePoints"]).ToString("N1"), 2);
            AddLabelPair(summaryPanel, "Maximálny počet bodov:", statistics["MaxPoints"].ToString("N0"), 3);
            AddLabelPair(summaryPanel, "Minimálny počet bodov:", statistics["MinPoints"].ToString("N0"), 4);

            summaryTab.Controls.Add(summaryPanel);

            // Naplnenie grafov
            FillMainChart();
            FillTrendChart();

            // Naplnenie porovnávacej záložky
            FillComparisonTab();
        }

        private void AddLabelPair(TableLayoutPanel panel, string title, string value, int row)
        {
            Label titleLabel = new Label
            {
                Text = title,
                Dock = DockStyle.Fill,
                Font = new Font(Font.FontFamily, 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label valueLabel = new Label
            {
                Text = value,
                Dock = DockStyle.Fill,
                Font = new Font(Font.FontFamily, 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            panel.Controls.Add(titleLabel, 0, row);
            panel.Controls.Add(valueLabel, 1, row);
        }

        private void FillMainChart()
        {
            mainChart.Series.Clear();

            // Vytvorenie série pre stĺpcový graf
            Series columnSeries = new Series("Body podľa súborov")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                LabelFormat = "N0"
            };

            // Zoradenie súborov podľa počtu bodov (zostupne)
            var sortedFiles = files.OrderByDescending(f => f.Points).Take(10); // Zobrazíme top 10

            foreach (var file in sortedFiles)
            {
                columnSeries.Points.AddXY(file.FileName, file.Points);
            }

            mainChart.Series.Add(columnSeries);

            // Nastavenie X-ovej osi
            mainChart.ChartAreas[0].AxisX.Interval = 1;
            mainChart.ChartAreas[0].AxisX.LabelStyle.Angle = -45;

            // Nadpis grafu
            mainChart.Titles.Add(new Title("Top 10 súborov podľa počtu bodov", Docking.Top,
                new Font(Font.FontFamily, 12, FontStyle.Bold), Color.Black));
        }

        private void FillTrendChart()
        {
            // Analýza trendov za posledných 12 mesiacov
            Dictionary<string, object> trends = statisticsModule.AnalyzeTrends(files);

            if (!trends.ContainsKey("MonthlyTrendData"))
                return;

            trendChart.Series.Clear();

            // Vytvorenie série pre trendový graf
            Series trendSeries = new Series("Body podľa mesiacov")
            {
                ChartType = SeriesChartType.Line,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                BorderWidth = 2
            };

            // Pridanie bodov do série
            var monthlyData = trends["MonthlyTrendData"] as List<dynamic>;

            if (monthlyData != null)
            {
                foreach (var item in monthlyData)
                {
                    trendSeries.Points.AddXY(item.Period, item.Points);
                }
            }

            trendChart.Series.Add(trendSeries);

            // Ak máme predikciu, pridáme ju
            if (trends.ContainsKey("Predictions"))
            {
                Series predictionSeries = new Series("Predikované body")
                {
                    ChartType = SeriesChartType.Line,
                    MarkerStyle = MarkerStyle.Diamond,
                    MarkerSize = 8,
                    BorderWidth = 2,
                    BorderDashStyle = ChartDashStyle.Dash,
                    Color = Color.Red
                };

                var predictions = trends["Predictions"] as List<Dictionary<string, object>>;

                // Pridanie posledného skutočného bodu pre kontinuitu grafu
                if (monthlyData != null && monthlyData.Count > 0)
                {
                    var lastActual = monthlyData.Last();
                    predictionSeries.Points.AddXY(lastActual.Period, lastActual.Points);
                }

                // Pridanie predikovaných bodov
                if (predictions != null)
                {
                    foreach (var pred in predictions)
                    {
                        predictionSeries.Points.AddXY(pred["Period"], pred["Points"]);
                    }
                }

                trendChart.Series.Add(predictionSeries);
            }

            // Nadpis grafu
            trendChart.Titles.Add(new Title("Vývoj bodov v čase", Docking.Top,
                new Font(Font.FontFamily, 12, FontStyle.Bold), Color.Black));

            if (trends.ContainsKey("TrendDirection"))
            {
                trendChart.Titles.Add(new Title($"Trend: {trends["TrendDirection"]}", Docking.Bottom,
                    new Font(Font.FontFamily, 10), Color.Gray));
            }
        }

        private void FillComparisonTab()
        {
            TabPage comparisonTab = tabControl.TabPages[3];

            // Kontrola, či máme aspoň dva súbory pre porovnanie
            if (files.Count < 2)
            {
                Label infoLabel = new Label
                {
                    Text = "Pre porovnanie potrebujete aspoň dva súbory.",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font(Font.FontFamily, 12)
                };

                comparisonTab.Controls.Add(infoLabel);
                return;
            }

            // Získanie porovnávacích dát
            Dictionary<string, object> comparison = statisticsModule.CompareFiles(files);

            // Vytvorenie DataGridView pre zobrazenie porovnania
            DataGridView comparisonGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoGenerateColumns = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.LightCyan
                }
            };

            // Definícia stĺpcov
            comparisonGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FileName",
                HeaderText = "Názov súboru",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            comparisonGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProcessedTime",
                HeaderText = "Dátum spracovania",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd.MM.yyyy HH:mm:ss"
                }
            });

            comparisonGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Points",
                HeaderText = "Body",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            comparisonGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PointsDiff",
                HeaderText = "Rozdiel od priemeru",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "+0;-0;0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            comparisonGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PercentOfTotal",
                HeaderText = "% z celku",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "P2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            // Naplnenie mriežky dátami
            if (comparison.ContainsKey("Comparison"))
            {
                var comparisonData = comparison["Comparison"] as List<Dictionary<string, object>>;

                if (comparisonData != null)
                {
                    foreach (var item in comparisonData)
                    {
                        int rowIndex = comparisonGrid.Rows.Add();
                        var row = comparisonGrid.Rows[rowIndex];

                        row.Cells["FileName"].Value = item["FileName"];
                        row.Cells["ProcessedTime"].Value = item["ProcessedTime"];
                        row.Cells["Points"].Value = item["Points"];
                        row.Cells["PointsDiff"].Value = item["PointsDiff"];
                        row.Cells["PercentOfTotal"].Value = item["PercentOfTotal"];

                        // Farebné zvýraznenie rozdielov
                        double diff = Convert.ToDouble(item["PointsDiff"]);

                        if (diff > 0)
                            row.Cells["PointsDiff"].Style.ForeColor = Color.Green;
                        else if (diff < 0)
                            row.Cells["PointsDiff"].Style.ForeColor = Color.Red;
                    }
                }
            }

            comparisonTab.Controls.Add(comparisonGrid);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel súbory (*.xlsx)|*.xlsx|Všetky súbory (*.*)|*.*";
                saveDialog.FilterIndex = 1;
                saveDialog.FileName = $"StatistikaRozšírená_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        statisticsModule.ExportExtendedStatistics(files, saveDialog.FileName);
                        Cursor = Cursors.Default;

                        MessageBox.Show($"Rozšírená štatistika bola úspešne exportovaná do súboru:\n{saveDialog.FileName}",
                            "Export dokončený", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show($"Chyba pri exporte štatistiky: {ex.Message}",
                            "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
