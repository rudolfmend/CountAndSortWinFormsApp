using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CountAndSortWinFormsAppNetFr4
{
    public partial class SelectFileForm : Form
    {
        // Konštanty pre štruktúru dávkového súboru
        private const int MIN_COLUMNS_REQUIRED = 11;    // Minimálny počet stĺpcov
        private string columnSeparator = string.Empty;  // Oddeľovač stĺpcov

        // Konštanty pre formátovanie výstupu
        private const string DATE_TIME_FORMAT = "dd.MM.yyyy HH:mm:ss";
        private const string NUMBER_FORMAT = "N0";
        private const string PROCESSED_PREFIX = "processed_";

        // Konštanty pre ListView
        private const int COLUMN_WIDTH_FILENAME = 220;
        private const int COLUMN_WIDTH_POINTS = 100;
        private const int COLUMN_WIDTH_DATE = 220;

        private List<string> selectedFilePaths = new List<string>(); // Path to the selected files / Cesta k vybraným súborom
        private int ColumnPointsIndex => (int)NumericUpDownPointsColumn.Value - 1;  // Prevod z 1-indexovaného UI na 0-indexovaný index
        private int ColumnNameIndex => (int)NumericUpDownNameColumn.Value - 1;
        private int ColumnIdIndex => (int)NumericUpDownIdColumn.Value - 1;
        private int duplicatesRemoved = 0; // Pre výpis počtu duplicít

        // Trieda pre sledovanie výsledkov spracovania súborov
        private class FileProcessingResult
        {
            public string FileName { get; set; }
            public bool Success { get; set; }
            public string OutputPath { get; set; }
            public int RemovedRows { get; set; }
            public int OriginalRows { get; set; }
            public int ProcessedRows { get; set; }
            public int OriginalPoints { get; set; }
            public int ProcessedPoints { get; set; }
            public string ErrorMessage { get; set; }
        }

        public SelectFileForm()
        {
            InitializeComponent();

            NumericUpDownPointsColumn.Value = Properties.Settings.Default.PointsColumnIndex > 0 ?
    Properties.Settings.Default.PointsColumnIndex : 11;

            // slovak as default language / slovenčina ako predvolený jazyk
            // Nastavenie predvoleného jazyka // Default language + DropDownStyle
            LanguageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            LanguageComboBox.SelectedIndex = 1; // Index pre "Slovensky"

            NumericUpDownPointsColumn.Value = 11; // Default to column 11 (index 10) / Predvolene na stĺpec 11 (index 10)
            NumericUpDownPointsColumn.ValueChanged += (s, e) => UpdateFormText(); // Aktualizácia popiskov pri zmene stĺpca


            // Nastavenie ToolTipu
            LanguageToolTip.SetToolTip(LanguageComboBox,
                "Wybór języka / Nyelvválasztás / Вибір мови");
            LanguageToolTip.SetToolTip(LabelLanguagesChoice,
                "Wybór języka / Nyelvválasztás / Вибір мови");
            
            ColumnToCountToolTip.SetToolTip(LabelPointsColumn, Properties.Strings.ColumnToCountToolTip);
            ColumnToCountToolTip.SetToolTip(NumericUpDownPointsColumn, Properties.Strings.ColumnToCountToolTip);

            // Získanie názvu a verzie aplikácie z Assembly
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            AssemblyTitleAttribute titleAttribute =
                (AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute));
            string title = titleAttribute?.Title ?? "CountAndSort";

            this.Text = $"{title} v{version.Major}.{version.Minor}.{version.Build}";

            // Načítanie poslednej použitej cesty
            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastOutputFolder))
            {
                TextBoxSelectOutputFolder.Text = Properties.Settings.Default.LastOutputFolder;
            }

            // Inicializácia stĺpcov pre ListView -
            // - bez priameho nastavenia textov stĺpcov lebo lokalizačné zdroje sa nemusia načítať ako prvé a hodnoty jazykov sa neaktualizujú.
            ListViewShowPointsValues.Columns.Clear();
            ListViewShowPointsValues.Columns.Add("", COLUMN_WIDTH_FILENAME);
            ListViewShowPointsValues.Columns.Add("", COLUMN_WIDTH_POINTS);
            ListViewShowPointsValues.Columns.Add("", COLUMN_WIDTH_DATE);

            UpdateFormText(); // správne texty stĺpcov podľa aktuálneho jazyka


            ComboBoxSeparatorType.Items.AddRange(new[] { "|", ";", ",", ".", " " });
            ComboBoxSeparatorType.SelectedItem = columnSeparator;

            ComboBoxSeparatorType.DrawMode = DrawMode.OwnerDrawFixed;
            ComboBoxSeparatorType.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxSeparatorType.DrawItem += ComboBoxSeparatorType_DrawItem;

            ComboBoxSeparatorType.SelectedIndex = 0; // Set first item ("|")
            columnSeparator = ComboBoxSeparatorType.SelectedItem.ToString();

            // Nastavenie alternujúcich farieb riadkov
            DataGridPreview.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            DataGridPreview.RowsDefaultCellStyle.BackColor = Color.White;
            DataGridPreview.BackgroundColor = Color.White;

            // Voliteľné vylepšenia pre lepší vzhľad
            DataGridPreview.GridColor = Color.LightGray;
            DataGridPreview.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            DataGridPreview.BorderStyle = BorderStyle.Fixed3D;

            //  obslužná rutina udalosti pre zmenu hodnoty NumericUpDownPointsColumn
            NumericUpDownPointsColumn.ValueChanged += (s, e) =>
            {
                ValidatePointsColumn();
                UpdateFormText();
            };
        }

        // Aktualizácia konštruktora pre inicializáciu DataGridView s checkbox v záhlaví
        private void InitializeDataGridWithCheckBoxHeader()
        {
            if (DataGridPreview.Columns.Count > 0 && DataGridPreview.Columns[0].Name == "Selected")
            {
                // Vytvoríme checkbox header pre stĺpec Selected
                var headerCell = new DataGridViewCheckBoxHeaderCell();
                headerCell.OnCheckBoxClicked += (sender, isChecked) =>
                {
                    // Označíme alebo odznačíme všetky riadky
                    foreach (DataGridViewRow row in DataGridPreview.Rows)
                    {
                        row.Cells["Selected"].Value = isChecked;
                    }
                    // Aktualizujeme aj stav checkbox kontrolky
                    CheckBoxSelectAll.Checked = isChecked;
                };

                DataGridPreview.Columns[0].HeaderCell = headerCell;
                DataGridPreview.InvalidateColumn(0);
            }
        }


        private void SaveColumnSettings()
        {
            Properties.Settings.Default.PointsColumnIndex = (int)NumericUpDownPointsColumn.Value;
            Properties.Settings.Default.NameColumnIndex = (int)NumericUpDownNameColumn.Value;
            Properties.Settings.Default.IdColumnIndex = (int)NumericUpDownIdColumn.Value;
            Properties.Settings.Default.Save();
        }


        private class ProcessedFileInfo
        {
            // properties for unique identification
            public string FileName { get; set; }
            public string FullPath { get; set; }
            public DateTime ProcessedTime { get; set; }
            public int Points { get; set; }
            public string UniqueIdentifier { get; set; }
            public DateTime FileCreationTime { get; set; }
            public long FileSize { get; set; }
            public string SourceDirectory { get; set; }

            // property for duplicate count
            public int RemovedDuplicates { get; set; }
            public int DuplicatesRemoved { get; set; }

           

            public ProcessedFileInfo(string fileName, string fullPath, DateTime time = default, int points = 0, string sourceDirectory = "")
            {
                FileName = fileName;
                FullPath = Path.GetFullPath(fullPath); // Convert to absolute path
                ProcessedTime = time == default ? DateTime.Now : time;
                Points = points;
                // Store the source directory from the input file / Na uloženie zdrojového adresára zo vstupného súboru
                //SourceDirectory = Path.GetDirectoryName(selectedFilePaths.Text);
                SourceDirectory = sourceDirectory;

                try
                {
                    var fileInfo = new FileInfo(FullPath);
                    FileCreationTime = fileInfo.CreationTime;
                    FileSize = fileInfo.Length;
                    UniqueIdentifier = $"{fileName}_{FileCreationTime.Ticks}_{FileSize}";
                }
                catch (Exception)
                {
                    // Handle file not found gracefully
                    FileCreationTime = DateTime.Now;
                    FileSize = 0;
                    UniqueIdentifier = $"{fileName}_{DateTime.Now.Ticks}";
                }
            }

            public override bool Equals(object obj)
            {
                if (obj is ProcessedFileInfo other)
                {
                    return UniqueIdentifier == other.UniqueIdentifier;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return UniqueIdentifier.GetHashCode();
            }
        }

        private readonly HashSet<ProcessedFileInfo> processedFiles = new HashSet<ProcessedFileInfo>();

        private bool IsFileAlreadyProcessed(string filePath)
        {
            var file = new ProcessedFileInfo(
                Path.GetFileName(filePath),
                Path.GetFullPath(filePath));
            return processedFiles.Contains(file);
        }

        private void AddToHistory(string filePath, int points, string sourceDirectory)
        {
            string fileName = Path.GetFileName(filePath);
            string fullPath = Path.GetFullPath(filePath);
            DateTime now = DateTime.Now;

            var newFileInfo = new ProcessedFileInfo(fileName, fullPath, now, points, sourceDirectory);

            // Pridáme nový súbor ak ešte neexistuje
            if (!processedFiles.Any(f => f.UniqueIdentifier == newFileInfo.UniqueIdentifier))
            {
                processedFiles.Add(newFileInfo);
            }

            // Aktualizujeme ListView
            ListViewShowPointsValues.Items.Clear();
            foreach (var file in processedFiles.OrderBy(f => f.ProcessedTime))
            {
                ListViewItem item = new ListViewItem(new[]
                {
            file.FileName,
            file.Points.ToString(NUMBER_FORMAT),
            file.ProcessedTime.ToString(DATE_TIME_FORMAT)
        });
                ListViewShowPointsValues.Items.Add(item);
            }

            // Po pridaní nových dát aktualizujeme štatistiky
            UpdateStatistics();

            // A tiež aktualizujeme texty labelov, aby sa zobrazili v správnom jazyku
            UpdateFormText();
        }

        private void ButtonSelectAFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Všetky súbory (*.*)|*.*|súbory (*.001)|*.001|súbory (*.002)|*.002|Textové súbory (*.txt)|*.txt";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = true; // Povolenie výberu viacerých súborov
                CheckBoxSelectAll.Checked = false; //Reset CheckBoxSelectAll

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePaths = openFileDialog.FileNames.ToList();
                    ButtonProcessData.Enabled = selectedFilePaths.Count > 0;

                    // Ak je vybratý aspoň jeden súbor, načítame prvý pre náhľad
                    if (selectedFilePaths.Count > 0)
                    {
                        try
                        {
                            string[] lines = File.ReadAllLines(selectedFilePaths[0]);
                            LoadDataToGrid(lines);

                            // Nastavenia DataGridView ktoré sú špecifické pre inicializáciu
                            DataGridPreview.AllowUserToAddRows = false;
                            DataGridPreview.AllowUserToDeleteRows = false;
                            DataGridPreview.MultiSelect = true;
                            DataGridPreview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                            // Povoliť editáciu len pre checkbox stĺpec
                            foreach (DataGridViewColumn col in DataGridPreview.Columns)
                            {
                                col.ReadOnly = col.Name != "Selected";
                            }

                            // Aktualizácia titulku - zobraziť počet súborov
                            var baseTitle = this.Text.Split('-')[0].Trim();
                            if (selectedFilePaths.Count == 1)
                            {
                                // Ak je len jeden súbor, zobrazíme jeho názov
                                this.Text = $"{baseTitle} - {Path.GetFileName(selectedFilePaths[0])}";
                            }
                            else
                            {
                                // Ak je viac súborov, zobrazíme ich počet
                                this.Text = $"{baseTitle} - {selectedFilePaths.Count} súborov vybraných";

                                // Keď sa vyberie viac súborov, môžeme zobraziť informáciu o počte vybraných súborov
                                ShowMultipleFilesSelectedInfo();
                            }

                            CheckBoxSelectAll.Checked = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Chyba pri čítaní súboru: {ex.Message}",
                                "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void ShowMultipleFilesSelectedInfo()
        {
            // Vytvoríme zoznam prvých 5 súborov pre zobrazenie
            var filesList = new System.Text.StringBuilder();
            int maxFilesToShow = Math.Min(selectedFilePaths.Count, 5);

            for (int i = 0; i < maxFilesToShow; i++)
            {
                filesList.AppendLine($"- {Path.GetFileName(selectedFilePaths[i])}");
            }

            if (selectedFilePaths.Count > 5)
            {
                filesList.AppendLine($"- ... a ďalších {selectedFilePaths.Count - 5} súborov");
            }

            MessageBox.Show(
                $"Vybraných {selectedFilePaths.Count} súborov.\n\n" +
                $"V tabuľke je zobrazený náhľad prvého súboru. Všetky súbory budú spracované po kliknutí na 'Spracovať údaje'.\n\n" +
                $"Zoznam súborov:\n{filesList}",
                "Informácia o výbere súborov",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private async Task<List<FileProcessingResult>> ProcessSingleFileWithSelectedRows(string filePath)
        {
            List<FileProcessingResult> results = new List<FileProcessingResult>();

            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show(Properties.Strings.MessageFileNotExist,
                        Properties.Strings.MessageError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    results.Add(new FileProcessingResult
                    {
                        FileName = Path.GetFileName(filePath),
                        Success = false,
                        ErrorMessage = Properties.Strings.MessageFileNotExist
                    });

                    return results;
                }

                string currentFile = Path.GetFileName(filePath);
                string sourceDirectory = Path.GetDirectoryName(filePath);

                if (IsFileAlreadyProcessed(filePath))
                {
                    var dialogResult = MessageBox.Show(
                        Properties.Strings.MessageFileAlreadyProcessed,
                        Properties.Strings.MessageWarning,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.No)
                    {
                        results.Add(new FileProcessingResult
                        {
                            FileName = currentFile,
                            Success = false,
                            ErrorMessage = "Užívateľ zrušil spracovanie už existujúceho súboru"
                        });

                        return results;
                    }
                }

                var originalLines = (await Task.Factory.StartNew(() => File.ReadAllLines(filePath))).ToList();
                var processedLines = await ProcessDataAsync(originalLines);

                if (processedLines.Count == 0)
                {
                    MessageBox.Show(Properties.Strings.MessageNoRowsSelected,
                        Properties.Strings.MessageWarning,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    results.Add(new FileProcessingResult
                    {
                        FileName = currentFile,
                        Success = false,
                        ErrorMessage = Properties.Strings.MessageNoRowsSelected
                    });

                    return results;
                }

                int originalPoints = await CalculateTotalPointsAsync(originalLines);
                string outputPath = await SaveProcessedDataAsync(processedLines, filePath);
                int processedPoints = await CalculateTotalPointsAsync(processedLines);

                // Pridáme do histórie a aktualizujeme štatistiky
                AddToHistory(filePath, processedPoints, sourceDirectory);

                // Vytvoríme výsledok spracovania
                results.Add(new FileProcessingResult
                {
                    FileName = currentFile,
                    Success = true,
                    OutputPath = outputPath,
                    RemovedRows = originalLines.Count - processedLines.Count,
                    OriginalRows = originalLines.Count,
                    ProcessedRows = processedLines.Count,
                    OriginalPoints = originalPoints,
                    ProcessedPoints = processedPoints
                });

                return results;
            }
            catch (Exception ex)
            {
                results.Add(new FileProcessingResult
                {
                    FileName = Path.GetFileName(filePath),
                    Success = false,
                    ErrorMessage = ex.Message
                });

                return results;
            }
        }

        // Method for processing multiple files with progress indicator / Metóda pre spracovanie viacerých súborov s indikátorom priebehu
        private async Task<List<FileProcessingResult>> ProcessMultipleFilesAsync(List<string> filePaths)
        {
            List<FileProcessingResult> results = new List<FileProcessingResult>();

            // Kontrola, či chce užívateľ pokračovať, ak niektoré súbory už boli spracované
            if (!ConfirmProcessingOfAlreadyProcessedFiles(filePaths))
            {
                // Užívateľ si neprial pokračovať, vrátime prázdny zoznam
                return results;
            }

            // Vytvorenie a zobrazenie progress formulára
            using (var progressForm = new ProgressForm("Spracovanie súborov"))
            {
                // Spustenie progress formy
                progressForm.Show(this);

                // Spracovanie každého súboru
                for (int i = 0; i < filePaths.Count; i++)
                {
                    string currentFilePath = filePaths[i];
                    string currentFile = Path.GetFileName(currentFilePath);

                    // Aktualizácia progress baru
                    progressForm.UpdateProgress(i + 1, filePaths.Count, currentFile);

                    // Kontrola existencie súboru
                    if (!File.Exists(currentFilePath))
                    {
                        results.Add(new FileProcessingResult
                        {
                            FileName = currentFile,
                            Success = false,
                            ErrorMessage = Properties.Strings.MessageFileNotExist
                        });
                        continue;
                    }

                    string sourceDirectory = Path.GetDirectoryName(currentFilePath);

                    try
                    {
                        // Načítanie a spracovanie súboru
                        var originalLines = await Task.Factory.StartNew(() =>
                            File.ReadAllLines(currentFilePath).ToList());

                        // Spracovanie dát súboru
                        List<string> processedLines;
                        if (CheckBoxRemoveDuplicatesRows.Checked)
                        {
                            var originalCount = originalLines.Count;
                            processedLines = RemoveDuplicateRows(originalLines);
                            duplicatesRemoved = originalCount - processedLines.Count;
                        }
                        else
                        {
                            processedLines = new List<string>(originalLines);
                            if (CheckBoxSortByName.Checked)
                            {
                                processedLines = SortLinesByName(processedLines);
                            }
                            if (CheckBoxRenumberTheOrder.Checked)
                            {
                                processedLines = RenumberFirstColumn(processedLines);
                            }
                        }

                        if (processedLines.Count == 0)
                        {
                            results.Add(new FileProcessingResult
                            {
                                FileName = currentFile,
                                Success = false,
                                ErrorMessage = Properties.Strings.MessageNoRowsSelected
                            });
                            continue;
                        }

                        // Výpočet bodov a uloženie súboru
                        int originalPoints = await CalculateTotalPointsAsync(originalLines);
                        string outputPath = await SaveProcessedDataAsync(processedLines, currentFilePath);
                        int processedPoints = await CalculateTotalPointsAsync(processedLines);

                        // Pridáme do histórie
                        AddToHistory(currentFilePath, processedPoints, sourceDirectory);

                        // Pridanie výsledku
                        results.Add(new FileProcessingResult
                        {
                            FileName = currentFile,
                            Success = true,
                            OutputPath = outputPath,
                            RemovedRows = originalLines.Count - processedLines.Count,
                            OriginalRows = originalLines.Count,
                            ProcessedRows = processedLines.Count,
                            OriginalPoints = originalPoints,
                            ProcessedPoints = processedPoints,
                            ErrorMessage = string.Empty
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Chyba pri spracovaní súboru {currentFile}: {ex.Message}");
                        results.Add(new FileProcessingResult
                        {
                            FileName = currentFile,
                            Success = false,
                            ErrorMessage = ex.Message
                        });
                    }
                }

                // Zatvorenie progress formy
                progressForm.Close();
            }

            return results;
        }


        /// <summary>
        /// Záznamy sa najprv zoradia podľa mena (stĺpec 4).
        /// Ak sú mená rovnaké, zoradia sa podľa čísla v druhom stĺpci.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private List<string> SortLinesByName(List<string> lines)
        {
            return lines
                .Select(line => line.Split(new[] { columnSeparator }, StringSplitOptions.None))
                .Where(parts => parts.Length > 4 && int.TryParse(parts[0], out _))
                .OrderBy(parts => parts[ColumnNameIndex])      // Použite vlastnosť namiesto ovládacieho prvku
                .ThenBy(parts => int.Parse(parts[ColumnIdIndex])) // Použite vlastnosť namiesto ovládacieho prvku
                .Select(parts => string.Join(columnSeparator, parts))
                .ToList();
        }

        private List<string> RenumberFirstColumn(List<string> lines)
        {
            int newNumber = 1;
            return lines
                .Select(line =>
                {
                    var parts = line.Split(new[] { columnSeparator }, StringSplitOptions.None);
                    if (parts.Length > 4 && int.TryParse(parts[0], out _))
                    {
                        parts[0] = newNumber++.ToString();
                        return string.Join(columnSeparator, parts);
                    }
                    return line;
                })
                .ToList();
        }

        private List<string> RemoveDuplicateRows(List<string> lines)
        {
            return lines
                .Select(line => line.Split(new[] { columnSeparator }, StringSplitOptions.None))
                .Where(parts => parts.Length > MIN_COLUMNS_REQUIRED && int.TryParse(parts[0], out _))
                .GroupBy(parts => $"{parts[ColumnIdIndex]}|{parts[2]}|{parts[5]}")
                .Select(group => group.First())  // Vybrať prvý záznam z každej skupiny
                .OrderBy(parts => parts[ColumnNameIndex]) // Použite vlastnosť namiesto ovládacieho prvku
                .Select((parts, index) => $"{index + 1}|{string.Join(columnSeparator, parts.Skip(1))}") // Prečíslovať
                .ToList();
        }

        /// <summary>
        /// Spracovanie údajov podľa nastavení checkboxov.
        /// Umožňuje užívateľovi vybrať priečinok pre uloženie súboru.
        /// Zachováva vybratú cestu v TextBoxe.
        /// Pri opätovnom otvorení dialógu začne od naposledy vybraného priečinka.
        /// Ak užívateľ nevyberie výstupný priečinok, použije sa pôvodný priečinok vstupného súboru
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private async void ButtonProcessData_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonProcessData.Enabled = false;
                ButtonSelectAFile.Enabled = false;
                ButtonSelectOutputFolder.Enabled = false;

                // 1. Najprv kontrolujeme či je vôbec zadaná cesta k súboru
                if (selectedFilePaths.Count == 0)
                {
                    MessageBox.Show(Properties.Strings.MessageSelectFile,
                        Properties.Strings.MessageWarning,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidatePointsColumn())
                {
                    MessageBox.Show(Properties.Strings.MessageInvalidColumn ??
                        "Vybraný stĺpec pre body neexistuje v súbore. Prosím vyberte platný stĺpec.",
                        Properties.Strings.MessageWarning,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Uloženie nastavení stĺpcov
                SaveColumnSettings();

                // Spracovanie súborov a získanie výsledkov
                List<FileProcessingResult> results;
                int totalFilesProcessed;
                int totalOriginalPoints;
                int totalProcessedPoints;

                // Ak je vybraný len jeden súbor, použijeme označené riadky z tabuľky
                if (selectedFilePaths.Count == 1)
                {
                    results = await ProcessSingleFileWithSelectedRows(selectedFilePaths[0]);
                }
                else
                {
                    // Pre viac súborov, spracujeme každý súbor kompletne
                    results = await ProcessMultipleFilesAsync(selectedFilePaths);
                }

                // Výpočet súhrnných štatistík
                totalFilesProcessed = results.Count(r => r.Success);
                totalOriginalPoints = results.Where(r => r.Success).Sum(r => r.OriginalPoints);
                totalProcessedPoints = results.Where(r => r.Success).Sum(r => r.ProcessedPoints);

                // Zobrazenie výsledkov
                ShowProcessingResults(results, totalFilesProcessed, totalOriginalPoints, totalProcessedPoints);

                // Vynútime aktualizáciu štatistík s oneskorením
                DelayedUpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba pri spracovaní údajov: {ex.Message}",
                    "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ButtonProcessData.Enabled = true;
                ButtonSelectAFile.Enabled = true;
                ButtonSelectOutputFolder.Enabled = true;
                UpdateStatistics();
            }
        }

        // Metóda na zobrazenie súhrnných výsledkov
        private void ShowProcessingResults(List<FileProcessingResult> results, int totalFilesProcessed,
            int totalOriginalPoints, int totalProcessedPoints)
        {
            if (results is null || results.Count == 0)
            {
                return;
            }

            // Ak bol spracovaný len jeden súbor, použijeme pôvodnú správu
            if (results.Count == 1 && results[0].Success)
            {
                var result = results[0];
                MessageBox.Show(
                    string.Format(Properties.Strings.MessageProcessingResults,
                        result.OutputPath,
                        result.RemovedRows,
                        result.OriginalRows,
                        result.ProcessedRows,
                        result.OriginalPoints,
                        result.ProcessedPoints),
                    Properties.Strings.MessageDone,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // Pre viac súborov vytvoríme súhrnnú správu
            StringBuilder message = new StringBuilder();
            message.AppendLine($"Spracovanie súborov dokončené.");
            message.AppendLine($"------------------------------------------");
            message.AppendLine($"Spracovaných: {totalFilesProcessed} z {results.Count} súborov");
            message.AppendLine($"Celkový počet bodov pred: {totalOriginalPoints:N0}");
            message.AppendLine($"Celkový počet bodov po: {totalProcessedPoints:N0}");
            message.AppendLine($"Rozdiel bodov: {totalProcessedPoints - totalOriginalPoints:N0}");
            message.AppendLine($"------------------------------------------");

            // Zoznam súborov podľa statusu
            int maxFilesToShow = 10; // Zobrazíme maximálne prvých 10 súborov

            // Úspešne spracované súbory
            var successfulFiles = results.Where(r => r.Success).ToList();
            if (successfulFiles.Any())
            {
                message.AppendLine();
                message.AppendLine("Úspešne spracované súbory:");

                for (int i = 0; i < Math.Min(successfulFiles.Count, maxFilesToShow); i++)
                {
                    var result = successfulFiles[i];
                    message.AppendLine($"- {result.FileName}: {result.ProcessedPoints:N0} bodov, odstránených {result.RemovedRows} riadkov");
                }

                if (successfulFiles.Count > maxFilesToShow)
                {
                    message.AppendLine($"- ... a ďalších {successfulFiles.Count - maxFilesToShow} súborov");
                }
            }

            // Neúspešne spracované súbory
            var failedFiles = results.Where(r => !r.Success).ToList();
            if (failedFiles.Any())
            {
                message.AppendLine();
                message.AppendLine("Súbory, ktoré neboli spracované:");

                for (int i = 0; i < Math.Min(failedFiles.Count, maxFilesToShow); i++)
                {
                    var result = failedFiles[i];
                    message.AppendLine($"- {result.FileName}: {result.ErrorMessage}");
                }

                if (failedFiles.Count > maxFilesToShow)
                {
                    message.AppendLine($"- ... a ďalších {failedFiles.Count - maxFilesToShow} súborov");
                }
            }

            // Opýtame sa užívateľa, či chce výsledky uložiť do súboru
            var dialogResult = MessageBox.Show(
                message.ToString() + "\n\nChcete uložiť podrobný výsledok do súboru?",
                Properties.Strings.MessageDone,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                SaveProcessingResults(results, totalFilesProcessed, totalOriginalPoints, totalProcessedPoints);
            }
        }

        private void SaveProcessingResults(List<FileProcessingResult> results, int totalFilesProcessed,
            int totalOriginalPoints, int totalProcessedPoints)
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = Properties.Strings.DialogFilterTextFiles;
                    saveDialog.FilterIndex = 1;
                    saveDialog.DefaultExt = "txt";
                    saveDialog.FileName = $"BatchProcessingResults_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder content = new StringBuilder();
                        content.AppendLine("VÝSLEDKY DÁVKOVÉHO SPRACOVANIA SÚBOROV");
                        content.AppendLine($"Dátum a čas: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                        content.AppendLine("------------------------------------------");
                        content.AppendLine($"Spracovaných: {totalFilesProcessed} z {results.Count} súborov");
                        content.AppendLine($"Celkový počet bodov pred: {totalOriginalPoints:N0}");
                        content.AppendLine($"Celkový počet bodov po: {totalProcessedPoints:N0}");
                        content.AppendLine($"Rozdiel bodov: {totalProcessedPoints - totalOriginalPoints:N0}");
                        content.AppendLine("------------------------------------------");

                        // Podrobný výpis pre všetky súbory
                        content.AppendLine("\nPODROBNÉ VÝSLEDKY PRE KAŽDÝ SÚBOR:");

                        // Úspešne spracované súbory
                        var successfulFiles = results.Where(r => r.Success).ToList();
                        if (successfulFiles.Any())
                        {
                            content.AppendLine("\nÚSPEŠNE SPRACOVANÉ SÚBORY:");
                            foreach (var result in successfulFiles)
                            {
                                content.AppendLine($"\n{result.FileName}");
                                content.AppendLine($"- Výstupný súbor: {result.OutputPath}");
                                content.AppendLine($"- Počet bodov pred: {result.OriginalPoints:N0}");
                                content.AppendLine($"- Počet bodov po: {result.ProcessedPoints:N0}");
                                content.AppendLine($"- Rozdiel bodov: {result.ProcessedPoints - result.OriginalPoints:N0}");
                                content.AppendLine($"- Počet riadkov pred: {result.OriginalRows}");
                                content.AppendLine($"- Počet riadkov po: {result.ProcessedRows}");
                                content.AppendLine($"- Odstránené duplicity: {result.RemovedRows}");
                            }
                        }

                        // Neúspešne spracované súbory
                        var failedFiles = results.Where(r => !r.Success).ToList();
                        if (failedFiles.Any())
                        {
                            content.AppendLine("\nNESPRACOVANÉ SÚBORY:");
                            foreach (var result in failedFiles)
                            {
                                content.AppendLine($"\n{result.FileName}");
                                content.AppendLine($"- Dôvod: {result.ErrorMessage}");
                            }
                        }

                        File.WriteAllText(saveDialog.FileName, content.ToString());
                        MessageBox.Show(
                            $"Výsledky boli úspešne uložené do súboru:\n{saveDialog.FileName}",
                            "Informácia",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Chyba pri ukladaní výsledkov: {ex.Message}",
                    "Chyba",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    

        // Kontrola, či chce užívateľ prepísať súbory, ktoré už boli spracované
        private bool ConfirmProcessingOfAlreadyProcessedFiles(List<string> filePaths)
        {
            // Kontrola, koľko súborov bolo už spracovaných
            var alreadyProcessedFiles = filePaths
                .Where(file => IsFileAlreadyProcessed(file))
                .Select(file => Path.GetFileName(file))
                .ToList();

            if (alreadyProcessedFiles.Count == 0)
                return true;

            // Ak je len jeden súbor, použijeme štandardnú správu
            if (alreadyProcessedFiles.Count == 1 && filePaths.Count == 1)
            {
                var result = MessageBox.Show(Properties.Strings.MessageFileAlreadyProcessed,
                    Properties.Strings.MessageWarning,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return result == DialogResult.Yes;
            }

            // Pre viac súborov zostavíme zoznam a spýtame sa raz
            var message = new StringBuilder();
            message.AppendLine($"{alreadyProcessedFiles.Count} z {filePaths.Count} vybraných súborov už bolo spracovaných:");

            // Zobraziť prvých 5 súborov v správe
            int showCount = Math.Min(alreadyProcessedFiles.Count, 5);
            for (int i = 0; i < showCount; i++)
            {
                message.AppendLine($"- {alreadyProcessedFiles[i]}");
            }

            // Ak je viac ako 5 súborov, pridáme poznámku
            if (alreadyProcessedFiles.Count > 5)
            {
                message.AppendLine($"- ... a ďalších {alreadyProcessedFiles.Count - 5} súborov");
            }

            message.AppendLine("\nChcete tieto súbory spracovať znova?");

            var dialogResult = MessageBox.Show(
                message.ToString(),
                Properties.Strings.MessageWarning,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            return dialogResult == DialogResult.Yes;
        }

        private List<string> GetUnselectedRows()
        {
            List<string> unselectedRows = new List<string>();
            foreach (DataGridViewRow row in DataGridPreview.Rows)
            {
                if (!(bool)row.Cells["Selected"].Value)
                {
                    var cells = row.Cells.Cast<DataGridViewCell>()
                                   .Skip(1)
                                   .Select(c => c.Value?.ToString() ?? "");
                    unselectedRows.Add(string.Join(columnSeparator, cells));
                }
            }
            return unselectedRows;
        }

        private async Task<List<string>> ProcessDataAsync(List<string> _lines)
        {
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    // Získame označené riadky
                    var dataLines = GetSelectedRows();

                    // Kontrola prázdneho výberu - vrátime prázdny list namiesto vyhodenia výnimky
                    if (dataLines.Count == 0)
                    {
                        return new List<string>();
                    }

                    // Spracovanie podľa nastavených možností
                    List<string> processedLines = new List<string>(dataLines);

                    if (CheckBoxRemoveDuplicatesRows.Checked)
                    {
                        var originalCount = processedLines.Count;
                        processedLines = RemoveDuplicateRows(processedLines);
                        duplicatesRemoved = originalCount - processedLines.Count;
                    }
                    else
                    {
                        if (CheckBoxSortByName.Checked)
                        {
                            processedLines = SortLinesByName(processedLines);
                        }

                        if (CheckBoxRenumberTheOrder.Checked)
                        {
                            processedLines = RenumberFirstColumn(processedLines);
                        }
                    }

                    return processedLines;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Chyba pri spracovaní dát: {ex.Message}");
                    throw;
                }
            });
        }

        private async Task<int> CalculateTotalPointsAsync(List<string> lines)
        {
            return await Task.Factory.StartNew(() =>
            {
                int totalPoints = 0;
                int lineCount = 0;
                int errorCount = 0;

              //foreach (var line in lines.Skip(HEADER_LINES))
                foreach (var line in lines)
                {
                    lineCount++;
                    var parts = line.Split(new[] { columnSeparator }, StringSplitOptions.None);

                    if (parts.Length <= ColumnPointsIndex)
                    {
                        Debug.WriteLine($"Riadok {lineCount}: Nedostatočný počet stĺpcov ({parts.Length})");
                        errorCount++;
                        continue;
                    }

                    string value = parts[ColumnPointsIndex].Trim();
                    if (string.IsNullOrEmpty(value))
                    {
                        Debug.WriteLine($"Riadok {lineCount}: Prázdna hodnota bodov");
                        errorCount++;
                        continue;
                    }

                    if (!int.TryParse(value, out int points))
                    {
                        Debug.WriteLine($"Riadok {lineCount}: Neplatná hodnota bodov: '{value}'");
                        errorCount++;
                        continue;
                    }

                    totalPoints += points;
                    Debug.WriteLine($"Riadok {lineCount}: Pridaných {points} bodov. Celkový súčet: {totalPoints}");
                }

                Debug.WriteLine($"Celkový počet chybných riadkov: {errorCount}");
                Debug.WriteLine($"Celkový počet spracovaných riadkov: {lineCount}");
                Debug.WriteLine($"Celkový súčet bodov: {totalPoints}");
                return totalPoints;
            });
        }

        //Helper method for controlling folder access / Pomocná metóda na kontrolu prístupu k priečinku
        private bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                //Try to create a test file / Pokus o vytvorenie testovacieho súboru 
                string testFile = Path.Combine(folderPath, "test.txt");
                using (FileStream fs = File.Create(testFile))
                {
                    fs.Close();
                }
                File.Delete(testFile);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Helper class for file operations / Pomocná trieda pre File operácie
        public static class FileExtensions
        {
            public static Task<string[]> ReadAllLinesAsync(string path)
            {
                return Task.Factory.StartNew(() => File.ReadAllLines(path));
            }

            public static Task WriteAllLinesAsync(string path, IEnumerable<string> lines)
            {
                return Task.Factory.StartNew(() => File.WriteAllLines(path, lines));
            }
        }

        private void ButtonSelectOutputFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = Properties.Strings.DialogSelectOutputFolder; //"Vyberte priečinok pre uloženie spracovaného súboru";

                // Ak už bol predtým vybraný priečinok, začneme od neho
                if (!string.IsNullOrEmpty(TextBoxSelectOutputFolder.Text) &&
                    System.IO.Directory.Exists(TextBoxSelectOutputFolder.Text))
                {
                    folderDialog.SelectedPath = TextBoxSelectOutputFolder.Text;
                }

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    TextBoxSelectOutputFolder.Text = folderDialog.SelectedPath;

                    //Saving a new directory path / Uloženie novej cesty
                    Properties.Settings.Default.LastOutputFolder = folderDialog.SelectedPath;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void ButtonSaveHistory_Click(object sender, EventArgs e)
        {
            StringBuilder content = new StringBuilder();
            try
            {
                if (ListViewShowPointsValues.Items.Count == 0)
                {
                    MessageBox.Show(Properties.Strings.MessageNoHistoryToSave,
                        Properties.Strings.MessageWarning,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = Properties.Strings.DialogFilterTextFiles;  //"Textové súbory (*.txt)|*.txt|Všetky súbory (*.*)|*.*";
                    saveDialog.FilterIndex = 1;
                    saveDialog.DefaultExt = "txt";
                    saveDialog.FileName = $"{Properties.Strings.HistoryFileName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        content.AppendLine(Properties.Strings.HistoryHeader); //("História spracovaných bodov");
                        content.AppendLine($"{Properties.Strings.HistoryCreated} {DateTime.Now:dd.MM.yyyy HH:mm:ss}"); //($"Vytvorené: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                        content.AppendLine("--------------------------------------------------");
                        content.AppendLine();

                        //Using the distinct to remove duplicates / Použite distinct na odstránenie duplicít
                        var uniqueFiles = processedFiles.GroupBy(f => f.UniqueIdentifier)
                                                        .Select(g => g.First());

                        foreach (var file in uniqueFiles)
                        {
                            content.AppendLine($"{file.FileName} " + Properties.Strings.HistoryTotalPoints + $"{file.Points:N0}");
                            content.AppendLine(file.SourceDirectory);
                            content.AppendLine("-----------------------");
                        }

                        content.AppendLine($"{Properties.Strings.HistoryDuplicates} {duplicatesRemoved}"); // počet zmazaných duplicít:
                        content.AppendLine($"{Properties.Strings.HistoryTotalPoints} {uniqueFiles.Sum(f => f.Points):N0}"); // Celkový súčet:
                        content.AppendLine($"{Properties.Strings.LabelFileCount} {uniqueFiles.Count()}");  //  Počet súborov:
                        int avgPoints = uniqueFiles.Any() ? (int)(uniqueFiles.Sum(f => f.Points) / uniqueFiles.Count()) : 0;
                        content.AppendLine($"{Properties.Strings.HistoryAveragePoints} {avgPoints:N0}");  //  Priemerné body:

                        File.WriteAllText(saveDialog.FileName, content.ToString());
                        //MessageBox.Show($"História bola úspešne uložená do súboru:\n{saveDialog.FileName}","Informácia"
                        MessageBox.Show(string.Format(Properties.Strings.MessageHistorySaved, Environment.NewLine + saveDialog.FileName),
                            Properties.Strings.MessageInfo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                //"Chyba pri ukladaní histórie: "Chyba"  - Error saving history: 
                MessageBox.Show(Properties.Strings.MessageErrorSavingHistory + ex.Message,
                    Properties.Strings.MessageError, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void UpdateStatistics()
        {
            try
            {
                // Počítame len súčet z ListView - to sú naše aktuálne spracované súbory
                int totalPoints = 0;
                int fileCount = ListViewShowPointsValues.Items.Count;

                // Prechádzame cez položky v ListView
                foreach (ListViewItem item in ListViewShowPointsValues.Items)
                {
                    string pointsText = item.SubItems[1].Text;
                    if (int.TryParse(pointsText.Replace(" ", ""), out int points))  // Odstránime všetky medzery
                    {
                        totalPoints += points;
                    }
                }

                Debug.WriteLine($"Aktualizujem štatistiky - Celkové body: {totalPoints}");
                Debug.WriteLine($"Počet súborov: {fileCount}");
                Debug.WriteLine($"Jazyk: {LanguageComboBox.SelectedItem}, Body: {totalPoints}");

                // Aktualizujeme všetky labely s rovnakými hodnotami
                string pointsFormat = totalPoints.ToString("N0");  // Formátujeme číslo len raz
              //  LabelTotalPoints.Text = Properties.Strings.LabelTotalPoints + $" {pointsFormat}";
                LabelFileCount.Text = Properties.Strings.LabelFileCount + $" {fileCount}";
                LabelTotalPoints.Text = Properties.Strings.HistoryTotalPoints + $" {pointsFormat}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Chyba pri aktualizácii štatistík: {ex.Message}");
                MessageBox.Show(Properties.Strings.MessageStatisticsError,
                    Properties.Strings.MessageError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataToGrid(string[] lines)
        {
            DataGridPreview.Rows.Clear();
            DataGridPreview.Columns.Clear();

            // 1. Nájdeme najvyšší počet oddeľovačov vo všetkých riadkoch
            int maxColumnCount = 0;
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    int separatorCount = line.Count(c => c == columnSeparator[0]);
                    maxColumnCount = Math.Max(maxColumnCount, separatorCount);
                }
            }

            // 2. Vytvoríme presný počet stĺpcov podľa najvyššieho počtu oddeľovačov
            // Pridanie checkbox stĺpca
            DataGridPreview.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "Selected",
                HeaderText = "",
                Width = 30
            });

            // Pridanie stĺpcov podľa najvyššieho počtu oddeľovačov
            for (int i = 0; i < maxColumnCount; i++)
            {
                DataGridPreview.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = $"Column{i}",
                    HeaderText = $"{i + 1}.",
                    Width = 100,
                    ReadOnly = true
                });
            }

            // 3. Plníme dáta a kontrolujeme hranice
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                int rowIndex = DataGridPreview.Rows.Add();
                var row = DataGridPreview.Rows[rowIndex];
                row.Cells[0].Value = false;  // Checkbox

                int startIndex = 0;
                int columnIndex = 0;

                // Prechádzame cez každý oddeľovač v riadku
                for (int i = 0; i < line.Length && columnIndex < maxColumnCount; i++)
                {
                    if (line[i] == columnSeparator[0])
                    {
                        if (startIndex < i)
                        {
                            string value = line.Substring(startIndex, i - startIndex);
                            row.Cells[columnIndex + 1].Value = value;
                        }
                        columnIndex++;
                        startIndex = i + 1;
                    }
                }

                // Pridanie posledného stĺpca, ak existuje
                if (startIndex < line.Length && columnIndex < maxColumnCount)
                {
                    string value = line.Substring(startIndex);
                    row.Cells[columnIndex + 1].Value = value;
                }
            }
            SetupColumnsNumberRange();

            // Inicializácia checkbox headera
            InitializeDataGridWithCheckBoxHeader();
        }

        private void CheckBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DataGridPreview.Rows)
            {
                row.Cells["Selected"].Value = CheckBoxSelectAll.Checked;
            }
        }

        private string GetUniqueFileName(string filePath, bool useBatchProcessing)
        {
            if (!File.Exists(filePath))
                return filePath;

            string folder = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            int counter = 1;

            string newFilePath;
            do
            {
                newFilePath = Path.Combine(folder, $"{fileName}({counter}){extension}");
                counter++;
            } while (File.Exists(newFilePath));

            return newFilePath;
        }

        private List<string> GetSelectedRows()
        {
            List<string> selectedRows = new List<string>();
            foreach (DataGridViewRow row in DataGridPreview.Rows)
            {
                if ((bool?)row.Cells["Selected"].Value ?? false)
                {
                    var cells = row.Cells.Cast<DataGridViewCell>()
                                   .Skip(1)  // Skip checkbox column / Preskočiť checkbox stĺpec
                                   .Select(c => (c.Value?.ToString() ?? "") + columnSeparator); // We add a separator to each value / Pridáme oddeľovač ku každej hodnote

                    selectedRows.Add(string.Join("", cells)); // no column separator, because it is already part of each column / Žiadny oddeľovač stĺpcov, pretože je už súčasťou každého stĺpca
                }
            }
            return selectedRows;
        }

        private async Task<string> SaveProcessedDataAsync(List<string> processedLines, string filePath)
        {
            // Získaj hodnotu z UI kontrolky na hlavnom vlákne
            string outputSeparator = ComboBoxOutputSeparatorType.SelectedItem?.ToString() ?? columnSeparator;

            // Získaj hodnotu z TextBoxu na hlavnom vlákne
            string outputFolderPath = TextBoxSelectOutputFolder.Text;

            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    string outputPath;
                    string originalDirectory = Path.GetDirectoryName(filePath);
                    string originalFileName = Path.GetFileName(filePath);

                    Debug.WriteLine($"Pôvodný adresár: {originalDirectory}");
                    Debug.WriteLine($"Pôvodný súbor: {originalFileName}");

                    // Kontrola výstupného adresára (bez prístupu k UI kontrolke)
                    if (!string.IsNullOrEmpty(outputFolderPath) &&
                        Directory.Exists(outputFolderPath) &&
                        HasWriteAccessToFolder(outputFolderPath))
                    {
                        outputPath = Path.Combine(
                            outputFolderPath,
                            PROCESSED_PREFIX + originalFileName
                        );
                        Debug.WriteLine($"Použitý výstupný adresár: {outputFolderPath}");
                    }
                    else
                    {
                        outputPath = Path.Combine(
                            originalDirectory,
                            PROCESSED_PREFIX + originalFileName
                        );
                        Debug.WriteLine("Použitý pôvodný adresár");
                    }

                    // Prevod riadkov s pôvodným oddeľovačom na riadky s novým oddeľovačom (len raz)
                    if (outputSeparator != columnSeparator)
                    {
                        processedLines = processedLines.Select(line =>
                            string.Join(outputSeparator,
                                       line.Split(new[] { columnSeparator }, StringSplitOptions.None)))
                                       .ToList();
                    }

                    // Pre dávkové spracovanie použijeme timestamp, aby sme mali unikátne súbory
                    bool useBatchProcessing = selectedFilePaths.Count > 1;
                    string uniqueOutputPath = GetUniqueFileName(outputPath, useBatchProcessing);

                    Debug.WriteLine($"Unikátna výstupná cesta: {uniqueOutputPath}");

                    string targetDirectory = Path.GetDirectoryName(uniqueOutputPath);
                    if (HasWriteAccessToFolder(targetDirectory))
                    {
                        File.WriteAllLines(uniqueOutputPath, processedLines);
                        Debug.WriteLine("Súbor úspešne uložený");
                        return uniqueOutputPath;
                    }
                    else
                    {
                        throw new UnauthorizedAccessException($"Nemáte prístup pre zápis do priečinka: {targetDirectory}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Chyba pri ukladaní súboru: {ex.Message}");
                    throw;
                }
            });
        }

        private void ComboBoxSeparatorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnSeparator = ComboBoxSeparatorType.SelectedItem.ToString();

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

        private void ComboBoxSeparatorType_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ComboBox combo = sender as ComboBox;
            string text = combo.Items[e.Index].ToString();

            // Získanie stredu pre text
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            // Vykreslenie pozadia
            e.DrawBackground();

            // Vykreslenie textu v strede
            using (Brush brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(text, e.Font, brush, e.Bounds, sf);
            }

            // Ak je položka vybraná, vykresliť focus rectangle
            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
            {
                e.DrawFocusRectangle();
            }
        }

        private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedLanguage = LanguageComboBox.SelectedItem.ToString();
                LanguageManager.ChangeLanguage(selectedLanguage);
                UpdateFormText();
            }
            catch (Exception ex)
            {
                //"Chyba pri zmene jazyka: {ex.Message}"
                MessageBox.Show(string.Format(Properties.Strings.MessageLanguageChangeError, ex.Message),
                    Properties.Strings.MessageError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateFormText()
        {
            ButtonSelectAFile.Text = Properties.Strings.ButtonSelectFile;
            ButtonProcessData.Text = Properties.Strings.ButtonProcessData;
            ButtonSelectOutputFolder.Text = Properties.Strings.ButtonSelectOutputFolder;
            ButtonSaveHistory.Text = Properties.Strings.ButtonSaveHistory;

            CheckBoxRenumberTheOrder.Text = Properties.Strings.CheckBoxRenumberTheOrder;
            CheckBoxSortByName.Text = Properties.Strings.CheckBoxSortByName;
            CheckBoxRemoveDuplicatesRows.Text = Properties.Strings.CheckBoxRemoveDuplicatesRows;
            CheckBoxSelectAll.Text = Properties.Strings.CheckBoxSelectAll;

            LabelDataStructureSeparatorIs.Text = Properties.Strings.LabelDataStructureSeparatorIs;
            LabelPointsColumn.Text = Properties.Strings.LabelPointsColumn ?? "Stĺpec s bodmi (číslo):";

            // Získame aktuálne hodnoty
            int totalPoints = CalculateTotalPointsFromListView();
            int fileCount = ListViewShowPointsValues.Items.Count;

            // Aktualizujeme labely s hodnotami
           // LabelTotalPoints.Text = Properties.Strings.LabelTotalPoints + $" {totalPoints:N0}";
            LabelFileCount.Text = Properties.Strings.LabelFileCount + $" {fileCount}";
            LabelTotalPoints.Text = Properties.Strings.HistoryTotalPoints + $" {totalPoints:N0}";

            // ToolTip pri zmene jazyka
            ColumnToCountToolTip.SetToolTip(LabelPointsColumn, Properties.Strings.ColumnToCountToolTip);
          //ColumnToCountToolTip.ToolTipTitle = Properties.Strings.ColumnToCountToolTip;
            ColumnToCountToolTip.SetToolTip(NumericUpDownPointsColumn, Properties.Strings.ColumnToCountToolTip);

            UpdateListViewColumns();
        }

        private void UpdateListViewColumns()
        {
            if (ListViewShowPointsValues.Columns.Count >= 3)
            {
                ListViewShowPointsValues.Columns[0].Text = Properties.Strings.ListViewColumnFile;
                ListViewShowPointsValues.Columns[1].Text = Properties.Strings.ListViewColumnPoints;
                ListViewShowPointsValues.Columns[2].Text = Properties.Strings.ListViewColumnDate;
            }
        }

        private int CalculateTotalPointsFromListView()
        {
            int totalPoints = 0;
            foreach (ListViewItem item in ListViewShowPointsValues.Items)
            {
                // Očistíme text od medzier a získame čistú hodnotu
                string pointsText = new string(item.SubItems[1].Text.Where(c => !char.IsWhiteSpace(c)).ToArray());
                if (int.TryParse(pointsText, out int points))
                {
                    totalPoints += points;
                }
            }
            return totalPoints;
        }

        private void DelayedUpdateStatistics()
        {
            // Vytvoríme timer, ktorý sa spustí len raz
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer
            {
                Interval = 100 // 100 milisekúnd
            };
            timer.Tick += (sender, e) =>
            {
                UpdateStatistics();
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        // Pridajte túto metódu na overenie, či vybraný stĺpec existuje v údajoch
        private bool ValidatePointsColumn()
        {
            // Ak nie sú načítané žiadne údaje, preskočte validáciu
            if (DataGridPreview.Columns.Count <= 1) // Iba stĺpec s checkboxom
                return true;

            // Skontrolujte, či vybraný stĺpec existuje v dátovej mriežke
            // +1 pretože prvý stĺpec je checkbox
            int columnIndex = (int)NumericUpDownPointsColumn.Value;
            bool isValid = columnIndex < DataGridPreview.Columns.Count;

            // Vizuálna spätná väzba - zmena farby na základe platnosti
            NumericUpDownPointsColumn.BackColor = isValid ? System.Drawing.SystemColors.Window : System.Drawing.Color.LightPink;

            return isValid;
        }

        // Pridajte metódu, ktorá sa zavolá pri načítaní súboru na nastavenie vhodných minimálnych a maximálnych hodnôt
        private void SetupColumnsNumberRange()
        {
            if (DataGridPreview.Columns.Count <= 1)
            {
                // Nie sú načítané žiadne údaje
                NumericUpDownPointsColumn.Minimum = 1;
                NumericUpDownPointsColumn.Maximum = 50; // Nejaká rozumná predvolená hodnota
                return;
            }

            // DataGridPreview.Columns.Count - 1 pretože prvý stĺpec je checkbox
            NumericUpDownPointsColumn.Minimum = 1;
            NumericUpDownPointsColumn.Maximum = DataGridPreview.Columns.Count - 1;

            ValidatePointsColumn();
        }
    }
}
