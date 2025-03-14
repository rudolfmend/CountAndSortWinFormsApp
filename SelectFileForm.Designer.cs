﻿using System.Windows.Forms;

namespace CountAndSortWinFormsAppNetFr4
{
    partial class SelectFileForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectFileForm));
            this.ButtonSelectAFile = new System.Windows.Forms.Button();
            this.ButtonProcessData = new System.Windows.Forms.Button();
            this.CheckBoxRenumberTheOrder = new System.Windows.Forms.CheckBox();
            this.CheckBoxSortByName = new System.Windows.Forms.CheckBox();
            this.CheckBoxRemoveDuplicatesRows = new System.Windows.Forms.CheckBox();
            this.ButtonSelectOutputFolder = new System.Windows.Forms.Button();
            this.TextBoxSelectOutputFolder = new System.Windows.Forms.TextBox();
            this.ButtonSaveHistory = new System.Windows.Forms.Button();
            this.ListViewShowPointsValues = new System.Windows.Forms.ListView();
            this.LabelFileCount = new System.Windows.Forms.Label();
            this.LabelTotalPoints = new System.Windows.Forms.Label();
            this.DataGridPreview = new System.Windows.Forms.DataGridView();
            this.CheckBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.LabelImportDataSeparator = new System.Windows.Forms.Label();
            this.ComboBoxImportSeparatorType = new System.Windows.Forms.ComboBox();
            this.LanguageComboBox = new System.Windows.Forms.ComboBox();
            this.LanguageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.LabelLanguagesChoice = new System.Windows.Forms.Label();
            this.LabelPointsColumn = new System.Windows.Forms.Label();
            this.NumericUpDownPointsColumn = new System.Windows.Forms.NumericUpDown();
            this.ColumnToCountToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.NumericUpDownIdColumn = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownNameColumn = new System.Windows.Forms.NumericUpDown();
            this.LabelNameColumn = new System.Windows.Forms.Label();
            this.LabelIdColumn = new System.Windows.Forms.Label();
            this.ComboBoxExportSeparatorType = new System.Windows.Forms.ComboBox();
            this.LabelExportDataSeparator = new System.Windows.Forms.Label();
            this.DataStructureSeparatorToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ButtonShowAnalysis = new System.Windows.Forms.Button();
            this.NumericUpDownDayColumn = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownServiceCodeColumn = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDownDiagnosisColumn = new System.Windows.Forms.NumericUpDown();
            this.LabelServiceCodeColumn = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPointsColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownIdColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownNameColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDayColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownServiceCodeColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDiagnosisColumn)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonSelectAFile
            // 
            this.ButtonSelectAFile.Location = new System.Drawing.Point(12, 31);
            this.ButtonSelectAFile.Name = "ButtonSelectAFile";
            this.ButtonSelectAFile.Size = new System.Drawing.Size(173, 52);
            this.ButtonSelectAFile.TabIndex = 1;
            this.ButtonSelectAFile.Text = "Výber súboru";
            this.ButtonSelectAFile.UseVisualStyleBackColor = true;
            this.ButtonSelectAFile.Click += new System.EventHandler(this.ButtonSelectAFile_Click);
            // 
            // ButtonProcessData
            // 
            this.ButtonProcessData.Location = new System.Drawing.Point(12, 89);
            this.ButtonProcessData.Name = "ButtonProcessData";
            this.ButtonProcessData.Size = new System.Drawing.Size(173, 52);
            this.ButtonProcessData.TabIndex = 2;
            this.ButtonProcessData.Text = "Spracovať údaje";
            this.ButtonProcessData.UseVisualStyleBackColor = true;
            this.ButtonProcessData.Click += new System.EventHandler(this.ButtonProcessData_Click);
            // 
            // CheckBoxRenumberTheOrder
            // 
            this.CheckBoxRenumberTheOrder.AutoSize = true;
            this.CheckBoxRenumberTheOrder.Checked = true;
            this.CheckBoxRenumberTheOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxRenumberTheOrder.Location = new System.Drawing.Point(235, 43);
            this.CheckBoxRenumberTheOrder.Name = "CheckBoxRenumberTheOrder";
            this.CheckBoxRenumberTheOrder.Size = new System.Drawing.Size(181, 26);
            this.CheckBoxRenumberTheOrder.TabIndex = 4;
            this.CheckBoxRenumberTheOrder.Text = "prečíslovať poradie";
            this.CheckBoxRenumberTheOrder.UseVisualStyleBackColor = true;
            // 
            // CheckBoxSortByName
            // 
            this.CheckBoxSortByName.AutoSize = true;
            this.CheckBoxSortByName.Checked = true;
            this.CheckBoxSortByName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxSortByName.Location = new System.Drawing.Point(235, 75);
            this.CheckBoxSortByName.Name = "CheckBoxSortByName";
            this.CheckBoxSortByName.Size = new System.Drawing.Size(183, 26);
            this.CheckBoxSortByName.TabIndex = 5;
            this.CheckBoxSortByName.Text = "zoradiť podľa mena";
            this.CheckBoxSortByName.UseVisualStyleBackColor = true;
            // 
            // CheckBoxRemoveDuplicatesRows
            // 
            this.CheckBoxRemoveDuplicatesRows.AutoSize = true;
            this.CheckBoxRemoveDuplicatesRows.Checked = true;
            this.CheckBoxRemoveDuplicatesRows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxRemoveDuplicatesRows.Location = new System.Drawing.Point(235, 107);
            this.CheckBoxRemoveDuplicatesRows.Name = "CheckBoxRemoveDuplicatesRows";
            this.CheckBoxRemoveDuplicatesRows.Size = new System.Drawing.Size(233, 26);
            this.CheckBoxRemoveDuplicatesRows.TabIndex = 7;
            this.CheckBoxRemoveDuplicatesRows.Text = "odstrániť duplicitné riadky";
            this.CheckBoxRemoveDuplicatesRows.UseVisualStyleBackColor = true;
            // 
            // ButtonSelectOutputFolder
            // 
            this.ButtonSelectOutputFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ButtonSelectOutputFolder.Location = new System.Drawing.Point(566, 10);
            this.ButtonSelectOutputFolder.Name = "ButtonSelectOutputFolder";
            this.ButtonSelectOutputFolder.Size = new System.Drawing.Size(173, 52);
            this.ButtonSelectOutputFolder.TabIndex = 8;
            this.ButtonSelectOutputFolder.Text = "Výber miesta uloženia spracovaného súboru";
            this.ButtonSelectOutputFolder.UseVisualStyleBackColor = true;
            this.ButtonSelectOutputFolder.Click += new System.EventHandler(this.ButtonSelectOutputFolder_Click);
            // 
            // TextBoxSelectOutputFolder
            // 
            this.TextBoxSelectOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSelectOutputFolder.Location = new System.Drawing.Point(783, 22);
            this.TextBoxSelectOutputFolder.Name = "TextBoxSelectOutputFolder";
            this.TextBoxSelectOutputFolder.Size = new System.Drawing.Size(642, 27);
            this.TextBoxSelectOutputFolder.TabIndex = 9;
            this.TextBoxSelectOutputFolder.Text = " ";
            // 
            // ButtonSaveHistory
            // 
            this.ButtonSaveHistory.Location = new System.Drawing.Point(12, 147);
            this.ButtonSaveHistory.Name = "ButtonSaveHistory";
            this.ButtonSaveHistory.Size = new System.Drawing.Size(173, 52);
            this.ButtonSaveHistory.TabIndex = 11;
            this.ButtonSaveHistory.Text = "Uložiť históriu bodov";
            this.ButtonSaveHistory.UseVisualStyleBackColor = true;
            this.ButtonSaveHistory.Click += new System.EventHandler(this.ButtonSaveHistory_Click);
            // 
            // ListViewShowPointsValues
            // 
            this.ListViewShowPointsValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListViewShowPointsValues.FullRowSelect = true;
            this.ListViewShowPointsValues.GridLines = true;
            this.ListViewShowPointsValues.HideSelection = false;
            this.ListViewShowPointsValues.Location = new System.Drawing.Point(1211, 274);
            this.ListViewShowPointsValues.Margin = new System.Windows.Forms.Padding(5);
            this.ListViewShowPointsValues.Name = "ListViewShowPointsValues";
            this.ListViewShowPointsValues.Size = new System.Drawing.Size(406, 621);
            this.ListViewShowPointsValues.TabIndex = 12;
            this.ListViewShowPointsValues.TileSize = new System.Drawing.Size(250, 44);
            this.ListViewShowPointsValues.UseCompatibleStateImageBehavior = false;
            this.ListViewShowPointsValues.View = System.Windows.Forms.View.Details;
            // 
            // LabelFileCount
            // 
            this.LabelFileCount.AutoSize = true;
            this.LabelFileCount.Location = new System.Drawing.Point(994, 133);
            this.LabelFileCount.Name = "LabelFileCount";
            this.LabelFileCount.Size = new System.Drawing.Size(131, 22);
            this.LabelFileCount.TabIndex = 14;
            this.LabelFileCount.Text = "LabelFileCount";
            // 
            // LabelTotalPoints
            // 
            this.LabelTotalPoints.AutoSize = true;
            this.LabelTotalPoints.Location = new System.Drawing.Point(994, 171);
            this.LabelTotalPoints.Name = "LabelTotalPoints";
            this.LabelTotalPoints.Size = new System.Drawing.Size(145, 22);
            this.LabelTotalPoints.TabIndex = 15;
            this.LabelTotalPoints.Text = "LabelTotalPoints";
            // 
            // DataGridPreview
            // 
            this.DataGridPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridPreview.Location = new System.Drawing.Point(12, 275);
            this.DataGridPreview.Name = "DataGridPreview";
            this.DataGridPreview.Size = new System.Drawing.Size(1191, 620);
            this.DataGridPreview.TabIndex = 16;
            // 
            // CheckBoxSelectAll
            // 
            this.CheckBoxSelectAll.AutoSize = true;
            this.CheckBoxSelectAll.Location = new System.Drawing.Point(235, 139);
            this.CheckBoxSelectAll.Name = "CheckBoxSelectAll";
            this.CheckBoxSelectAll.Size = new System.Drawing.Size(195, 26);
            this.CheckBoxSelectAll.TabIndex = 17;
            this.CheckBoxSelectAll.Text = "označiť všetky riadky";
            this.CheckBoxSelectAll.UseVisualStyleBackColor = true;
            this.CheckBoxSelectAll.CheckedChanged += new System.EventHandler(this.CheckBoxSelectAll_CheckedChanged);
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
            // LanguageComboBox
            // 
            this.LanguageComboBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.LanguageComboBox.FormattingEnabled = true;
            this.LanguageComboBox.Items.AddRange(new object[] {
            "English",
            "Slovensky",
            "Česky",
            "Deutsch",
            "Polski",
            "Magyar",
            "Українська"});
            this.LanguageComboBox.Location = new System.Drawing.Point(1613, 0);
            this.LanguageComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.LanguageComboBox.Name = "LanguageComboBox";
            this.LanguageComboBox.Size = new System.Drawing.Size(144, 28);
            this.LanguageComboBox.TabIndex = 20;
            this.LanguageComboBox.SelectedIndexChanged += new System.EventHandler(this.LanguageComboBox_SelectedIndexChanged);
            // 
            // LanguageToolTip
            // 
            this.LanguageToolTip.ShowAlways = true;
            this.LanguageToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.LanguageToolTip.ToolTipTitle = "Výber jazyka / Language selection / Sprachauswahl";
            // 
            // LabelLanguagesChoice
            // 
            this.LabelLanguagesChoice.AutoSize = true;
            this.LabelLanguagesChoice.Dock = System.Windows.Forms.DockStyle.Right;
            this.LabelLanguagesChoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.LabelLanguagesChoice.Location = new System.Drawing.Point(1567, 0);
            this.LabelLanguagesChoice.Margin = new System.Windows.Forms.Padding(0);
            this.LabelLanguagesChoice.Name = "LabelLanguagesChoice";
            this.LabelLanguagesChoice.Size = new System.Drawing.Size(46, 31);
            this.LabelLanguagesChoice.TabIndex = 21;
            this.LabelLanguagesChoice.Text = "🌐";
            // 
            // LabelPointsColumn
            // 
            this.LabelPointsColumn.AutoSize = true;
            this.LabelPointsColumn.Location = new System.Drawing.Point(562, 171);
            this.LabelPointsColumn.Name = "LabelPointsColumn";
            this.LabelPointsColumn.Size = new System.Drawing.Size(185, 22);
            this.LabelPointsColumn.TabIndex = 22;
            this.LabelPointsColumn.Text = "Stĺpec s bodmi (číslo):";
            this.LabelPointsColumn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // NumericUpDownPointsColumn
            // 
            this.NumericUpDownPointsColumn.AutoSize = true;
            this.NumericUpDownPointsColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.NumericUpDownPointsColumn.Location = new System.Drawing.Point(783, 169);
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
            // ColumnToCountToolTip
            // 
            this.ColumnToCountToolTip.AutoPopDelay = 5000;
            this.ColumnToCountToolTip.InitialDelay = 500;
            this.ColumnToCountToolTip.ReshowDelay = 100;
            this.ColumnToCountToolTip.ShowAlways = true;
            this.ColumnToCountToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // NumericUpDownIdColumn
            // 
            this.NumericUpDownIdColumn.AutoSize = true;
            this.NumericUpDownIdColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.NumericUpDownIdColumn.Location = new System.Drawing.Point(783, 131);
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
            this.NumericUpDownNameColumn.Location = new System.Drawing.Point(783, 93);
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
            // LabelNameColumn
            // 
            this.LabelNameColumn.AutoSize = true;
            this.LabelNameColumn.Location = new System.Drawing.Point(562, 95);
            this.LabelNameColumn.Name = "LabelNameColumn";
            this.LabelNameColumn.Size = new System.Drawing.Size(195, 22);
            this.LabelNameColumn.TabIndex = 26;
            this.LabelNameColumn.Text = "Stĺpec s menom (číslo):";
            this.LabelNameColumn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelIdColumn
            // 
            this.LabelIdColumn.AutoSize = true;
            this.LabelIdColumn.Location = new System.Drawing.Point(562, 133);
            this.LabelIdColumn.Name = "LabelIdColumn";
            this.LabelIdColumn.Size = new System.Drawing.Size(206, 22);
            this.LabelIdColumn.TabIndex = 27;
            this.LabelIdColumn.Text = "Stĺpec s Id čislom (číslo):";
            this.LabelIdColumn.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            // LabelExportDataSeparator
            // 
            this.LabelExportDataSeparator.AutoSize = true;
            this.LabelExportDataSeparator.Location = new System.Drawing.Point(994, 59);
            this.LabelExportDataSeparator.Name = "LabelExportDataSeparator";
            this.LabelExportDataSeparator.Size = new System.Drawing.Size(243, 22);
            this.LabelExportDataSeparator.TabIndex = 29;
            this.LabelExportDataSeparator.Text = "Oddeľovač na export súboru:";
            // 
            // DataStructureSeparatorToolTip
            // 
            this.DataStructureSeparatorToolTip.ShowAlways = true;
            this.DataStructureSeparatorToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // ButtonShowAnalysis
            // 
            this.ButtonShowAnalysis.Location = new System.Drawing.Point(12, 205);
            this.ButtonShowAnalysis.Name = "ButtonShowAnalysis";
            this.ButtonShowAnalysis.Size = new System.Drawing.Size(173, 52);
            this.ButtonShowAnalysis.TabIndex = 30;
            this.ButtonShowAnalysis.Text = "Analysis";
            this.ButtonShowAnalysis.UseVisualStyleBackColor = true;
            this.ButtonShowAnalysis.Click += new System.EventHandler(this.ButtonShowAnalysis_Click);
            // 
            // NumericUpDownDayColumn
            // 
            this.NumericUpDownDayColumn.AutoSize = true;
            this.NumericUpDownDayColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold);
            this.NumericUpDownDayColumn.Location = new System.Drawing.Point(783, 245);
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
            this.NumericUpDownServiceCodeColumn.Location = new System.Drawing.Point(783, 207);
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
            this.NumericUpDownDiagnosisColumn.Location = new System.Drawing.Point(783, 169);
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
            // LabelServiceCodeColumn
            // 
            this.LabelServiceCodeColumn.AutoSize = true;
            this.LabelServiceCodeColumn.Location = new System.Drawing.Point(562, 209);
            this.LabelServiceCodeColumn.Name = "LabelServiceCodeColumn";
            this.LabelServiceCodeColumn.Size = new System.Drawing.Size(199, 22);
            this.LabelServiceCodeColumn.TabIndex = 32;
            this.LabelServiceCodeColumn.Text = "Stĺpec s kódom výkonu:";
            this.LabelServiceCodeColumn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SelectFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1757, 906);
            this.Controls.Add(this.ButtonShowAnalysis);
            this.Controls.Add(this.LabelExportDataSeparator);
            this.Controls.Add(this.ComboBoxExportSeparatorType);
            this.Controls.Add(this.LabelIdColumn);
            this.Controls.Add(this.LabelNameColumn);
            this.Controls.Add(this.NumericUpDownNameColumn);
            this.Controls.Add(this.NumericUpDownIdColumn);
            this.Controls.Add(this.NumericUpDownPointsColumn);
            this.Controls.Add(this.NumericUpDownServiceCodeColumn);
            this.Controls.Add(this.NumericUpDownDayColumn);
            this.Controls.Add(this.NumericUpDownDiagnosisColumn);
            this.Controls.Add(this.LabelServiceCodeColumn);
            this.Controls.Add(this.LabelPointsColumn);
            this.Controls.Add(this.LabelLanguagesChoice);
            this.Controls.Add(this.LanguageComboBox);
            this.Controls.Add(this.ComboBoxImportSeparatorType);
            this.Controls.Add(this.LabelImportDataSeparator);
            this.Controls.Add(this.CheckBoxSelectAll);
            this.Controls.Add(this.DataGridPreview);
            this.Controls.Add(this.LabelTotalPoints);
            this.Controls.Add(this.LabelFileCount);
            this.Controls.Add(this.ListViewShowPointsValues);
            this.Controls.Add(this.ButtonSaveHistory);
            this.Controls.Add(this.TextBoxSelectOutputFolder);
            this.Controls.Add(this.ButtonSelectOutputFolder);
            this.Controls.Add(this.CheckBoxRemoveDuplicatesRows);
            this.Controls.Add(this.CheckBoxSortByName);
            this.Controls.Add(this.CheckBoxRenumberTheOrder);
            this.Controls.Add(this.ButtonProcessData);
            this.Controls.Add(this.ButtonSelectAFile);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "SelectFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownPointsColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownIdColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownNameColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDayColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownServiceCodeColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDiagnosisColumn)).EndInit();

            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ButtonSelectAFile;
        private System.Windows.Forms.Button ButtonProcessData;
        private System.Windows.Forms.CheckBox CheckBoxRenumberTheOrder;
        private System.Windows.Forms.CheckBox CheckBoxSortByName;
        private System.Windows.Forms.CheckBox CheckBoxRemoveDuplicatesRows;
        private System.Windows.Forms.Button ButtonSelectOutputFolder;
        private System.Windows.Forms.TextBox TextBoxSelectOutputFolder;
        private System.Windows.Forms.Button ButtonSaveHistory;
        private System.Windows.Forms.ListView ListViewShowPointsValues;
        private System.Windows.Forms.Label LabelFileCount;
        private System.Windows.Forms.Label LabelTotalPoints;
        private DataGridView DataGridPreview;
        private CheckBox CheckBoxSelectAll;
        private Label LabelImportDataSeparator;
        private ComboBox ComboBoxImportSeparatorType;
        private ComboBox LanguageComboBox;
        private ToolTip LanguageToolTip;
        private Label LabelLanguagesChoice;
        private Label LabelPointsColumn;
        private NumericUpDown NumericUpDownPointsColumn;
        private ToolTip ColumnToCountToolTip;
        private NumericUpDown NumericUpDownIdColumn;
        private NumericUpDown NumericUpDownNameColumn;
        private NumericUpDown NumericUpDownServiceCodeColumn;
        private NumericUpDown NumericUpDownDayColumn;
        private NumericUpDown NumericUpDownDiagnosisColumn;
        private Label LabelServiceCodeColumn;
        private Label LabelNameColumn;
        private Label LabelIdColumn;
        private ComboBox ComboBoxExportSeparatorType;
        private Label LabelExportDataSeparator;
        private ToolTip DataStructureSeparatorToolTip;
        private Button ButtonShowAnalysis;
    }
}

