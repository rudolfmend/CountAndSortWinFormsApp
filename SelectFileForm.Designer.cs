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
            this.TextBoxSelectedFileDirectory = new System.Windows.Forms.TextBox();
            this.ButtonSelectAFile = new System.Windows.Forms.Button();
            this.ButtonProcessData = new System.Windows.Forms.Button();
            this.CheckBoxRenumberTheOrder = new System.Windows.Forms.CheckBox();
            this.CheckBoxSortByName = new System.Windows.Forms.CheckBox();
            this.CheckBoxRemoveDuplicatesRows = new System.Windows.Forms.CheckBox();
            this.ButtonSelectOutputFolder = new System.Windows.Forms.Button();
            this.TextBoxSelectOutputFolder = new System.Windows.Forms.TextBox();
            this.ButtonSaveHistory = new System.Windows.Forms.Button();
            this.ListViewShowPointsValues = new System.Windows.Forms.ListView();
            this.LabelTotalSum = new System.Windows.Forms.Label();
            this.LabelFileCount = new System.Windows.Forms.Label();
            this.LabelAverage = new System.Windows.Forms.Label();
            this.DataGridPreview = new System.Windows.Forms.DataGridView();
            this.CheckBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.LabelDataStructureSeparatorIs = new System.Windows.Forms.Label();
            this.ComboBoxSeparatorType = new System.Windows.Forms.ComboBox();
            this.LanguageComboBox = new System.Windows.Forms.ComboBox();
            this.LanguageToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // TextBoxSelectedFileDirectory
            // 
            this.TextBoxSelectedFileDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSelectedFileDirectory.Location = new System.Drawing.Point(12, 12);
            this.TextBoxSelectedFileDirectory.Name = "TextBoxSelectedFileDirectory";
            this.TextBoxSelectedFileDirectory.Size = new System.Drawing.Size(1389, 27);
            this.TextBoxSelectedFileDirectory.TabIndex = 0;
            // 
            // ButtonSelectAFile
            // 
            this.ButtonSelectAFile.Location = new System.Drawing.Point(12, 45);
            this.ButtonSelectAFile.Name = "ButtonSelectAFile";
            this.ButtonSelectAFile.Size = new System.Drawing.Size(173, 52);
            this.ButtonSelectAFile.TabIndex = 1;
            this.ButtonSelectAFile.Text = "Výber súboru";
            this.ButtonSelectAFile.UseVisualStyleBackColor = true;
            this.ButtonSelectAFile.Click += new System.EventHandler(this.ButtonSelectAFile_Click);
            // 
            // ButtonProcessData
            // 
            this.ButtonProcessData.Location = new System.Drawing.Point(12, 103);
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
            this.CheckBoxRenumberTheOrder.Location = new System.Drawing.Point(235, 57);
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
            this.CheckBoxSortByName.Location = new System.Drawing.Point(235, 89);
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
            this.CheckBoxRemoveDuplicatesRows.Location = new System.Drawing.Point(235, 121);
            this.CheckBoxRemoveDuplicatesRows.Name = "CheckBoxRemoveDuplicatesRows";
            this.CheckBoxRemoveDuplicatesRows.Size = new System.Drawing.Size(233, 26);
            this.CheckBoxRemoveDuplicatesRows.TabIndex = 7;
            this.CheckBoxRemoveDuplicatesRows.Text = "odstrániť duplicitné riadky";
            this.CheckBoxRemoveDuplicatesRows.UseVisualStyleBackColor = true;
            // 
            // ButtonSelectOutputFolder
            // 
            this.ButtonSelectOutputFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ButtonSelectOutputFolder.Location = new System.Drawing.Point(609, 45);
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
            this.TextBoxSelectOutputFolder.Location = new System.Drawing.Point(788, 56);
            this.TextBoxSelectOutputFolder.Name = "TextBoxSelectOutputFolder";
            this.TextBoxSelectOutputFolder.Size = new System.Drawing.Size(613, 27);
            this.TextBoxSelectOutputFolder.TabIndex = 9;
            this.TextBoxSelectOutputFolder.Text = " ";
            // 
            // ButtonSaveHistory
            // 
            this.ButtonSaveHistory.Location = new System.Drawing.Point(12, 161);
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
            this.ListViewShowPointsValues.Location = new System.Drawing.Point(1211, 219);
            this.ListViewShowPointsValues.Margin = new System.Windows.Forms.Padding(5);
            this.ListViewShowPointsValues.Name = "ListViewShowPointsValues";
            this.ListViewShowPointsValues.Size = new System.Drawing.Size(534, 675);
            this.ListViewShowPointsValues.TabIndex = 12;
            this.ListViewShowPointsValues.TileSize = new System.Drawing.Size(250, 44);
            this.ListViewShowPointsValues.UseCompatibleStateImageBehavior = false;
            this.ListViewShowPointsValues.View = System.Windows.Forms.View.Details;
            // 
            // LabelTotalSum
            // 
            this.LabelTotalSum.AutoSize = true;
            this.LabelTotalSum.Location = new System.Drawing.Point(609, 121);
            this.LabelTotalSum.Name = "LabelTotalSum";
            this.LabelTotalSum.Size = new System.Drawing.Size(0, 22);
            this.LabelTotalSum.TabIndex = 13;
            // 
            // LabelFileCount
            // 
            this.LabelFileCount.AutoSize = true;
            this.LabelFileCount.Location = new System.Drawing.Point(609, 153);
            this.LabelFileCount.Name = "LabelFileCount";
            this.LabelFileCount.Size = new System.Drawing.Size(0, 22);
            this.LabelFileCount.TabIndex = 14;
            // 
            // LabelAverage
            // 
            this.LabelAverage.AutoSize = true;
            this.LabelAverage.Location = new System.Drawing.Point(609, 191);
            this.LabelAverage.Name = "LabelAverage";
            this.LabelAverage.Size = new System.Drawing.Size(0, 22);
            this.LabelAverage.TabIndex = 15;
            // 
            // DataGridPreview
            // 
            this.DataGridPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.DataGridPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridPreview.Location = new System.Drawing.Point(12, 220);
            this.DataGridPreview.Name = "DataGridPreview";
            this.DataGridPreview.Size = new System.Drawing.Size(1191, 675);
            this.DataGridPreview.TabIndex = 16;
            // 
            // CheckBoxSelectAll
            // 
            this.CheckBoxSelectAll.AutoSize = true;
            this.CheckBoxSelectAll.Location = new System.Drawing.Point(235, 153);
            this.CheckBoxSelectAll.Name = "CheckBoxSelectAll";
            this.CheckBoxSelectAll.Size = new System.Drawing.Size(195, 26);
            this.CheckBoxSelectAll.TabIndex = 17;
            this.CheckBoxSelectAll.Text = "označiť všetky riadky";
            this.CheckBoxSelectAll.UseVisualStyleBackColor = true;
            this.CheckBoxSelectAll.CheckedChanged += new System.EventHandler(this.CheckBoxSelectAll_CheckedChanged);
            // 
            // LabelDataStructureSeparatorIs
            // 
            this.LabelDataStructureSeparatorIs.AutoSize = true;
            this.LabelDataStructureSeparatorIs.Location = new System.Drawing.Point(609, 125);
            this.LabelDataStructureSeparatorIs.Name = "LabelDataStructureSeparatorIs";
            this.LabelDataStructureSeparatorIs.Size = new System.Drawing.Size(289, 22);
            this.LabelDataStructureSeparatorIs.TabIndex = 18;
            this.LabelDataStructureSeparatorIs.Text = "Oddeľovač stĺpcov tabuľky je znak:";
            // 
            // ComboBoxSeparatorType
            // 
            this.ComboBoxSeparatorType.AutoCompleteCustomSource.AddRange(new string[] {
            "\"|\"",
            "\";\"",
            "\",\"",
            "\".\"",
            "\" \""});
            this.ComboBoxSeparatorType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComboBoxSeparatorType.Font = new System.Drawing.Font("Roboto", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ComboBoxSeparatorType.FormattingEnabled = true;
            this.ComboBoxSeparatorType.Location = new System.Drawing.Point(904, 121);
            this.ComboBoxSeparatorType.Name = "ComboBoxSeparatorType";
            this.ComboBoxSeparatorType.Size = new System.Drawing.Size(54, 31);
            this.ComboBoxSeparatorType.TabIndex = 19;
            this.ComboBoxSeparatorType.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSeparatorType_SelectedIndexChanged);
            // 
            // LanguageComboBox
            // 
            this.LanguageComboBox.FormattingEnabled = true;
            this.LanguageComboBox.Items.AddRange(new object[] {
            "English",
            "Slovensky",
            "Česky",
            "Deutsch",
            "Polski",
            "Magyar",
            "Українська"});
            this.LanguageComboBox.Location = new System.Drawing.Point(1601, 12);
            this.LanguageComboBox.Name = "LanguageComboBox";
            this.LanguageComboBox.Size = new System.Drawing.Size(121, 28);
            this.LanguageComboBox.TabIndex = 20;
            this.LanguageComboBox.SelectedIndexChanged += new System.EventHandler(this.LanguageComboBox_SelectedIndexChanged);
            // 
            // SelectFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1757, 906);
            this.Controls.Add(this.LanguageComboBox);
            this.Controls.Add(this.ComboBoxSeparatorType);
            this.Controls.Add(this.LabelDataStructureSeparatorIs);
            this.Controls.Add(this.CheckBoxSelectAll);
            this.Controls.Add(this.DataGridPreview);
            this.Controls.Add(this.LabelAverage);
            this.Controls.Add(this.LabelFileCount);
            this.Controls.Add(this.LabelTotalSum);
            this.Controls.Add(this.ListViewShowPointsValues);
            this.Controls.Add(this.ButtonSaveHistory);
            this.Controls.Add(this.TextBoxSelectOutputFolder);
            this.Controls.Add(this.ButtonSelectOutputFolder);
            this.Controls.Add(this.CheckBoxRemoveDuplicatesRows);
            this.Controls.Add(this.CheckBoxSortByName);
            this.Controls.Add(this.CheckBoxRenumberTheOrder);
            this.Controls.Add(this.ButtonProcessData);
            this.Controls.Add(this.ButtonSelectAFile);
            this.Controls.Add(this.TextBoxSelectedFileDirectory);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "SelectFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.DataGridPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxSelectedFileDirectory;
        private System.Windows.Forms.Button ButtonSelectAFile;
        private System.Windows.Forms.Button ButtonProcessData;
        private System.Windows.Forms.CheckBox CheckBoxRenumberTheOrder;
        private System.Windows.Forms.CheckBox CheckBoxSortByName;
        private System.Windows.Forms.CheckBox CheckBoxRemoveDuplicatesRows;
        private System.Windows.Forms.Button ButtonSelectOutputFolder;
        private System.Windows.Forms.TextBox TextBoxSelectOutputFolder;
        private System.Windows.Forms.Button ButtonSaveHistory;
        private System.Windows.Forms.ListView ListViewShowPointsValues;
        private System.Windows.Forms.Label LabelTotalSum;
        private System.Windows.Forms.Label LabelFileCount;
        private System.Windows.Forms.Label LabelAverage;
        private DataGridView DataGridPreview;
        private CheckBox CheckBoxSelectAll;
        private Label LabelDataStructureSeparatorIs;
        private ComboBox ComboBoxSeparatorType;
        private ComboBox LanguageComboBox;
        private ToolTip LanguageToolTip;
    }
}

