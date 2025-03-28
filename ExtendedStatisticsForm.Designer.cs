﻿namespace CountAndSortWinFormsAppNetFr4
{
    partial class ExtendedStatisticsForm
    {
        private System.ComponentModel.IContainer components = null;

        // Deklarácia komponentov
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage summaryTab;
        private System.Windows.Forms.TabPage chartsTab;
        private System.Windows.Forms.TabPage trendTab;
        private System.Windows.Forms.TabPage comparisonTab;
        private System.Windows.Forms.DataVisualization.Charting.Chart mainChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart trendChart;
        private System.Windows.Forms.Button ButtonExport;
        private System.Windows.Forms.TableLayoutPanel summaryPanel;
        private System.Windows.Forms.TabPage doctorsTab;
        private System.Windows.Forms.TabPage facilitiesTab;
        private System.Windows.Forms.TabPage diagnosisTab;
        private System.Windows.Forms.DataVisualization.Charting.Chart doctorsChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart facilitiesChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart diagnosisChart;
        private System.Windows.Forms.DataGridView doctorsGrid;
        private System.Windows.Forms.DataGridView facilitiesGrid;
        private System.Windows.Forms.DataGridView diagnosisGrid;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtendedStatisticsForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.summaryTab = new System.Windows.Forms.TabPage();
            this.summaryPanel = new System.Windows.Forms.TableLayoutPanel();
            this.chartsTab = new System.Windows.Forms.TabPage();
            this.mainChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.trendTab = new System.Windows.Forms.TabPage();
            this.trendChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comparisonTab = new System.Windows.Forms.TabPage();
            this.doctorsTab = new System.Windows.Forms.TabPage();
            this.doctorsGrid = new System.Windows.Forms.DataGridView();
            this.doctorsChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.facilitiesTab = new System.Windows.Forms.TabPage();
            this.facilitiesGrid = new System.Windows.Forms.DataGridView();
            this.facilitiesChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.diagnosisTab = new System.Windows.Forms.TabPage();
            this.diagnosisGrid = new System.Windows.Forms.DataGridView();
            this.diagnosisChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ButtonExport = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.summaryTab.SuspendLayout();
            this.chartsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainChart)).BeginInit();
            this.trendTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trendChart)).BeginInit();
            this.doctorsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.doctorsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.doctorsChart)).BeginInit();
            this.facilitiesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.facilitiesGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.facilitiesChart)).BeginInit();
            this.diagnosisTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.diagnosisGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagnosisChart)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.doctorsTab);
            this.tabControl.Controls.Add(this.facilitiesTab);
            this.tabControl.Controls.Add(this.diagnosisTab);
            this.tabControl.Controls.Add(this.summaryTab);
            this.tabControl.Controls.Add(this.chartsTab);
            this.tabControl.Controls.Add(this.trendTab);
            this.tabControl.Controls.Add(this.comparisonTab);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1524, 740);
            this.tabControl.TabIndex = 0;
            // 
            // summaryTab
            // 
            this.summaryTab.Controls.Add(this.summaryPanel);
            this.summaryTab.Location = new System.Drawing.Point(4, 25);
            this.summaryTab.Name = "summaryTab";
            this.summaryTab.Padding = new System.Windows.Forms.Padding(3);
            this.summaryTab.Size = new System.Drawing.Size(1516, 711);
            this.summaryTab.TabIndex = 0;
            this.summaryTab.Text = "Súhrn";
            this.summaryTab.UseVisualStyleBackColor = true;
            // 
            // summaryPanel
            // 
            this.summaryPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.summaryPanel.ColumnCount = 2;
            this.summaryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.summaryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.summaryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryPanel.Location = new System.Drawing.Point(3, 3);
            this.summaryPanel.Name = "summaryPanel";
            this.summaryPanel.RowCount = 6;
            this.summaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.summaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.summaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.summaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.summaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.summaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.summaryPanel.Size = new System.Drawing.Size(1510, 705);
            this.summaryPanel.TabIndex = 0;
            // 
            // chartsTab
            // 
            this.chartsTab.Controls.Add(this.mainChart);
            this.chartsTab.Location = new System.Drawing.Point(4, 25);
            this.chartsTab.Name = "chartsTab";
            this.chartsTab.Padding = new System.Windows.Forms.Padding(3);
            this.chartsTab.Size = new System.Drawing.Size(1516, 711);
            this.chartsTab.TabIndex = 1;
            this.chartsTab.Text = "Grafy";
            this.chartsTab.UseVisualStyleBackColor = true;
            // 
            // mainChart
            // 
            this.mainChart.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mainChart.ChartAreas.Add(chartArea1);
            this.mainChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainChart.Legends.Add(legend1);
            this.mainChart.Location = new System.Drawing.Point(3, 3);
            this.mainChart.Name = "mainChart";
            this.mainChart.Size = new System.Drawing.Size(1510, 705);
            this.mainChart.TabIndex = 0;
            // 
            // trendTab
            // 
            this.trendTab.Controls.Add(this.trendChart);
            this.trendTab.Location = new System.Drawing.Point(4, 25);
            this.trendTab.Name = "trendTab";
            this.trendTab.Size = new System.Drawing.Size(1516, 711);
            this.trendTab.TabIndex = 2;
            this.trendTab.Text = "Trendy";
            this.trendTab.UseVisualStyleBackColor = true;
            // 
            // trendChart
            // 
            this.trendChart.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea2.Name = "TrendArea";
            this.trendChart.ChartAreas.Add(chartArea2);
            this.trendChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "TrendLegend";
            this.trendChart.Legends.Add(legend2);
            this.trendChart.Location = new System.Drawing.Point(0, 0);
            this.trendChart.Name = "trendChart";
            this.trendChart.Size = new System.Drawing.Size(1516, 711);
            this.trendChart.TabIndex = 0;
            // 
            // comparisonTab
            // 
            this.comparisonTab.Location = new System.Drawing.Point(4, 25);
            this.comparisonTab.Name = "comparisonTab";
            this.comparisonTab.Size = new System.Drawing.Size(1516, 711);
            this.comparisonTab.TabIndex = 3;
            this.comparisonTab.Text = "Porovnanie";
            this.comparisonTab.UseVisualStyleBackColor = true;
            // 
            // doctorsTab
            // 
            this.doctorsTab.Controls.Add(this.doctorsGrid);
            this.doctorsTab.Controls.Add(this.doctorsChart);
            this.doctorsTab.Location = new System.Drawing.Point(4, 25);
            this.doctorsTab.Name = "doctorsTab";
            this.doctorsTab.Size = new System.Drawing.Size(1516, 711);
            this.doctorsTab.TabIndex = 4;
            this.doctorsTab.Text = "Lekári";
            this.doctorsTab.UseVisualStyleBackColor = true;
            // 
            // doctorsGrid
            // 
            this.doctorsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doctorsGrid.Location = new System.Drawing.Point(0, 0);
            this.doctorsGrid.Name = "doctorsGrid";
            this.doctorsGrid.Size = new System.Drawing.Size(1516, 711);
            this.doctorsGrid.TabIndex = 0;
            // 
            // doctorsChart
            // 
            this.doctorsChart.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.Name = "MainArea";
            this.doctorsChart.ChartAreas.Add(chartArea1);
            this.doctorsChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "MainLegend";
            this.doctorsChart.Legends.Add(legend1);
            this.doctorsChart.Location = new System.Drawing.Point(0, 0);
            this.doctorsChart.Name = "doctorsChart";
            this.doctorsChart.Size = new System.Drawing.Size(1516, 711);
            this.doctorsChart.TabIndex = 0;
            // 
            // facilitiesTab
            // 
            this.facilitiesTab.Controls.Add(this.facilitiesGrid);
            this.facilitiesTab.Controls.Add(this.facilitiesChart);
            this.facilitiesTab.Location = new System.Drawing.Point(4, 25);
            this.facilitiesTab.Name = "facilitiesTab";
            this.facilitiesTab.Size = new System.Drawing.Size(1516, 711);
            this.facilitiesTab.TabIndex = 5;
            this.facilitiesTab.Text = "Zariadenia";
            this.facilitiesTab.UseVisualStyleBackColor = true;
            // 
            // facilitiesGrid
            // 
            this.facilitiesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.facilitiesGrid.Location = new System.Drawing.Point(0, 0);
            this.facilitiesGrid.Name = "facilitiesGrid";
            this.facilitiesGrid.Size = new System.Drawing.Size(1516, 711);
            this.facilitiesGrid.TabIndex = 0;
            // 
            // facilitiesChart
            // 
            this.facilitiesChart.BackColor = System.Drawing.Color.WhiteSmoke;
            this.facilitiesChart.ChartAreas.Add(chartArea1);
            this.facilitiesChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.facilitiesChart.Legends.Add(legend1);
            this.facilitiesChart.Location = new System.Drawing.Point(0, 0);
            this.facilitiesChart.Name = "facilitiesChart";
            this.facilitiesChart.Size = new System.Drawing.Size(1516, 711);
            this.facilitiesChart.TabIndex = 0;
            // 
            // diagnosisTab
            // 
            this.diagnosisTab.Controls.Add(this.diagnosisGrid);
            this.diagnosisTab.Controls.Add(this.diagnosisChart);
            this.diagnosisTab.Location = new System.Drawing.Point(4, 25);
            this.diagnosisTab.Name = "diagnosisTab";
            this.diagnosisTab.Size = new System.Drawing.Size(1516, 711);
            this.diagnosisTab.TabIndex = 6;
            this.diagnosisTab.Text = "Diagnózy";
            this.diagnosisTab.UseVisualStyleBackColor = true;
            // 
            // diagnosisGrid
            // 
            this.diagnosisGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnosisGrid.Location = new System.Drawing.Point(0, 0);
            this.diagnosisGrid.Name = "diagnosisGrid";
            this.diagnosisGrid.Size = new System.Drawing.Size(1516, 711);
            this.diagnosisGrid.TabIndex = 0;
            // 
            // diagnosisChart
            // 
            this.diagnosisChart.BackColor = System.Drawing.Color.WhiteSmoke;
            this.diagnosisChart.ChartAreas.Add(chartArea1);
            this.diagnosisChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagnosisChart.Legends.Add(legend1);
            this.diagnosisChart.Location = new System.Drawing.Point(0, 0);
            this.diagnosisChart.Name = "diagnosisChart";
            this.diagnosisChart.Size = new System.Drawing.Size(1516, 711);
            this.diagnosisChart.TabIndex = 0;
            // 
            // ButtonExport
            // 
            this.ButtonExport.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ButtonExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.ButtonExport.Location = new System.Drawing.Point(0, 740);
            this.ButtonExport.Name = "ButtonExport";
            this.ButtonExport.Size = new System.Drawing.Size(1524, 40);
            this.ButtonExport.TabIndex = 1;
            this.ButtonExport.Text = "Exportovať štatistiku";
            this.ButtonExport.UseVisualStyleBackColor = true;
            this.ButtonExport.Click += new System.EventHandler(this.ButtonExport_Click);
            // 
            // ExtendedStatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1524, 780);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.ButtonExport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExtendedStatisticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Extended Statistics";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabControl.ResumeLayout(false);
            this.summaryTab.ResumeLayout(false);
            this.chartsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainChart)).EndInit();
            this.trendTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trendChart)).EndInit();
            this.doctorsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.doctorsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.doctorsChart)).EndInit();
            this.facilitiesTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.facilitiesGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.facilitiesChart)).EndInit();
            this.diagnosisTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.diagnosisGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagnosisChart)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
