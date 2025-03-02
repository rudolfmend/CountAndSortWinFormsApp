using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text;

namespace CountAndSortWinFormsAppNetFr4
{
    /// <summary>
    /// Formulár pre zobrazenie a výber súborov na spracovanie
    /// </summary>
    public partial class FileBatchSelectionForm : Form
    {
        private readonly List<string> filePaths;
        private ListView fileListView;
        private Button buttonSelectAll;
        private Button buttonUnselectAll;
        private Button buttonProcess;
        private Button buttonCancel;
        private Label lblTotalFiles;

        public List<string> SelectedFiles { get; private set; } = new List<string>();

        public FileBatchSelectionForm(List<string> filePaths)
        {
            this.filePaths = filePaths;
            this.InitializeComponentCustom();
            PopulateListView();
        }

        private void InitializeComponentCustom()
        {
            this.Text = $"{Properties.Strings.FileSelection}"; //"Výber súborov na spracovanie";
            this.Size = new System.Drawing.Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // Label pre celkový počet súborov
            lblTotalFiles = new Label
            {
                Text = $"{Properties.Strings.TotalFilesSelected} {filePaths.Count}", //Celkový počet vybraných súborov:
                Location = new System.Drawing.Point(10, 10),
                AutoSize = true
            };

            // ListView pre zoznam súborov
            fileListView = new ListView
            {
                Location = new System.Drawing.Point(10, 40),
                Size = new System.Drawing.Size(775, 370),
                View = View.Details,
                CheckBoxes = true,
                FullRowSelect = true,
                GridLines = true
            };

            // Pridanie stĺpcov
            //"Súbor"
            fileListView.Columns.Add(Properties.Strings.ListViewColumnFile, 350);
            //"Veľkosť"
            fileListView.Columns.Add(Properties.Strings.ListViewColumnSize, 100);
            //"Cesta"
            fileListView.Columns.Add(Properties.Strings.ListViewColumnPath, 300);

            // Tlačidlá
            buttonSelectAll = new Button
            {
                Text = Properties.Strings.CheckMarkAll, //"Označiť všetko",
                Location = new System.Drawing.Point(10, 420),
                Size = new System.Drawing.Size(120, 30)
            };
            buttonSelectAll.Click += (sender, e) => SelectAllFiles(true);

            buttonUnselectAll = new Button
            {
                Text = Properties.Strings.UncheckMarkAll, //"Odznačiť všetko", 
                Location = new System.Drawing.Point(140, 420),
                Size = new System.Drawing.Size(120, 30)
            };
            buttonUnselectAll.Click += (sender, e) => SelectAllFiles(false);

            buttonProcess = new Button
            {
                Text = Properties.Strings.ProcessSelected, //"Spracovať vybrané",
                Location = new System.Drawing.Point(550, 420),
                Size = new System.Drawing.Size(120, 30),
                DialogResult = DialogResult.OK
            };
            buttonProcess.Click += ButtonProcess_Click;

            buttonCancel = new Button
            {
                Text = Properties.Strings.Cancel, //"Zrušiť",
                Location = new System.Drawing.Point(680, 420),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            // Pridanie prvkov do formulára
            this.Controls.Add(lblTotalFiles);
            this.Controls.Add(fileListView);
            this.Controls.Add(buttonSelectAll);
            this.Controls.Add(buttonUnselectAll);
            this.Controls.Add(buttonProcess);
            this.Controls.Add(buttonCancel);

            this.AcceptButton = buttonProcess;
            this.CancelButton = buttonCancel;
        }

        private void PopulateListView()
        {
            fileListView.Items.Clear();

            foreach (var filePath in filePaths)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    ListViewItem item = new ListViewItem(fileInfo.Name);
                    item.SubItems.Add(FormatFileSize(fileInfo.Length));
                    item.SubItems.Add(fileInfo.DirectoryName);
                    item.Checked = true; // Všetky súbory sú predvolene označené
                    item.Tag = filePath; // Uložíme celú cestu do Tag

                    fileListView.Items.Add(item);
                }
                catch (Exception ex)
                {
                    // Ak sa vyskytne problém s prístupom k súboru, stále ho pridáme ale s chybovou správou
                    ListViewItem item = new ListViewItem(Path.GetFileName(filePath));
                    item.SubItems.Add($"{Properties.Strings.MessageError} {Properties.Strings.FileAccessProblem}  " + ex.Message);//Chyba:
                    item.SubItems.Add(Path.GetDirectoryName(filePath));
                    item.Checked = false;
                    item.Tag = filePath;

                    fileListView.Items.Add(item);
                }
            }

            UpdateSelectedCount();
        }

        private string FormatFileSize(long bytes)
        {
            string[] suffix = { "B", "KB", "MB", "GB" };
            int i;
            double dblSByte = bytes;

            for (i = 0; i < suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return $"{dblSByte:0.##} {suffix[i]}";
        }

        private void SelectAllFiles(bool selected)
        {
            foreach (ListViewItem item in fileListView.Items)
            {
                item.Checked = selected;
            }

            UpdateSelectedCount();
        }

        private void UpdateSelectedCount()
        {
            int selectedCount = fileListView.CheckedItems.Count;
            lblTotalFiles.Text = $"{Properties.Strings.TotalFilesSelected} {filePaths.Count} ({Properties.Strings.Selected}: {selectedCount})"; //Celkový počet vybraných súborov: {filePaths.Count} (označených: {selectedCount})";

            // Aktualizácia tlačidla spracovať
            buttonProcess.Enabled = selectedCount > 0;
        }

        private void ButtonProcess_Click(object sender, EventArgs e)
        {
            SelectedFiles = new List<string>();

            foreach (ListViewItem item in fileListView.Items)
            {
                if (item.Checked)
                {
                    SelectedFiles.Add(item.Tag.ToString());
                }
            }

            if (SelectedFiles.Count == 0)
            {
                //MessageBox.Show("Nie sú označené žiadne súbory na spracovanie.",
                //    "Upozornenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //this.DialogResult = DialogResult.None;
                MessageBox.Show(Properties.Strings.MessageNoFilesSelected, Properties.Strings.MessageWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
