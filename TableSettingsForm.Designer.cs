using CountAndSortWinFormsAppNetFr4.Properties;
using System.Windows.Forms;

namespace CountAndSortWinFormsAppNetFr4
{
    partial class TableSettingsForm
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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "TableSettingsForm";


            this.ComboBoxImportSeparatorType = new System.Windows.Forms.ComboBox();
            this.ComboBoxExportSeparatorType = new System.Windows.Forms.ComboBox();
            this.LabelNameColumn = new System.Windows.Forms.Label();
            this.LabelIdColumn = new System.Windows.Forms.Label();
            this.LabelPointsColumn = new System.Windows.Forms.Label();
            this.LabelServiceCodeColumn = new System.Windows.Forms.Label();
            this.LabelDateOfServiceColumn = new System.Windows.Forms.Label();
            this.LabelDiagnosisColumn = new System.Windows.Forms.Label();
            this.LabelExportDataSeparator = new System.Windows.Forms.Label();
            this.LabelImportDataSeparator = new System.Windows.Forms.Label();
            this.NumericUpDownPointsColumn = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownIdColumn = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownNameColumn = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownDayColumn = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownServiceCodeColumn = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownDiagnosisColumn = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPointsColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownIdColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownNameColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDayColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownServiceCodeColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDiagnosisColumn)).BeginInit();
            this.SuspendLayout();

            // 
            // ComboBoxImportSeparatorType
            // 
            this.ComboBoxImportSeparatorType.AutoCompleteCustomSource.AddRange(new string[] {
            "\"|\"",
            "\";\"",
            "\",\"",
            "\".\"",
            "\" \""});
            this.ComboBoxImportSeparatorType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComboBoxImportSeparatorType.Font = new System.Drawing.Font("Roboto", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ComboBoxImportSeparatorType.FormattingEnabled = true;
            this.ComboBoxImportSeparatorType.Location = new System.Drawing.Point(1289, 91);
            this.ComboBoxImportSeparatorType.Name = "ComboBoxImportSeparatorType";
            this.ComboBoxImportSeparatorType.Size = new System.Drawing.Size(54, 31);
            this.ComboBoxImportSeparatorType.TabIndex = 19;
            this.ComboBoxImportSeparatorType.SelectedIndexChanged += new System.EventHandler(this.ComboBoxImportSeparatorType_SelectedIndexChanged);
            // 
            // ComboBoxExportSeparatorType
            // 
            this.ComboBoxExportSeparatorType.AutoCompleteCustomSource.AddRange(new string[] {
            "\"|\"",
            "\";\"",
            "\",\"",
            "\".\"",
            "\" \""});
            this.ComboBoxExportSeparatorType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComboBoxExportSeparatorType.Font = new System.Drawing.Font("Roboto", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ComboBoxExportSeparatorType.FormattingEnabled = true;
            this.ComboBoxExportSeparatorType.Items.AddRange(new object[] {
            "|",
            ";",
            ",",
            ".",
            ""});
            this.ComboBoxExportSeparatorType.Location = new System.Drawing.Point(1289, 54);
            this.ComboBoxExportSeparatorType.Name = "ComboBoxExportSeparatorType";
            this.ComboBoxExportSeparatorType.Size = new System.Drawing.Size(54, 31);
            this.ComboBoxExportSeparatorType.TabIndex = 28;
            // 
            // LabelNameColumn
            // 
            this.LabelNameColumn.AutoSize = true;
            this.LabelNameColumn.Location = new System.Drawing.Point(562, 65);
            this.LabelNameColumn.Name = "LabelNameColumn";
            this.LabelNameColumn.Size = new System.Drawing.Size(195, 22);
            this.LabelNameColumn.TabIndex = 26;
            this.LabelNameColumn.Text = "Stĺpec s menom (číslo):";
            this.LabelNameColumn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelIdColumn
            // 
            this.LabelIdColumn.AutoSize = true;
            this.LabelIdColumn.Location = new System.Drawing.Point(562, 103);
            this.LabelIdColumn.Name = "LabelIdColumn";
            this.LabelIdColumn.Size = new System.Drawing.Size(206, 22);
            this.LabelIdColumn.TabIndex = 27;
            this.LabelIdColumn.Text = "Stĺpec s Id čislom (číslo):";
            this.LabelIdColumn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelPointsColumn
            // 
            this.LabelPointsColumn.AutoSize = true;
            this.LabelPointsColumn.Location = new System.Drawing.Point(562, 141);
            this.LabelPointsColumn.Name = "LabelPointsColumn";
            this.LabelPointsColumn.Size = new System.Drawing.Size(185, 22);
            this.LabelPointsColumn.TabIndex = 22;
            this.LabelPointsColumn.Text = "Stĺpec s bodmi (číslo):";
            this.LabelPointsColumn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelServiceCodeColumn
            // 
            this.LabelServiceCodeColumn.AutoSize = true;
            this.LabelServiceCodeColumn.Location = new System.Drawing.Point(562, 179);
            this.LabelServiceCodeColumn.Name = "LabelServiceCodeColumn";
            this.LabelServiceCodeColumn.Size = new System.Drawing.Size(199, 22);
            this.LabelServiceCodeColumn.TabIndex = 32;
            this.LabelServiceCodeColumn.Text = "Stĺpec s kódom výkonu:";
            this.LabelServiceCodeColumn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelDateOfServiceColumn
            // 
            this.LabelDateOfServiceColumn.AutoSize = true;
            this.LabelDateOfServiceColumn.Location = new System.Drawing.Point(562, 217);
            this.LabelDateOfServiceColumn.Name = "LabelDateOfServiceColumn";
            this.LabelDateOfServiceColumn.Size = new System.Drawing.Size(219, 22);
            this.LabelDateOfServiceColumn.TabIndex = 33;
            this.LabelDateOfServiceColumn.Text = "Stĺpec s dátumom výkonu:";
            this.LabelDateOfServiceColumn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelDiagnosisColumn
            // 
            this.LabelDiagnosisColumn.AutoSize = true;
            this.LabelDiagnosisColumn.Location = new System.Drawing.Point(562, 250);
            this.LabelDiagnosisColumn.Name = "LabelDiagnosisColumn";
            this.LabelDiagnosisColumn.Size = new System.Drawing.Size(145, 22);
            this.LabelDiagnosisColumn.TabIndex = 32;
            this.LabelDiagnosisColumn.Text = "Stĺpec Diagnóza:";
            this.LabelDiagnosisColumn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelExportDataSeparator
            // 
            this.LabelExportDataSeparator.AutoSize = true;
            this.LabelExportDataSeparator.Location = new System.Drawing.Point(994, 59);
            this.LabelExportDataSeparator.Name = "LabelExportDataSeparator";
            this.LabelExportDataSeparator.Size = new System.Drawing.Size(243, 22);
            this.LabelExportDataSeparator.TabIndex = 29;
            this.LabelExportDataSeparator.Text = "Oddeľovač na export súboru:";
            // 
            // LabelImportDataSeparator
            // 
            this.LabelImportDataSeparator.AutoSize = true;
            this.LabelImportDataSeparator.Location = new System.Drawing.Point(994, 95);
            this.LabelImportDataSeparator.Name = "LabelImportDataSeparator";
            this.LabelImportDataSeparator.Size = new System.Drawing.Size(289, 22);
            this.LabelImportDataSeparator.TabIndex = 18;
            this.LabelImportDataSeparator.Text = "Oddeľovač stĺpcov tabuľky je znak:";
            // 
            // NumericUpDownPointsColumn
            // 
            this.NumericUpDownPointsColumn.AutoSize = true;
            this.NumericUpDownPointsColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.NumericUpDownPointsColumn.Location = new System.Drawing.Point(783, 139);
            this.NumericUpDownPointsColumn.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NumericUpDownPointsColumn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDownPointsColumn.Name = "NumericUpDownPointsColumn";
            this.NumericUpDownPointsColumn.Size = new System.Drawing.Size(68, 27);
            this.NumericUpDownPointsColumn.TabIndex = 23;
            this.NumericUpDownPointsColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDownPointsColumn.Value = new decimal(new int[] {
            11,
            0,
            0,
            0});
            // 
            // NumericUpDownIdColumn
            // 
            this.NumericUpDownIdColumn.AutoSize = true;
            this.NumericUpDownIdColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.NumericUpDownIdColumn.Location = new System.Drawing.Point(783, 101);
            this.NumericUpDownIdColumn.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NumericUpDownIdColumn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDownIdColumn.Name = "NumericUpDownIdColumn";
            this.NumericUpDownIdColumn.Size = new System.Drawing.Size(68, 27);
            this.NumericUpDownIdColumn.TabIndex = 24;
            this.NumericUpDownIdColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDownIdColumn.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // NumericUpDownNameColumn
            // 
            this.NumericUpDownNameColumn.AutoSize = true;
            this.NumericUpDownNameColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.NumericUpDownNameColumn.Location = new System.Drawing.Point(783, 63);
            this.NumericUpDownNameColumn.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NumericUpDownNameColumn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDownNameColumn.Name = "NumericUpDownNameColumn";
            this.NumericUpDownNameColumn.Size = new System.Drawing.Size(68, 27);
            this.NumericUpDownNameColumn.TabIndex = 25;
            this.NumericUpDownNameColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDownNameColumn.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // NumericUpDownDayColumn
            // 
            this.NumericUpDownDayColumn.AutoSize = true;
            this.NumericUpDownDayColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.NumericUpDownDayColumn.Location = new System.Drawing.Point(783, 215);
            this.NumericUpDownDayColumn.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NumericUpDownDayColumn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDownDayColumn.Name = "NumericUpDownDayColumn";
            this.NumericUpDownDayColumn.Size = new System.Drawing.Size(68, 27);
            this.NumericUpDownDayColumn.TabIndex = 31;
            this.NumericUpDownDayColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDownDayColumn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // NumericUpDownServiceCodeColumn
            // 
            this.NumericUpDownServiceCodeColumn.AutoSize = true;
            this.NumericUpDownServiceCodeColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.NumericUpDownServiceCodeColumn.Location = new System.Drawing.Point(783, 177);
            this.NumericUpDownServiceCodeColumn.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NumericUpDownServiceCodeColumn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDownServiceCodeColumn.Name = "NumericUpDownServiceCodeColumn";
            this.NumericUpDownServiceCodeColumn.Size = new System.Drawing.Size(68, 27);
            this.NumericUpDownServiceCodeColumn.TabIndex = 31;
            this.NumericUpDownServiceCodeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDownServiceCodeColumn.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // NumericUpDownDiagnosisColumn
            // 
            this.NumericUpDownDiagnosisColumn.AutoSize = true;
            this.NumericUpDownDiagnosisColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.NumericUpDownDiagnosisColumn.Location = new System.Drawing.Point(783, 250);
            this.NumericUpDownDiagnosisColumn.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NumericUpDownDiagnosisColumn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDownDiagnosisColumn.Name = "NumericUpDownDiagnosisColumn";
            this.NumericUpDownDiagnosisColumn.Size = new System.Drawing.Size(68, 27);
            this.NumericUpDownDiagnosisColumn.TabIndex = 31;
            this.NumericUpDownDiagnosisColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDownDiagnosisColumn.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            //
            // TableSettingsForm
            //
            this.Controls.Add(this.ComboBoxExportSeparatorType);
            this.Controls.Add(this.ComboBoxImportSeparatorType);
            this.Controls.Add(this.LabelExportDataSeparator);
            this.Controls.Add(this.LabelImportDataSeparator);
            this.Controls.Add(this.LabelNameColumn);
            this.Controls.Add(this.LabelIdColumn);
            this.Controls.Add(this.LabelPointsColumn);
            this.Controls.Add(this.LabelServiceCodeColumn);
            this.Controls.Add(this.LabelDateOfServiceColumn);
            this.Controls.Add(this.LabelDiagnosisColumn);
            this.Controls.Add(this.NumericUpDownNameColumn);
            this.Controls.Add(this.NumericUpDownIdColumn);
            this.Controls.Add(this.NumericUpDownPointsColumn);
            this.Controls.Add(this.NumericUpDownServiceCodeColumn);
            this.Controls.Add(this.NumericUpDownDayColumn);
            this.Controls.Add(this.NumericUpDownDiagnosisColumn);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "SelectFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPointsColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownIdColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownNameColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDayColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownServiceCodeColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDiagnosisColumn)).EndInit();
        }


        #endregion
        private ComboBox ComboBoxImportSeparatorType;
        private ComboBox ComboBoxExportSeparatorType;
        private Label LabelExportDataSeparator;
        private Label LabelImportDataSeparator;
        private Label LabelNameColumn;
        private Label LabelIdColumn;
        private Label LabelPointsColumn;
        private Label LabelServiceCodeColumn;
        private Label LabelDateOfServiceColumn;
        private Label LabelDiagnosisColumn;
        private NumericUpDown NumericUpDownNameColumn;
        private NumericUpDown NumericUpDownIdColumn;
        private NumericUpDown NumericUpDownPointsColumn;
        private NumericUpDown NumericUpDownServiceCodeColumn;
        private NumericUpDown NumericUpDownDayColumn;
        private NumericUpDown NumericUpDownDiagnosisColumn;
    }
}