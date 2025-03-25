using CountAndSortWinFormsAppNetFr4.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace CountAndSortWinFormsAppNetFr4
{
    public partial class TableSettingsForm : Form
    {
        private SelectFileForm mainForm;
        private string columnSeparator;

        public TableSettingsForm(SelectFileForm mainForm, int pointsIndex, int nameIndex, int idIndex,
        int dayIndex, int serviceCodeIndex, int diagnosisIndex, string separator, int columnCount, int totalLinesColumnIndex)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.columnSeparator = separator;

            // Nastavenie rozsahu hodnôt na základe počtu stĺpcov v tabuľke
            if (columnCount > 1)
            {
                SetupColumnsNumberRange(columnCount);
            }

            // Nastavenie hodnôt pre NumericUpDown kontrolky (už deklarované v designer súbore)
            NumericUpDownPointsColumn.Value = pointsIndex + 1;
            NumericUpDownNameColumn.Value = nameIndex + 1;
            NumericUpDownIdColumn.Value = idIndex + 1;
            NumericUpDownDayColumn.Value = dayIndex + 1;
            NumericUpDownServiceCodeColumn.Value = serviceCodeIndex + 1;
            NumericUpDownDiagnosisColumn.Value = diagnosisIndex + 1;
            //NumericUpDownTotalLines.Value = Settings.Default.TotalLinesColumnIndex;
            NumericUpDownTotalLines.Value = totalLinesColumnIndex + 1;

            // Inicializácia separátorov
            if (ComboBoxImportSeparatorType.Items.Count == 0)
                ComboBoxImportSeparatorType.Items.AddRange(new object[] { "|", ";", ",", ".", " " });

            if (ComboBoxExportSeparatorType.Items.Count == 0)
                ComboBoxExportSeparatorType.Items.AddRange(new object[] { "|", ";", ",", ".", " " });

            // Nastavenie vybraného separátora
            if (ComboBoxImportSeparatorType.Items.Contains(separator))
                ComboBoxImportSeparatorType.SelectedItem = separator;
            else if (ComboBoxImportSeparatorType.Items.Count > 0)
                ComboBoxImportSeparatorType.SelectedIndex = 0;

            if (ComboBoxExportSeparatorType.Items.Contains(separator))
                ComboBoxExportSeparatorType.SelectedItem = separator;
            else if (ComboBoxExportSeparatorType.Items.Count > 0)
                ComboBoxExportSeparatorType.SelectedIndex = 0;

            // Nastavenie ToolTipov
            ColumnToCountToolTip.SetToolTip(LabelPointsColumn, Strings.ColumnToCountToolTip);
            ColumnToCountToolTip.SetToolTip(NumericUpDownPointsColumn, Strings.ColumnToCountToolTip);

            DataStructureSeparatorToolTip.SetToolTip(LabelExportDataSeparator, Strings.DataStructureSeparatorToolTip);
            DataStructureSeparatorToolTip.SetToolTip(LabelImportDataSeparator, Strings.DataStructureSeparatorToolTip);
            DataStructureSeparatorToolTip.SetToolTip(ComboBoxImportSeparatorType, Strings.DataStructureSeparatorToolTip);
            DataStructureSeparatorToolTip.SetToolTip(ComboBoxExportSeparatorType, Strings.DataStructureSeparatorToolTip);

            // V konštruktore TableSettingsForm (na koniec)
            NumericUpDownPointsColumn.ValueChanged += (s, e) => 
            {
                ValidatePointsColumn();
                UpdateFormText();
            };
            // Aktualizovať texty formulára
            UpdateFormText();
        }

        private void ComboBoxImportSeparatorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mainForm != null && mainForm.selectedFilePaths.Count > 0)
            {
                try
                {
                    string[] lines = File.ReadAllLines(mainForm.selectedFilePaths[0]);
                    mainForm.LoadDataToGrid(lines);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Strings.MessageError} Load data error {ex.Message}",
                        $"{Strings.MessageError}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void UpdateFormText()
        {
            // Aktualizácia textov labelov
            LabelImportDataSeparator.Text = Strings.LabelImportDataSeparator;
            LabelExportDataSeparator.Text = Strings.LabelExportDataSeparator;
            LabelPointsColumn.Text = Strings.LabelPointsColumn ?? "Stĺpec s bodmi (číslo):";
            LabelNameColumn.Text = Strings.LabelNameColumn ?? "Stĺpec s menom:";
            LabelIdColumn.Text = Strings.LabelIdColumn ?? "Stĺpec s ID:";
            LabelServiceCodeColumn.Text = Strings.LabelServiceCodeColumn ?? "Stĺpec s kódom výkonu:";
            LabelDateOfServiceColumn.Text = Strings.LabelDateOfServiceColumn ?? "Stĺpec s dátumom výkonu:";
            LabelDiagnosisColumn.Text = Strings.LabelDiagnosisColumn ?? "Stĺpec s diagnózou:";

            // ToolTip nastavenia
            ColumnToCountToolTip.ToolTipTitle = Strings.Settings;
            DataStructureSeparatorToolTip.ToolTipTitle = Strings.Settings;

            ButtonSave.Text = Strings.Save;
            ButtonCancel.Text = Strings.Cancel;
 
            this.Text = Strings.ButtonTableSettings;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            // Uložiť nastavenia
            Settings.Default.PointsColumnIndex = (int)NumericUpDownPointsColumn.Value;
            Settings.Default.NameColumnIndex = (int)NumericUpDownNameColumn.Value;
            Settings.Default.IdColumnIndex = (int)NumericUpDownIdColumn.Value;
            Settings.Default.ServiceCodeColumnIndex = (int)NumericUpDownServiceCodeColumn.Value;
            Settings.Default.DayColumnIndex = (int)NumericUpDownDayColumn.Value;
            Settings.Default.DiagnosisColumnIndex = (int)NumericUpDownDiagnosisColumn.Value;
            Settings.Default.TotalLinesColumnIndex = (int)NumericUpDownTotalLines.Value;
            Settings.Default.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public string GetSelectedSeparator()
        {
            return ComboBoxImportSeparatorType.SelectedItem?.ToString() ?? "|";
        }

        public string GetExportSeparator()
        {
            return ComboBoxExportSeparatorType.SelectedItem?.ToString() ?? "|";
        }

        public void SetupColumnsNumberRange(int columnCount)
        {
            if (columnCount <= 1)
            {
                // Nie sú načítané žiadne údaje
                NumericUpDownPointsColumn.Minimum = 1;
                NumericUpDownPointsColumn.Maximum = 50;
                return;
            }

            // columnCount - 1 pretože prvý stĺpec je checkbox
            NumericUpDownPointsColumn.Minimum = 1;
            NumericUpDownPointsColumn.Maximum = columnCount - 1;

            NumericUpDownNameColumn.Minimum = 1;
            NumericUpDownNameColumn.Maximum = columnCount - 1;

            NumericUpDownIdColumn.Minimum = 1;
            NumericUpDownIdColumn.Maximum = columnCount - 1;

            NumericUpDownDayColumn.Minimum = 1;
            NumericUpDownDayColumn.Maximum = columnCount - 1;

            NumericUpDownServiceCodeColumn.Minimum = 1;
            NumericUpDownServiceCodeColumn.Maximum = columnCount - 1;

            NumericUpDownDiagnosisColumn.Minimum = 1;
            NumericUpDownDiagnosisColumn.Maximum = columnCount - 1;
                        
            NumericUpDownTotalLines.Minimum = 1; //TotalLinesColumnIndex
            NumericUpDownTotalLines.Maximum = columnCount - 1;

        }

        public bool ValidatePointsColumn()
        {
            int columnIndex = (int)NumericUpDownPointsColumn.Value;
            bool isValid = columnIndex < NumericUpDownPointsColumn.Maximum;

            // Vizuálna spätná väzba - zmena farby na základe platnosti
            NumericUpDownPointsColumn.BackColor = isValid ? System.Drawing.SystemColors.Window : System.Drawing.Color.LightPink;

            return isValid;
        }
    }
}
