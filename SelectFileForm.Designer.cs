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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectFileForm));
            this.TextBoxSelectedFileDirectory = new System.Windows.Forms.TextBox();
            this.ButtonSelectAFile = new System.Windows.Forms.Button();
            this.ButtonProcessData = new System.Windows.Forms.Button();
            this.RichTextBoxDataPreview = new System.Windows.Forms.RichTextBox();
            this.CheckBoxRenumberTheOrder = new System.Windows.Forms.CheckBox();
            this.CheckBoxSortByName = new System.Windows.Forms.CheckBox();
            this.CheckBoxOmitTheHeader = new System.Windows.Forms.CheckBox();
            this.CheckBoxRemoveDuplicatesRows = new System.Windows.Forms.CheckBox();
            this.ButtonSelectOutputFolder = new System.Windows.Forms.Button();
            this.TextBoxSelectOutputFolder = new System.Windows.Forms.TextBox();
            this.ButtonSaveHistory = new System.Windows.Forms.Button();
            this.ListViewShowPointsValues = new System.Windows.Forms.ListView();
            // Inicializácia ListViewShowPointsValues
            this.ListViewShowPointsValues.View = View.Details;
            this.ListViewShowPointsValues.FullRowSelect = true;
            this.ListViewShowPointsValues.GridLines = true;
            this.ListViewShowPointsValues.Columns.Add("Súbor", 150);
            this.ListViewShowPointsValues.Columns.Add("Body", 100);
            this.ListViewShowPointsValues.Columns.Add("Dátum", 150);
            this.LabelTotalSum = new System.Windows.Forms.Label();
            this.LabelFileCount = new System.Windows.Forms.Label();
            this.LabelAverage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TextBoxSelectedFileDirectory
            // 
            this.TextBoxSelectedFileDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSelectedFileDirectory.Location = new System.Drawing.Point(12, 12);
            this.TextBoxSelectedFileDirectory.Name = "TextBoxSelectedFileDirectory";
            this.TextBoxSelectedFileDirectory.Size = new System.Drawing.Size(1310, 27);
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
            // RichTextBoxDataPreview
            // 
            this.RichTextBoxDataPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichTextBoxDataPreview.Location = new System.Drawing.Point(12, 219);
            this.RichTextBoxDataPreview.Name = "RichTextBoxDataPreview";
            this.RichTextBoxDataPreview.Size = new System.Drawing.Size(1039, 675);
            this.RichTextBoxDataPreview.TabIndex = 3;
            this.RichTextBoxDataPreview.Text = "";
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
            this.CheckBoxRenumberTheOrder.CheckedChanged += new System.EventHandler(this.CheckBoxRenumberTheOrder_CheckedChanged);
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
            this.CheckBoxSortByName.CheckedChanged += new System.EventHandler(this.CheckBoxSortByName_CheckedChanged);
            // 
            // CheckBoxOmitTheHeader
            // 
            this.CheckBoxOmitTheHeader.AutoSize = true;
            this.CheckBoxOmitTheHeader.Checked = true;
            this.CheckBoxOmitTheHeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxOmitTheHeader.Location = new System.Drawing.Point(235, 153);
            this.CheckBoxOmitTheHeader.Name = "CheckBoxOmitTheHeader";
            this.CheckBoxOmitTheHeader.Size = new System.Drawing.Size(172, 26);
            this.CheckBoxOmitTheHeader.TabIndex = 6;
            this.CheckBoxOmitTheHeader.Text = "vynechať hlavičku";
            this.CheckBoxOmitTheHeader.UseVisualStyleBackColor = true;
            this.CheckBoxOmitTheHeader.CheckedChanged += new System.EventHandler(this.CheckBoxOmitTheHeader_CheckedChanged);
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
            this.CheckBoxRemoveDuplicatesRows.CheckedChanged += new System.EventHandler(this.CheckBoxRemoveDuplicatesRows_CheckedChanged);
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
            this.TextBoxSelectOutputFolder.Size = new System.Drawing.Size(534, 27);
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
            this.ListViewShowPointsValues.HideSelection = false;
            this.ListViewShowPointsValues.Location = new System.Drawing.Point(1057, 219);
            this.ListViewShowPointsValues.Name = "ListViewShowPointsValues";
            this.ListViewShowPointsValues.Size = new System.Drawing.Size(265, 675);
            this.ListViewShowPointsValues.TabIndex = 12;
            this.ListViewShowPointsValues.UseCompatibleStateImageBehavior = false;
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
            this.LabelAverage.Location = new System.Drawing.Point(609, 177);
            this.LabelAverage.Name = "LabelAverage";
            this.LabelAverage.Size = new System.Drawing.Size(0, 22);
            this.LabelAverage.TabIndex = 15;
            // 
            // SelectFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1334, 906);
            this.Controls.Add(this.LabelAverage);
            this.Controls.Add(this.LabelFileCount);
            this.Controls.Add(this.LabelTotalSum);
            this.Controls.Add(this.ListViewShowPointsValues);
            this.Controls.Add(this.ButtonSaveHistory);
            this.Controls.Add(this.TextBoxSelectOutputFolder);
            this.Controls.Add(this.ButtonSelectOutputFolder);
            this.Controls.Add(this.CheckBoxRemoveDuplicatesRows);
            this.Controls.Add(this.CheckBoxOmitTheHeader);
            this.Controls.Add(this.CheckBoxSortByName);
            this.Controls.Add(this.CheckBoxRenumberTheOrder);
            this.Controls.Add(this.RichTextBoxDataPreview);
            this.Controls.Add(this.ButtonProcessData);
            this.Controls.Add(this.ButtonSelectAFile);
            this.Controls.Add(this.TextBoxSelectedFileDirectory);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "SelectFileForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxSelectedFileDirectory;
        private System.Windows.Forms.Button ButtonSelectAFile;
        private System.Windows.Forms.Button ButtonProcessData;
        private System.Windows.Forms.RichTextBox RichTextBoxDataPreview;
        private System.Windows.Forms.CheckBox CheckBoxRenumberTheOrder;
        private System.Windows.Forms.CheckBox CheckBoxSortByName;
        private System.Windows.Forms.CheckBox CheckBoxOmitTheHeader;
        private System.Windows.Forms.CheckBox CheckBoxRemoveDuplicatesRows;
        private System.Windows.Forms.Button ButtonSelectOutputFolder;
        private System.Windows.Forms.TextBox TextBoxSelectOutputFolder;
        private System.Windows.Forms.Button ButtonSaveHistory;
        private System.Windows.Forms.ListView ListViewShowPointsValues;
        private System.Windows.Forms.Label LabelTotalSum;
        private System.Windows.Forms.Label LabelFileCount;
        private System.Windows.Forms.Label LabelAverage;
    }
}

