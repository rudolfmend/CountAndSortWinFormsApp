using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CountAndSortWinFormsAppNetFr4
{
    public partial class TableSettingsForm: Form
    {
        public TableSettingsForm()
        {
            InitializeComponent();
        }

        private void ComboBoxImportSeparatorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnSeparator = ComboBoxImportSeparatorType.SelectedItem.ToString();

            // If the file is loaded, reload the data with a new delimiter
            // Ak je súbor načítaný, znovu načítať dáta s novým oddeľovačom
            if (selectedFilePaths != null && selectedFilePaths.Count > 0)
            {
                try
                {
                    string[] lines = File.ReadAllLines(selectedFilePaths[0]);
                    LoadDataToGrid(lines);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Chyba pri načítaní súboru: {ex.Message}",
                        "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void UpdateFormText()
        {
            LabelImportDataSeparator.Text = Properties.Strings.LabelImportDataSeparator;
            LabelExportDataSeparator.Text = Properties.Strings.LabelExportDataSeparator;
            LabelPointsColumn.Text = Properties.Strings.LabelPointsColumn ?? "Stĺpec s bodmi (číslo):";
            LabelNameColumn.Text = Properties.Strings.LabelNameColumn ?? "Stĺpec s menom:";
            LabelIdColumn.Text = Properties.Strings.LabelIdColumn ?? "Stĺpec s ID:";
            LabelServiceCodeColumn.Text = Properties.Strings.LabelServiceCodeColumn ?? "Stĺpec s kódom služby:";
            LabelDateOfServiceColumn.Text = Properties.Strings.LabelDateOfServiceColumn ?? "Stĺpec s dátumom služby:";
        }

    }
}
