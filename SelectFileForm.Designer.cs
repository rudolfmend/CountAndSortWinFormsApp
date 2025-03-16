using System.Windows.Forms;

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
            this.ButtonSaveHistory = new System.Windows.Forms.Button();
            this.ButtonShowAnalysis = new System.Windows.Forms.Button();
            this.ButtonSelectOutputFolder = new System.Windows.Forms.Button();
            this.ButtonTableSettings = new System.Windows.Forms.Button();
            this.LanguageComboBox = new System.Windows.Forms.ComboBox();
            this.CheckBoxRenumberTheOrder = new System.Windows.Forms.CheckBox();
            this.CheckBoxSortByName = new System.Windows.Forms.CheckBox();
            this.CheckBoxRemoveDuplicatesRows = new System.Windows.Forms.CheckBox();
            this.CheckBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.LanguageToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.DataGridPreview = new System.Windows.Forms.DataGridView();
            this.TextBoxSelectOutputFolder = new System.Windows.Forms.TextBox();
            this.ListViewShowPointsValues = new System.Windows.Forms.ListView();
            this.LabelFileCount = new System.Windows.Forms.Label();
            this.LabelTotalPoints = new System.Windows.Forms.Label();
            this.LabelLanguagesChoice = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridPreview)).BeginInit();
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
            // ButtonShowAnalysis
            // 
            this.ButtonShowAnalysis.Location = new System.Drawing.Point(12, 205);
            this.ButtonShowAnalysis.Name = "ButtonShowAnalysis";
            this.ButtonShowAnalysis.Size = new System.Drawing.Size(173, 52);
            this.ButtonShowAnalysis.TabIndex = 30;
            this.ButtonShowAnalysis.Text = global::CountAndSortWinFormsAppNetFr4.Properties.Strings.Analysis;
            this.ButtonShowAnalysis.UseVisualStyleBackColor = true;
            this.ButtonShowAnalysis.Click += new System.EventHandler(this.ButtonShowAnalysis_Click);
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
            // ButtonTableSettings
            // 
            this.ButtonTableSettings.Location = new System.Drawing.Point(566, 68);
            this.ButtonTableSettings.Name = "ButtonTableSettings";
            this.ButtonTableSettings.Size = new System.Drawing.Size(173, 52);
            this.ButtonTableSettings.TabIndex = 34;
            this.ButtonTableSettings.Text = "Table Settings";
            this.ButtonTableSettings.UseVisualStyleBackColor = true;
            this.ButtonTableSettings.Click += new System.EventHandler(this.ButtonTableSettings_Click);
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
            this.LanguageComboBox.Location = new System.Drawing.Point(1567, 0);
            this.LanguageComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.LanguageComboBox.Name = "LanguageComboBox";
            this.LanguageComboBox.Size = new System.Drawing.Size(144, 28);
            this.LanguageComboBox.TabIndex = 20;
            this.LanguageComboBox.SelectedIndexChanged += new System.EventHandler(this.LanguageComboBox_SelectedIndexChanged);
            // 
            // CheckBoxRenumberTheOrder
            // 
            this.CheckBoxRenumberTheOrder.AutoSize = true;
            this.CheckBoxRenumberTheOrder.Checked = true;
            this.CheckBoxRenumberTheOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxRenumberTheOrder.Location = new System.Drawing.Point(234, 129);
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
            this.CheckBoxSortByName.Location = new System.Drawing.Point(234, 161);
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
            this.CheckBoxRemoveDuplicatesRows.Location = new System.Drawing.Point(234, 193);
            this.CheckBoxRemoveDuplicatesRows.Name = "CheckBoxRemoveDuplicatesRows";
            this.CheckBoxRemoveDuplicatesRows.Size = new System.Drawing.Size(233, 26);
            this.CheckBoxRemoveDuplicatesRows.TabIndex = 7;
            this.CheckBoxRemoveDuplicatesRows.Text = "odstrániť duplicitné riadky";
            this.CheckBoxRemoveDuplicatesRows.UseVisualStyleBackColor = true;
            // 
            // CheckBoxSelectAll
            // 
            this.CheckBoxSelectAll.AutoSize = true;
            this.CheckBoxSelectAll.Location = new System.Drawing.Point(234, 225);
            this.CheckBoxSelectAll.Name = "CheckBoxSelectAll";
            this.CheckBoxSelectAll.Size = new System.Drawing.Size(195, 26);
            this.CheckBoxSelectAll.TabIndex = 17;
            this.CheckBoxSelectAll.Text = "označiť všetky riadky";
            this.CheckBoxSelectAll.UseVisualStyleBackColor = true;
            this.CheckBoxSelectAll.CheckedChanged += new System.EventHandler(this.CheckBoxSelectAll_CheckedChanged);
            // 
            // LanguageToolTip
            // 
            this.LanguageToolTip.ShowAlways = true;
            this.LanguageToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.LanguageToolTip.ToolTipTitle = "Výber jazyka / Language selection / Sprachauswahl";
            // 
            // DataGridPreview
            // 
            this.DataGridPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridPreview.Location = new System.Drawing.Point(12, 275);
            this.DataGridPreview.Name = "DataGridPreview";
            this.DataGridPreview.Size = new System.Drawing.Size(1191, 620);
            this.DataGridPreview.TabIndex = 16;
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
            this.LabelFileCount.Location = new System.Drawing.Point(583, 161);
            this.LabelFileCount.Name = "LabelFileCount";
            this.LabelFileCount.Size = new System.Drawing.Size(131, 22);
            this.LabelFileCount.TabIndex = 14;
            this.LabelFileCount.Text = "LabelFileCount";
            // 
            // LabelTotalPoints
            // 
            this.LabelTotalPoints.AutoSize = true;
            this.LabelTotalPoints.Location = new System.Drawing.Point(583, 199);
            this.LabelTotalPoints.Name = "LabelTotalPoints";
            this.LabelTotalPoints.Size = new System.Drawing.Size(145, 22);
            this.LabelTotalPoints.TabIndex = 15;
            this.LabelTotalPoints.Text = "LabelTotalPoints";
            // 
            // LabelLanguagesChoice
            // 
            this.LabelLanguagesChoice.AutoSize = true;
            this.LabelLanguagesChoice.Dock = System.Windows.Forms.DockStyle.Right;
            this.LabelLanguagesChoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.LabelLanguagesChoice.Location = new System.Drawing.Point(1711, 0);
            this.LabelLanguagesChoice.Margin = new System.Windows.Forms.Padding(0);
            this.LabelLanguagesChoice.Name = "LabelLanguagesChoice";
            this.LabelLanguagesChoice.Size = new System.Drawing.Size(46, 31);
            this.LabelLanguagesChoice.TabIndex = 21;
            this.LabelLanguagesChoice.Text = "🌐";
            // 
            // SelectFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1757, 906);
            this.Controls.Add(this.ButtonTableSettings);
            this.Controls.Add(this.ButtonShowAnalysis);
            this.Controls.Add(this.ButtonSaveHistory);
            this.Controls.Add(this.ButtonSelectOutputFolder);
            this.Controls.Add(this.ButtonProcessData);
            this.Controls.Add(this.ButtonSelectAFile);
            this.Controls.Add(this.LanguageComboBox);
            this.Controls.Add(this.DataGridPreview);
            this.Controls.Add(this.LabelLanguagesChoice);
            this.Controls.Add(this.LabelTotalPoints);
            this.Controls.Add(this.LabelFileCount);
            this.Controls.Add(this.ListViewShowPointsValues);
            this.Controls.Add(this.CheckBoxSelectAll);
            this.Controls.Add(this.CheckBoxRemoveDuplicatesRows);
            this.Controls.Add(this.CheckBoxSortByName);
            this.Controls.Add(this.CheckBoxRenumberTheOrder);
            this.Controls.Add(this.TextBoxSelectOutputFolder);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "SelectFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridPreview)).EndInit();
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
        private Button ButtonTableSettings;
        private Button ButtonShowAnalysis;
        private DataGridView DataGridPreview;
        private CheckBox CheckBoxSelectAll;
        private ComboBox LanguageComboBox;
        private Label LabelLanguagesChoice;
        private ToolTip LanguageToolTip;
    }
}

