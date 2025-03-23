namespace CountAndSortWinFormsAppNetFr4
{
    partial class ClientAnalysisForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientAnalysisForm));
            this.ViewPanel = new System.Windows.Forms.Panel();
            this.ButtonExport = new System.Windows.Forms.Button();
            this.ButtonService = new System.Windows.Forms.Button();
            this.ValueButton = new System.Windows.Forms.Button();
            this.PointsButton = new System.Windows.Forms.Button();
            this.ChartPanel = new System.Windows.Forms.Panel();
            this.PieChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.BarChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.DataGridView = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Client = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPoints = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AveragePointsPerService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ViewPanel.SuspendLayout();
            this.ChartPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PieChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ViewPanel
            // 
            this.ViewPanel.BackColor = System.Drawing.Color.LightGray;
            this.ViewPanel.Controls.Add(this.ButtonExport);
            this.ViewPanel.Controls.Add(this.ButtonService);
            this.ViewPanel.Controls.Add(this.ValueButton);
            this.ViewPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ViewPanel.Location = new System.Drawing.Point(0, 0);
            this.ViewPanel.Name = "ViewPanel";
            this.ViewPanel.Size = new System.Drawing.Size(1484, 50);
            this.ViewPanel.TabIndex = 0;
            // 
            // ButtonExport
            // 
            this.ButtonExport.Location = new System.Drawing.Point(894, 10);
            this.ButtonExport.Name = "ButtonExport";
            this.ButtonExport.Size = new System.Drawing.Size(120, 30);
            this.ButtonExport.TabIndex = 2;
            this.ButtonExport.Text = global::CountAndSortWinFormsAppNetFr4.Properties.Strings.ButtonExport;
            this.ButtonExport.UseVisualStyleBackColor = true;
            // 
            // ButtonService
            // 
            this.ButtonService.Location = new System.Drawing.Point(270, 10);
            this.ButtonService.Name = "ButtonService";
            this.ButtonService.Size = new System.Drawing.Size(120, 30);
            this.ButtonService.TabIndex = 1;
            this.ButtonService.Text = global::CountAndSortWinFormsAppNetFr4.Properties.Strings.ViewByServices;
            this.ButtonService.UseVisualStyleBackColor = true;
            // 
            // ValueButton
            // 
            this.ValueButton.Location = new System.Drawing.Point(140, 10);
            this.ValueButton.Name = "ValueButton";
            this.ValueButton.Size = new System.Drawing.Size(120, 30);
            this.ValueButton.TabIndex = 0;
            this.ValueButton.Text = global::CountAndSortWinFormsAppNetFr4.Properties.Strings.ViewByFinances;
            this.ValueButton.UseVisualStyleBackColor = true;
            // 
            // PointsButton
            // 
            this.PointsButton.Location = new System.Drawing.Point(10, 10);
            this.PointsButton.Name = "PointsButton";
            this.PointsButton.Size = new System.Drawing.Size(120, 30);
            this.PointsButton.TabIndex = 1;
            this.PointsButton.Text = global::CountAndSortWinFormsAppNetFr4.Properties.Strings.ViewByPoints;
            this.PointsButton.UseVisualStyleBackColor = true;
            // 
            // ChartPanel
            // 
            this.ChartPanel.BackColor = System.Drawing.Color.White;
            this.ChartPanel.Controls.Add(this.PieChart);
            this.ChartPanel.Controls.Add(this.BarChart);
            this.ChartPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChartPanel.Location = new System.Drawing.Point(0, 50);
            this.ChartPanel.Name = "ChartPanel";
            this.ChartPanel.Size = new System.Drawing.Size(1484, 300);
            this.ChartPanel.TabIndex = 2;
            // 
            // PieChart
            // 
            chartArea1.Name = "ChartArea1";
            this.PieChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.PieChart.Legends.Add(legend1);
            this.PieChart.Location = new System.Drawing.Point(630, 10);
            this.PieChart.Name = "PieChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.PieChart.Series.Add(series1);
            this.PieChart.Size = new System.Drawing.Size(500, 280);
            this.PieChart.TabIndex = 1;
            this.PieChart.Text = "chart1";
            // 
            // BarChart
            // 
            chartArea2.Name = "ChartArea1";
            this.BarChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.BarChart.Legends.Add(legend2);
            this.BarChart.Location = new System.Drawing.Point(10, 10);
            this.BarChart.Name = "BarChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.BarChart.Series.Add(series2);
            this.BarChart.Size = new System.Drawing.Size(500, 280);
            this.BarChart.TabIndex = 0;
            this.BarChart.Text = "chart1";
            // 
            // DataGridView
            // 
            this.DataGridView.AllowUserToAddRows = false;
            this.DataGridView.AllowUserToDeleteRows = false;
            this.DataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Client,
            this.ServiceCount,
            this.TotalPoints,
            this.TotalValue,
            this.AveragePointsPerService});
            this.DataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView.Location = new System.Drawing.Point(0, 350);
            this.DataGridView.Name = "DataGridView";
            this.DataGridView.ReadOnly = true;
            this.DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView.Size = new System.Drawing.Size(1484, 411);
            this.DataGridView.TabIndex = 3;
            // 
            // Id
            // 
            this.Id.HeaderText = "ID";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // Client
            // 
            this.Client.HeaderText = "Client";
            this.Client.Name = "Client";
            this.Client.ReadOnly = true;
            // 
            // ServiceCount
            // 
            this.ServiceCount.HeaderText = "Services";
            this.ServiceCount.Name = "ServiceCount";
            this.ServiceCount.ReadOnly = true;
            // 
            // TotalPoints
            // 
            this.TotalPoints.HeaderText = "Total Points";
            this.TotalPoints.Name = "TotalPoints";
            this.TotalPoints.ReadOnly = true;
            // 
            // TotalValue
            // 
            this.TotalValue.HeaderText = "Value (€)";
            this.TotalValue.Name = "TotalValue";
            this.TotalValue.ReadOnly = true;
            // 
            // AveragePointsPerService
            // 
            this.AveragePointsPerService.HeaderText = "Avg. Points/Service";
            this.AveragePointsPerService.Name = "AveragePointsPerService";
            this.AveragePointsPerService.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 240;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Client";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 240;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Services";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 241;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = global::CountAndSortWinFormsAppNetFr4.Properties.Strings.TotalPoints;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 240;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Value (€)";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 240;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Avg. Points/Service";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 240;
            // 
            // ClientAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 761);
            this.Controls.Add(this.DataGridView);
            this.Controls.Add(this.ChartPanel);
            this.Controls.Add(this.PointsButton);
            this.Controls.Add(this.ViewPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ClientAnalysisForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ViewPanel.ResumeLayout(false);
            this.ChartPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PieChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ViewPanel;
        private System.Windows.Forms.Button PointsButton;
        private System.Windows.Forms.Button ButtonService;
        private System.Windows.Forms.Button ValueButton;
        private System.Windows.Forms.Button ButtonExport;
        private System.Windows.Forms.Panel ChartPanel;
        private System.Windows.Forms.DataVisualization.Charting.Chart BarChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart PieChart;
        private System.Windows.Forms.DataGridView DataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Client;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn AveragePointsPerService;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    }
}