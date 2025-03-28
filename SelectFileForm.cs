﻿using System;
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
using CountAndSortWinFormsAppNetFr4.Properties;
using System.Windows.Forms.DataVisualization.Charting;
using System.Security.Cryptography.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

// Doplnenie referencií pre grafy
// Aby grafy fungovali, je potrebné pridať referenciu na assembly System.Windows.Forms.DataVisualization v projekte.
// V súbore CountAndSortWinFormsAppNetFr4.csproj je potrebné pridať:
//   <Reference Include="System.Windows.Forms.DataVisualization" />     

namespace CountAndSortWinFormsAppNetFr4
{
    public partial class SelectFileForm : Form
    {
        // Indexy stĺpcov pre základné dáta
        private int _pointsColumnIndex;
        private int _nameColumnIndex;
        private int _idColumnIndex;
        private int _dayColumnIndex;
        private int _serviceCodeColumnIndex;
        private int _diagnosisColumnIndex;
        private int _totalLinesColumnIndex;

        // Oddelenie stĺpcov
        private string _importSeparator = "|";
        private string _exportSeparator = "|";

        // Konštanty pre štruktúru dávkového súboru
        private const int MIN_COLUMNS_REQUIRED = 6;    // Minimálny počet stĺpcov
        //public string columnSeparator = string.Empty;  // Oddeľovač stĺpcov

        // Konštanty pre formátovanie výstupu
        private const string DATE_TIME_FORMAT = "dd.MM.yyyy HH:mm:ss";
        private const string NUMBER_FORMAT = "N0";
        private const string PROCESSED_PREFIX = "processed_";

        // Konštanty pre ListView
        private const int COLUMN_WIDTH_FILENAME = 220;
        private const int COLUMN_WIDTH_POINTS = 100;
        private const int COLUMN_WIDTH_DATE = 220;

        public List<string> selectedFilePaths = new List<string>(); // Path to the selected files / Cesta k vybraným súborom
                                                                    //public int ColumnPointsIndex => (int)NumericUpDownPointsColumn.Value - 1;  // Prevod z 1-indexovaného UI na 0-indexovaný index
                                                                    //public int ColumnNameIndex => (int)NumericUpDownNameColumn.Value - 1;
                                                                    //public int ColumnIdIndex => (int)NumericUpDownIdColumn.Value - 1;
                                                                    //public int ColumnDayIndex => (int)NumericUpDownDayColumn.Value - 1;
                                                                    //public int ColumnServiceCodeIndex => (int)NumericUpDownServiceCodeColumn.Value - 1;
                                                                    //public int ColumnDiagnosisIndex => (int)NumericUpDownDiagnosisColumn.Value - 1;

        // vlastnosti, ktoré používajú privátne premenné
        public int ColumnPointsIndex => _pointsColumnIndex;
        public int ColumnNameIndex => _nameColumnIndex;
        public int ColumnIdIndex => _idColumnIndex;
        public int ColumnDayIndex => _dayColumnIndex;
        public int ColumnServiceCodeIndex => _serviceCodeColumnIndex;
        public int ColumnDiagnosisIndex => _diagnosisColumnIndex;
        public int ColumnTotalLinesIndex => _totalLinesColumnIndex;
        public string ColumnSeparator
        {
            get { return _importSeparator; }
            set { _importSeparator = value; }
        }
        public int duplicatesRemoved = 0; // Pre výpis počtu duplicít

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

            this.Load += SelectFileForm_Load;
            this.VisibleChanged += SelectFileForm_VisibleChanged;

            // Načítanie uložených nastavení
            _pointsColumnIndex = Settings.Default.PointsColumnIndex - 1;
            _nameColumnIndex = Settings.Default.NameColumnIndex - 1;
            _idColumnIndex = Settings.Default.IdColumnIndex - 1;
            _serviceCodeColumnIndex = Settings.Default.ServiceCodeColumnIndex - 1;
            _dayColumnIndex = Settings.Default.DayColumnIndex - 1;
            _diagnosisColumnIndex = Settings.Default.DiagnosisColumnIndex - 1;
            _importSeparator = "|"; // Predvolená hodnota
            _exportSeparator = "|"; // Predvolená hodnota

        // slovak as default language / slovenčina ako predvolený jazyk
        // Nastavenie predvoleného jazyka // Default language + DropDownStyle
            LanguageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            LanguageComboBox.SelectedIndex = 1; // Index pre "Slovensky"

            // Nastavenie ToolTipu
            LanguageToolTip.SetToolTip(LanguageComboBox,
                "Wybór języka / Nyelvválasztás / Вибір мови");
            LanguageToolTip.SetToolTip(LabelLanguagesChoice,
                "Wybór języka / Nyelvválasztás / Вибір мови");

            // Získanie názvu a verzie aplikácie z Assembly
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version version = assembly.GetName().Version;
            AssemblyTitleAttribute titleAttribute =
                (AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute));
            string title = titleAttribute?.Title ?? "CountAndSort";

            this.Text = $"{title} v{version.Major}.{version.Minor}.{version.Build}";

            // Načítanie poslednej použitej cesty
            if (!string.IsNullOrEmpty(Settings.Default.LastOutputFolder))
            {
                TextBoxSelectOutputFolder.Text = Settings.Default.LastOutputFolder;
            }

            // Inicializácia stĺpcov pre ListView -
            // - bez priameho nastavenia textov stĺpcov lebo lokalizačné zdroje sa nemusia načítať ako prvé a hodnoty jazykov sa neaktualizujú.
            ListViewShowPointsValues.Columns.Clear();
            ListViewShowPointsValues.Columns.Add("", COLUMN_WIDTH_FILENAME);
            ListViewShowPointsValues.Columns.Add("", COLUMN_WIDTH_POINTS);
            ListViewShowPointsValues.Columns.Add("", COLUMN_WIDTH_DATE);

            UpdateFormText(); // správne texty stĺpcov podľa aktuálneho jazyka

            // Nastavenie alternujúcich farieb riadkov
            DataGridPreview.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            DataGridPreview.RowsDefaultCellStyle.BackColor = Color.White;
            DataGridPreview.BackgroundColor = Color.White;

            // Voliteľné vylepšenia pre lepší vzhľad
            DataGridPreview.GridColor = Color.LightGray;
            DataGridPreview.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            DataGridPreview.BorderStyle = BorderStyle.Fixed3D;
        }
        
        private void SelectFileForm_Load(object sender, EventArgs e)
        {
            // Zabezpečiť, že texty stĺpcov sú správne nastavené podľa aktuálneho jazyka
            UpdateListViewColumns();

            // Možno budete chcieť zavolať aj ďalšie metódy aktualizácie UI
            UpdateFormText();
            UpdateStatistics();
        }

        private void SelectFileForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                UpdateListViewColumns();
            }
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
            Settings.Default.PointsColumnIndex = _pointsColumnIndex + 1;
            Settings.Default.NameColumnIndex = _nameColumnIndex + 1;
            Settings.Default.IdColumnIndex = _idColumnIndex + 1;
            Settings.Default.ServiceCodeColumnIndex = _serviceCodeColumnIndex + 1;
            Settings.Default.DayColumnIndex = _dayColumnIndex + 1;
            Settings.Default.DiagnosisColumnIndex = _diagnosisColumnIndex + 1;
            Settings.Default.Save();
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

            // Vytvorenie inštancie s rozšírenými vlastnosťami
            var newFileInfo = new ProcessedFileInfo(fileName, fullPath, now, points, sourceDirectory);

            // Analýza obsahu súboru pre rozšírené štatistiky
            newFileInfo.AnalyzeContent(filePath,
                _pointsColumnIndex,
                _diagnosisColumnIndex,
                _serviceCodeColumnIndex,
                19, // Stĺpec pre lekára (z vášho formulára nastavení)
                18, // Stĺpec pre zariadenie
                33  // Stĺpec pre cenu za bod
            );

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
                        catch (Exception)
                        {
                            MessageBox.Show(Strings.MessageFileError,
                                Strings.MessageError,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                filesList.AppendLine($"- {Strings.AndOther} {selectedFilePaths.Count - 5} {Strings.AFiles}");  //... a ďalších -- súborov
            }

            MessageBox.Show(
                $"{Strings.Selected} {selectedFilePaths.Count} {Strings.AFiles}.\n\n" +
                $"{Strings.MessageShowsPreviewAllFilesProcessed}\n\n" + // V tabuľke je zobrazený náhľad prvého súboru. Všetky súbory budú spracované po kliknutí na 'Spracovať údaje'.
                $"{Strings.FileList} \n{filesList}", //$"Zoznam súborov:\n{filesList}",
                Strings.MessageInfoFileSelection, // "Informácia o výbere súborov",
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
                    MessageBox.Show(Strings.MessageFileNotExist,
                        Strings.MessageError,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    results.Add(new FileProcessingResult
                    {
                        FileName = Path.GetFileName(filePath),
                        Success = false,
                        ErrorMessage = Strings.MessageFileNotExist
                    });

                    return results;
                }

                string currentFile = Path.GetFileName(filePath);
                string sourceDirectory = Path.GetDirectoryName(filePath);

                if (IsFileAlreadyProcessed(filePath))
                {
                    var dialogResult = MessageBox.Show(
                        Strings.MessageFileAlreadyProcessed,
                        Strings.MessageWarning,
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
                    MessageBox.Show(Strings.MessageNoRowsSelected,
                        Strings.MessageWarning,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    results.Add(new FileProcessingResult
                    {
                        FileName = currentFile,
                        Success = false,
                        ErrorMessage = Strings.MessageNoRowsSelected
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
            using (var progressForm = new ProgressForm(Strings.FileProcessing)) //("Spracovanie súborov"))
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
                            ErrorMessage = Strings.MessageFileNotExist
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
                                ErrorMessage = Strings.MessageNoRowsSelected
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
                .Select(line => line.Split(new[] { ColumnSeparator }, StringSplitOptions.None))
                .Where(parts => parts.Length > 4 && int.TryParse(parts[0], out _))
                .OrderBy(parts => parts[ColumnNameIndex])      // Použite vlastnosť namiesto ovládacieho prvku
                .ThenBy(parts => int.Parse(parts[ColumnIdIndex])) // Použite vlastnosť namiesto ovládacieho prvku
                .Select(parts => string.Join(ColumnSeparator, parts))
                .ToList();
        }

        private List<string> RenumberFirstColumn(List<string> lines)
        {
            int newNumber = 1;
            return lines
                .Select(line =>
                {
                    var parts = line.Split(new[] { ColumnSeparator }, StringSplitOptions.None);
                    if (parts.Length > 4 && int.TryParse(parts[0], out _))
                    {
                        parts[0] = newNumber++.ToString();
                        return string.Join(ColumnSeparator, parts);
                    }
                    return line;
                })
                .ToList();
        }

        private List<string> RemoveDuplicateRows(List<string> lines)
        {
            // Odfiltrujeme hlavičku a informácie o dávke (prvé dva riadky)
            var headerLines = lines.Take(2).ToList();
            var dataLines = lines.Skip(2).ToList();

            // Výsledný zoznam začneme hlavičkou
            var result = new List<string>(headerLines);

            // Aktualizujeme hodnotu počtu záznamov v hlavičke, ak existuje
            if (headerLines.Count > 0)
            {
                string headerLine = headerLines[0];
                string[] headerParts = headerLine.Split(new[] { ColumnSeparator }, StringSplitOptions.None);

                if (headerParts.Length > 4 && int.TryParse(headerParts[4], out int recordCount))
                {
                    // Odstránené duplicity počítame neskôr
                    int originalRecordCount = recordCount;
                    int duplicatesCount = 0;

                    // Spracované záznamy
                    var processedRecords = dataLines
                        .Select(line => line.Split(new[] { ColumnSeparator }, StringSplitOptions.None))
                        .Where(parts => parts.Length > MIN_COLUMNS_REQUIRED)
                        .ToList();

                    // Identifikácia a odstránenie duplicít
                    var uniqueRecords = new Dictionary<string, string[]>();

                    foreach (var parts in processedRecords)
                    {
                        // Vytvorenie kľúča z kombinácie dňa (index 1), rodného čísla (index 2) a kódu výkonu (nastaviteľný index)
                        // Zabezpečíme, aby indexy boli v rozsahu poľa
                        string day = parts.Length > 1 ? parts[1] : "";
                        string personalId = parts.Length > 2 ? parts[2] : "";

                        // Použijeme nastaviteľný index pre kód výkonu
                        string serviceCode = "";
                        if (parts.Length > ColumnServiceCodeIndex && ColumnServiceCodeIndex >= 0)
                        {
                            serviceCode = parts[ColumnServiceCodeIndex];
                        }

                        string uniqueKey = $"{day}|{personalId}|{serviceCode}";

                        if (!uniqueRecords.ContainsKey(uniqueKey))
                        {
                            uniqueRecords[uniqueKey] = parts;
                        }
                        else
                        {
                            duplicatesCount++;
                        }
                    }

                    // Aktualizácia počtu záznamov v hlavičke a uloženie počtu odstránených duplicít
                    int newRecordCount = originalRecordCount - duplicatesCount;
                    if (newRecordCount < 0) newRecordCount = 0; // Zabraňujeme záporným hodnotám

                    headerParts[4] = newRecordCount.ToString();
                    result[0] = string.Join(ColumnSeparator, headerParts);

                    // Uložíme počet odstránených duplicít pre informáciu používateľovi
                    duplicatesRemoved = duplicatesCount;

                    // Zoraďujeme a prečíslujeme unikátne záznamy
                    var sortedRecords = uniqueRecords.Values
                        .OrderBy(parts => parts.Length > ColumnNameIndex && ColumnNameIndex >= 0 ? parts[ColumnNameIndex] : "")
                        .ToList();

                    // Prečíslovanie a pridanie záznamov do výsledku
                    for (int i = 0; i < sortedRecords.Count; i++)
                    {
                        sortedRecords[i][0] = (i + 1).ToString();
                        result.Add(string.Join(ColumnSeparator, sortedRecords[i]));
                    }

                    return result;
                }
            }

            // Ak nemáme hlavičku alebo je neplatná, použijeme pôvodný spôsob
            var uniqueLines = dataLines
                .Select(line => line.Split(new[] { ColumnSeparator }, StringSplitOptions.None))
                .Where(parts => parts.Length > MIN_COLUMNS_REQUIRED && int.TryParse(parts[0], out _))
                .GroupBy(parts =>
                {
                    string day = parts.Length > 1 ? parts[1] : "";
                    string personalId = parts.Length > 2 ? parts[2] : "";

                    // Použijeme nastaviteľný index pre kód výkonu
                    string serviceCode = "";
                    if (parts.Length > ColumnServiceCodeIndex && ColumnServiceCodeIndex >= 0)
                    {
                        serviceCode = parts[ColumnServiceCodeIndex];
                    }

                    return $"{day}|{personalId}|{serviceCode}";
                })
                .Select(group => group.First())  // Vyberieme prvý záznam z každej skupiny
                .OrderBy(parts => parts.Length > ColumnNameIndex && ColumnNameIndex >= 0 ? parts[ColumnNameIndex] : "")
                .Select((parts, index) => $"{index + 1}{ColumnSeparator}{string.Join(ColumnSeparator, parts.Skip(1))}")
                .ToList();

            // Uložíme počet odstránených duplicít
            duplicatesRemoved = dataLines.Count - uniqueLines.Count;

            // Pridáme spracované riadky k hlavičke
            result.AddRange(uniqueLines);

            return result;
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
                    MessageBox.Show(Strings.MessageSelectFile,
                        Strings.MessageWarning,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidatePointsColumn())
                {
                    MessageBox.Show(Strings.MessageInvalidColumn ??
                        "Vybraný stĺpec pre body neexistuje v súbore. Prosím vyberte platný stĺpec.",
                        Strings.MessageWarning,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Uloženie nastavení stĺpcov
                SaveColumnSettings();

                // Vytvorenie inštancie BatchFileProcessor s nastaveniami z formulára
                var processor = new BatchFileProcessor(
                    separator: ColumnSeparator,
                    dayColIndex: ColumnDayIndex,
                    idColIndex: ColumnIdIndex,
                    serviceCodeColIndex: ColumnServiceCodeIndex,
                    nameColIndex: ColumnNameIndex,
                    pointsColIndex: ColumnPointsIndex,
                    totalLinesColIndex: ColumnTotalLinesIndex,
                    sortByName: CheckBoxSortByName.Checked,
                    renumberRows: CheckBoxRenumberTheOrder.Checked,
                    removeDuplicates: CheckBoxRemoveDuplicatesRows.Checked
                );

                // Spracovanie súborov a získanie výsledkov
                List<ProcessingResult> results = new List<ProcessingResult>();

                // Ak je vybraný len jeden súbor a sú označené konkrétne riadky v DataGridView,
                // mohli by sme ešte implementovať špeciálne spracovanie vybraných riadkov

                // Vytvorenie a zobrazenie progress formulára pri viacerých súboroch
                if (selectedFilePaths.Count > 1)
                {
                    using (var progressForm = new ProgressForm(Strings.FileProcessing))
                    {
                        progressForm.Show(this);

                        // Spracovanie každého súboru
                        for (int i = 0; i < selectedFilePaths.Count; i++)
                        {
                            string currentFilePath = selectedFilePaths[i];
                            string currentFile = Path.GetFileName(currentFilePath);
                            string sourceDirectory = Path.GetDirectoryName(currentFilePath);

                            // Aktualizácia progress baru
                            progressForm.UpdateProgress(i + 1, selectedFilePaths.Count, currentFile);

                            // Kontrola existencie súboru
                            if (!File.Exists(currentFilePath))
                            {
                                results.Add(new ProcessingResult
                                {
                                    FileName = currentFile,
                                    Success = false,
                                    ErrorMessage = Strings.MessageFileNotExist
                                });
                                continue;
                            }

                            // Kontrola, či súbor už bol spracovaný
                            if (IsFileAlreadyProcessed(currentFilePath))
                            {
                                var dialogResult = MessageBox.Show(
                                    Strings.MessageFileAlreadyProcessed,
                                    Strings.MessageWarning,
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                                if (dialogResult == DialogResult.No)
                                {
                                    results.Add(new ProcessingResult
                                    {
                                        FileName = currentFile,
                                        Success = false,
                                        ErrorMessage = "Užívateľ zrušil spracovanie už existujúceho súboru"
                                    });
                                    continue;
                                }
                            }

                            // Vytvorenie výstupnej cesty
                            string outputFolder = !string.IsNullOrEmpty(TextBoxSelectOutputFolder.Text) &&
                                                Directory.Exists(TextBoxSelectOutputFolder.Text)
                                                ? TextBoxSelectOutputFolder.Text
                                                : Path.GetDirectoryName(currentFilePath);

                            string outputPath = Path.Combine(
                                outputFolder,
                                "processed_" + Path.GetFileName(currentFilePath)
                            );

                            // Asynchrónne spracovanie súboru
                            var result = await processor.ProcessBatchFileAsync(currentFilePath, outputPath);
                            results.Add(result);

                            // Pridanie do histórie pri úspešnom spracovaní
                            if (result.Success)
                            {
                                AddToHistory(currentFilePath, result.ProcessedPointsCount, sourceDirectory);
                            }
                        }
                    }
                }
                else if (selectedFilePaths.Count == 1)
                {
                    // Spracovanie jedného súboru
                    string filePath = selectedFilePaths[0];
                    string sourceDirectory = Path.GetDirectoryName(filePath);

                    // Kontrola, či súbor už bol spracovaný
                    if (IsFileAlreadyProcessed(filePath))
                    {
                        var dialogResult = MessageBox.Show(
                            Strings.MessageFileAlreadyProcessed,
                            Strings.MessageWarning,
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (dialogResult == DialogResult.No)
                        {
                            results.Add(new ProcessingResult
                            {
                                FileName = Path.GetFileName(filePath),
                                Success = false,
                                ErrorMessage = "Užívateľ zrušil spracovanie už existujúceho súboru"
                            });
                            return;
                        }
                    }

                    // Vytvorenie výstupnej cesty
                    string outputFolder = !string.IsNullOrEmpty(TextBoxSelectOutputFolder.Text) &&
                                        Directory.Exists(TextBoxSelectOutputFolder.Text)
                                        ? TextBoxSelectOutputFolder.Text
                                        : Path.GetDirectoryName(filePath);

                    string outputPath = Path.Combine(
                        outputFolder,
                        "processed_" + Path.GetFileName(filePath)
                    );

                    // Asynchrónne spracovanie súboru
                    var result = await processor.ProcessBatchFileAsync(filePath, outputPath);
                    results.Add(result);

                    // Pridanie do histórie pri úspešnom spracovaní
                    if (result.Success)
                    {
                        AddToHistory(filePath, result.ProcessedPointsCount, sourceDirectory);
                    }
                }

                // Zobrazenie výsledkov spracovania
                ShowProcessingResults(
                    results,
                    results.Count(r => r.Success),
                    results.Sum(r => r.OriginalPointsCount),
                    results.Sum(r => r.ProcessedPointsCount)
                );

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
        /// <summary>
        /// Zobrazenie súhrnných výsledkov spracovania
        /// </summary>
        /// <param name="results">- zoznam výsledkov spracovania súborov</param>
        /// <param name="totalFilesProcessed">- celkový počet spracovaných súborov</param>
        /// <param name="totalOriginalPoints">- celkový počet bodov pred spracovaním</param>
        /// <param name="totalProcessedPoints">- celkový počet bodov po spracovaní</param>      
        private void ShowProcessingResults(List<ProcessingResult> results, int totalFilesProcessed,
    int totalOriginalPoints, int totalProcessedPoints)
        {
            if (results is null || results.Count == 0)
            {
                return;
            }

            // Ak bol spracovaný len jeden súbor, použije pôvodnú správu
            if (results.Count == 1 && results[0].Success)
            {
                var result = results[0];


                MessageBox.Show(
                    string.Format(Strings.MessageProcessingResults,
                        result.OutputFilePath,
                        result.GetRemovedRowsCount().ToString("N0"), // Formátované odstránené riadky
                        result.OriginalRecordCount.ToString("N0"),   // Formátované pôvodné riadky
                        result.ProcessedRecordCount.ToString("N0"),  // Formátované spracované riadky
                        result.OriginalPointsCount.ToString("N0"),   // Formátované pôvodné body
                        result.ProcessedPointsCount.ToString("N0")), // Formátované spracované body
                    Strings.MessageDone,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // Pre viac súborov súhrnna správa
            StringBuilder message = new StringBuilder();
            message.AppendLine(Strings.ProcessingComplete);
            message.AppendLine($"------------------------------------------");
            message.AppendLine(string.Format(Strings.ProcessedFilesCount, totalFilesProcessed, results.Count));
            message.AppendLine(string.Format(Strings.TotalPointsBefore, totalOriginalPoints.ToString("N0")));
            message.AppendLine(string.Format(Strings.TotalPointsAfter, totalProcessedPoints.ToString("N0")));
            message.AppendLine(string.Format(Strings.PointsDifference, (totalProcessedPoints - totalOriginalPoints).ToString("N0")));
            message.AppendLine($"------------------------------------------");

            // Zoznam súborov podľa statusu
            int maxFilesToShow = 10;

            // Úspešne spracované súbory
            var successfulFiles = results.Where(r => r.Success).ToList();
            if (successfulFiles.Any())
            {
                message.AppendLine();
                message.AppendLine(Strings.SuccessfulFilesHeader);

                for (int i = 0; i < Math.Min(successfulFiles.Count, maxFilesToShow); i++)
                {
                    var result = successfulFiles[i];
                    message.AppendLine($"- {result.FileName}: {result.ProcessedPointsCount:N0} {Strings.PointsRemoved} {result.OriginalRecordCount - result.ProcessedRecordCount} {Strings.Lines}");
                }

                if (successfulFiles.Count > maxFilesToShow)
                {
                    message.AppendLine($"{Strings.AndOther} {successfulFiles.Count - maxFilesToShow} {Strings.AFiles}");
                }
            }

            // Neúspešne spracované súbory
            var failedFiles = results.Where(r => !r.Success).ToList();
            if (failedFiles.Any())
            {
                message.AppendLine();
                message.AppendLine(Strings.FailedFilesHeader);

                for (int i = 0; i < Math.Min(failedFiles.Count, maxFilesToShow); i++)
                {
                    var result = failedFiles[i];
                    message.AppendLine($"- {result.FileName}: {result.ErrorMessage}");
                }

                if (failedFiles.Count > maxFilesToShow)
                {
                    message.AppendLine($"{Strings.AndOther} {failedFiles.Count - maxFilesToShow} {Strings.AFiles}");
                }
            }

            // Konverzia ProcessingResult na FileProcessingResult
            var fileResults = results.Select(r => new FileProcessingResult
            {
                FileName = r.FileName,
                Success = r.Success,
                OutputPath = r.OutputFilePath,
                RemovedRows = r.OriginalRecordCount - r.ProcessedRecordCount,
                OriginalRows = r.OriginalRecordCount,
                ProcessedRows = r.ProcessedRecordCount,
                OriginalPoints = r.OriginalPointsCount,
                ProcessedPoints = r.ProcessedPointsCount,
                ErrorMessage = r.ErrorMessage
            }).ToList();

            // Asking the user if they want to save the results to a file
            var dialogResult = MessageBox.Show(
                message.ToString() + $"\n\n {Strings.MessageDoYouWantSaveDetailedResult}",
                Strings.MessageDone,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                SaveProcessingResults(fileResults, totalFilesProcessed, totalOriginalPoints, totalProcessedPoints);
            }
        }

        private void SaveProcessingResults(List<FileProcessingResult> results, int totalFilesProcessed,
    int totalOriginalPoints, int totalProcessedPoints)
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = Strings.DialogFilterTextFiles;
                    saveDialog.FilterIndex = 1;
                    saveDialog.DefaultExt = "txt";
                    saveDialog.FileName = $"BatchProcessingResults_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder content = new StringBuilder();
                        content.AppendLine(Strings.BatchProcessingResultsTitle);
                        content.AppendLine($"{Strings.DateTimeLabel} {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                        content.AppendLine("------------------------------------------");
                        content.AppendLine(string.Format(Strings.ProcessedFilesCount,
                            totalFilesProcessed, results.Count));
                        content.AppendLine(string.Format(Strings.TotalPointsBefore,
                            totalOriginalPoints.ToString("N0")));
                        content.AppendLine(string.Format(Strings.TotalPointsAfter,
                            totalProcessedPoints.ToString("N0")));
                        content.AppendLine(string.Format(Strings.PointsDifference,
                            (totalProcessedPoints - totalOriginalPoints).ToString("N0")));
                        content.AppendLine("------------------------------------------");

                        // Podrobný výpis pre všetky súbory
                        content.AppendLine("\n" + Strings.DetailedResultsHeader);

                        // Úspešne spracované súbory
                        var successfulFiles = results.Where(r => r.Success).ToList();
                        if (successfulFiles.Any())
                        {
                            content.AppendLine("\n" + Strings.SuccessfulFilesHeader);
                            foreach (var result in successfulFiles)
                            {
                                content.AppendLine($"\n{result.FileName}");
                                content.AppendLine($"- {Strings.OutputFileLabel} {result.OutputPath}");
                                content.AppendLine($"- {Strings.PointsBeforeLabel} {result.OriginalPoints:N0}");
                                content.AppendLine($"- {Strings.PointsAfterLabel} {result.ProcessedPoints:N0}");
                                content.AppendLine($"- {Strings.PointsDifferenceLabel} {(result.ProcessedPoints - result.OriginalPoints):N0}");
                                content.AppendLine($"- {Strings.RowsBeforeLabel} {result.OriginalRows}");
                                content.AppendLine($"- {Strings.RowsAfterLabel} {result.ProcessedRows}");
                                content.AppendLine($"- {Strings.RemovedDuplicatesLabel} {result.RemovedRows}");
                            }
                        }

                        // Neúspešne spracované súbory
                        var failedFiles = results.Where(r => !r.Success).ToList();
                        if (failedFiles.Any())
                        {
                            content.AppendLine("\n" + Strings.FailedFilesHeader);
                            foreach (var result in failedFiles)
                            {
                                content.AppendLine($"\n{result.FileName}");
                                content.AppendLine($"- {Strings.FailureReasonLabel} {result.ErrorMessage}");
                            }
                        }

                        File.WriteAllText(saveDialog.FileName, content.ToString());
                        MessageBox.Show(
                            string.Format(Strings.ResultsSavedSuccessfully, saveDialog.FileName),
                            Strings.MessageInfo,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(Strings.ErrorSavingResults, ex.Message),
                    Strings.MessageError,
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
                var result = MessageBox.Show(Strings.MessageFileAlreadyProcessed,
                        Strings.MessageWarning,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return result == DialogResult.Yes;
            }

            // Pre viac súborov zostavíme zoznam a spýtame sa raz
            var message = new StringBuilder();
            message.AppendLine($"{alreadyProcessedFiles.Count} {Strings.From} {filePaths.Count} {Strings.MessageFilesAlreadyBeenProcessed}");// vybraných súborov už bolo spracovaných:

            // Zobraziť prvých 5 súborov v správe
            int showCount = Math.Min(alreadyProcessedFiles.Count, 5);
            for (int i = 0; i < showCount; i++)
            {
                message.AppendLine($"- {alreadyProcessedFiles[i]}");
            }

            // Ak je viac ako 5 súborov, pridáme poznámku
            if (alreadyProcessedFiles.Count > 5)
            {
                message.AppendLine($"{Strings.AndOther } {alreadyProcessedFiles.Count - 5} {Strings.AFiles}"); //- ... a ďalších   súborov
            }

            
            message.AppendLine($"\n{Strings.MessageDoYouWantProcessAgain}");//("\nChcete tieto súbory spracovať znova?");

            var dialogResult = MessageBox.Show(
                message.ToString(),
                Strings.MessageWarning,
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
                    unselectedRows.Add(string.Join(ColumnSeparator, cells));
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

                foreach (var line in lines)
                {
                    lineCount++;
                    var parts = line.Split(new[] { _importSeparator }, StringSplitOptions.None);

                    if (parts.Length <= _pointsColumnIndex)
                    {
                        Debug.WriteLine($"Riadok {lineCount}: Nedostatočný počet stĺpcov ({parts.Length})");
                        errorCount++;
                        continue;
                    }

                    string value = parts[_pointsColumnIndex].Trim();
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
                folderDialog.Description = Strings.DialogSelectOutputFolder; //"Vyberte priečinok pre uloženie spracovaného súboru";

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
                    Settings.Default.LastOutputFolder = folderDialog.SelectedPath;
                    Settings.Default.Save();
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
                    MessageBox.Show(Strings.MessageNoHistoryToSave,
                        Strings.MessageWarning,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = Strings.DialogFilterTextFiles;  //"Textové súbory (*.txt)|*.txt|Všetky súbory (*.*)|*.*";
                    saveDialog.FilterIndex = 1;
                    saveDialog.DefaultExt = "txt";
                    saveDialog.FileName = $"{Strings.HistoryFileName}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        content.AppendLine(Strings.HistoryHeader); //("História spracovaných bodov");
                        content.AppendLine($"{Strings.HistoryCreated} {DateTime.Now:dd.MM.yyyy HH:mm:ss}"); //($"Vytvorené: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                        content.AppendLine("--------------------------------------------------");
                        content.AppendLine();

                        //Using the distinct to remove duplicates / Použite distinct na odstránenie duplicít
                        var uniqueFiles = processedFiles.GroupBy(f => f.UniqueIdentifier)
                                                        .Select(g => g.First());

                        foreach (var file in uniqueFiles)
                        {
                            content.AppendLine($"{file.FileName} " + Strings.HistoryTotalPoints + $"{file.Points:N0}");
                            content.AppendLine(file.SourceDirectory);
                            content.AppendLine("-----------------------");
                        }

                        content.AppendLine($"{Strings.HistoryDuplicates} {duplicatesRemoved}"); // počet zmazaných duplicít:
                        content.AppendLine($"{Strings.HistoryTotalPoints} {uniqueFiles.Sum(f => f.Points):N0}"); // Celkový súčet:
                        content.AppendLine($"{Strings.LabelFileCount} {uniqueFiles.Count()}");  //  Počet súborov:
                        int avgPoints = uniqueFiles.Any() ? (int)(uniqueFiles.Sum(f => f.Points) / uniqueFiles.Count()) : 0;
                        content.AppendLine($"{Strings.HistoryAveragePoints} {avgPoints:N0}");  //  Priemerné body:

                        File.WriteAllText(saveDialog.FileName, content.ToString());
                        //MessageBox.Show($"História bola úspešne uložená do súboru:\n{saveDialog.FileName}","Informácia"
                        MessageBox.Show(string.Format(Strings.MessageHistorySaved, Environment.NewLine + saveDialog.FileName),
                            Strings.MessageInfo, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                //"Chyba pri ukladaní histórie: "Chyba"  - Error saving history: 
                MessageBox.Show(Strings.MessageErrorSavingHistory + ex.Message,
                    Strings.MessageError, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void UpdateStatistics()
        {
            try
            {
                // Počítame len súčet z ListView - to sú naše aktuálne spracované súbory
                // Spočítame základné štatistiky
                int totalPoints = 0;
                decimal totalValue = 0m;
                int fileCount = ListViewShowPointsValues.Items.Count;
                int totalDiagnoses = 0;
                int totalServices = 0;

                // Agregujeme všetky údaje zo všetkých súborov
                foreach (var file in processedFiles)
                {
                    totalPoints += file.Points;
                    totalValue += file.TotalValue;
                    totalDiagnoses += file.DiagnosisCounts.Keys.Count;
                    totalServices += file.ServiceCounts.Keys.Count;
                }

                Debug.WriteLine($"Aktualizujem štatistiky - Celkové body: {totalPoints}");
                Debug.WriteLine($"Počet súborov: {fileCount}");
                Debug.WriteLine($"Jazyk: {LanguageComboBox.SelectedItem}, Body: {totalPoints}");

                // Aktualizujeme všetky labely s rovnakými hodnotami
                // Aktualizujeme všetky labely s novými hodnotami
                string pointsFormat = totalPoints.ToString("N0");
                LabelFileCount.Text = Strings.LabelFileCount + $" {fileCount}";
                LabelTotalPoints.Text = Strings.HistoryTotalPoints + $" {pointsFormat}";

                // Ak máte ďalšie labely pre rozšírené štatistiky, aktualizujte ich tu
                // LabelTotalValue.Text = "Celková hodnota: " + totalValue.ToString("N2") + " €";
                // LabelTotalDiagnoses.Text = "Počet diagnóz: " + totalDiagnoses.ToString("N0");
                // LabelTotalServices.Text = "Počet služieb: " + totalServices.ToString("N0");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Chyba pri aktualizácii štatistík: {ex.Message}");
                MessageBox.Show(Strings.MessageStatisticsError,
                    Strings.MessageError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadDataToGrid(string[] lines)
        {
            DataGridPreview.Rows.Clear();
            DataGridPreview.Columns.Clear();

            // 1. Nájdeme najvyšší počet oddeľovačov vo všetkých riadkoch
            int maxColumnCount = 0;
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    int separatorCount = line.Count(c => c == ColumnSeparator[0]);
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
                    if (line[i] == ColumnSeparator[0])
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
            //SetupColumnsNumberRange();

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
                                   .Select(c => (c.Value?.ToString() ?? "") + ColumnSeparator); // We add a separator to each value / Pridáme oddeľovač ku každej hodnote

                    selectedRows.Add(string.Join("", cells)); // no column separator, because it is already part of each column / Žiadny oddeľovač stĺpcov, pretože je už súčasťou každého stĺpca
                }
            }
            return selectedRows;
        }

        private async Task<string> SaveProcessedDataAsync(List<string> processedLines, string filePath)
        {
            // hodnota z UI kontrolky na hlavnom vlákne
            //string outputSeparator = ComboBoxExportSeparatorType.SelectedItem?.ToString() ?? columnSeparator;
            string outputSeparator = _exportSeparator ?? _importSeparator;

            // hodnota z TextBoxu na hlavnom vlákne
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
                    if (outputSeparator != ColumnSeparator)
                    {
                        processedLines = processedLines.Select(line =>
                            string.Join(outputSeparator,
                                       line.Split(new[] { ColumnSeparator }, StringSplitOptions.None)))
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
                        throw new UnauthorizedAccessException($"{Strings.MessageNoAccessToFolder} {targetDirectory}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{Strings.ErrorSavingFile}: {ex.Message}");
                    throw;
                }
            });
        }

        private void ComboBoxImportSeparatorType_DrawItem(object sender, DrawItemEventArgs e)
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
                MessageBox.Show(string.Format(Strings.MessageLanguageChangeError, ex.Message),
                    Strings.MessageError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateFormText();
            UpdateStatistics();
        }

        private void UpdateFormText()
        {
            ButtonSelectAFile.Text = Strings.ButtonSelectFile;
            ButtonProcessData.Text = Strings.ButtonProcessData;
            ButtonSelectOutputFolder.Text = Strings.ButtonSelectOutputFolder;
            ButtonSaveHistory.Text = Strings.ButtonSaveHistory;
            ButtonShowAnalysis.Text = Strings.Analysis;
            ButtonTableSettings.Text = Strings.ButtonTableSettings;

            CheckBoxRenumberTheOrder.Text = Strings.CheckBoxRenumberTheOrder;
            CheckBoxSortByName.Text = Strings.CheckBoxSortByName;
            CheckBoxRemoveDuplicatesRows.Text = Strings.CheckBoxRemoveDuplicatesRows;
            CheckBoxSelectAll.Text = Strings.CheckBoxSelectAll;

           // Získame aktuálne hodnoty
            int totalPoints = CalculateTotalPointsFromListView();
            int fileCount = ListViewShowPointsValues.Items.Count;

           // Aktualizuje labely s hodnotami
            LabelFileCount.Text = Strings.LabelFileCount + $" {fileCount}";
            LabelTotalPoints.Text = Strings.HistoryTotalPoints + $" {totalPoints:N0}";

            UpdateListViewColumns();
        }

        public void UpdateListViewColumns()
        {
            if (ListViewShowPointsValues.Columns.Count >= 3)
            {
                ListViewShowPointsValues.Columns[0].Text = Strings.ListViewColumnFile;
                ListViewShowPointsValues.Columns[1].Text = Strings.ListViewColumnPoints;
                ListViewShowPointsValues.Columns[2].Text = Strings.ListViewColumnDate;
            }
        }

        public int CalculateTotalPointsFromListView()
        {
            int totalPoints = 0;
            foreach (ListViewItem item in ListViewShowPointsValues.Items)
            {
                // Očistí text od medzier a získa čistú hodnotu
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

        // na overenie, či vybraný stĺpec existuje v údajoch
        public bool ValidatePointsColumn()
        {
            // Ak nie sú načítané žiadne údaje, preskočí validáciu
            if (DataGridPreview.Columns.Count <= 1) // Iba stĺpec s checkboxom
                return true;

            // Skontroluje, či vybraný stĺpec existuje v dátovej mriežke
            // +1 pretože prvý stĺpec je checkbox
            int columnIndex = _pointsColumnIndex + 1;
            bool isValid = columnIndex < DataGridPreview.Columns.Count;

            return isValid;
        }

        private void ButtonShowAnalysis_Click(object sender, EventArgs e)
        {
            var analyzer = new ClientAnalyzer();
            var clients = new List<ClientInfo>();

            foreach (var filePath in selectedFilePaths)
            {
                clients.AddRange(analyzer.ProcessBatchFile(filePath));
            }

            var aggregatedClients = analyzer.AggregateClients(clients);

            var analysisForm = new ClientAnalysisForm(aggregatedClients);
            analysisForm.ShowDialog();
        }

        private void ButtonTableSettings_Click(object sender, EventArgs e)
        {
            // Vytvorí inštanciu TableSettingsForm s požadovanými parametrami
            var settingsForm = new TableSettingsForm(
                this,
                _pointsColumnIndex,
                _nameColumnIndex,
                _idColumnIndex,
                _dayColumnIndex,
                _serviceCodeColumnIndex,
                _diagnosisColumnIndex,
                _importSeparator,
                DataGridPreview.Columns.Count,  // Predáva počet stĺpcov
                _totalLinesColumnIndex
            );
            // Formulár si nastaví rozsah hodnôt na základe počtu stĺpcov
            // v DataGridPreview, ktorý sa predáva v konštruktore

            // Zobrazí formulár a spracuje výsledok
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
        // Aktualizuje privátne premenné z nastavení, ktoré používateľ uložil
                _pointsColumnIndex = Settings.Default.PointsColumnIndex - 1;
                _nameColumnIndex = Settings.Default.NameColumnIndex - 1;
                _idColumnIndex = Settings.Default.IdColumnIndex - 1;
                _serviceCodeColumnIndex = Settings.Default.ServiceCodeColumnIndex - 1;
                _dayColumnIndex = Settings.Default.DayColumnIndex - 1;
                _diagnosisColumnIndex = Settings.Default.DiagnosisColumnIndex - 1;
                _totalLinesColumnIndex = Settings.Default.TotalLinesColumnIndex - 1;

                // Aktualizuje separátor
                _importSeparator = settingsForm.GetSelectedSeparator();
                _exportSeparator = settingsForm.GetExportSeparator();

                // Znovu načíta údaje s novým separátorom, ak je to potrebné
                if (selectedFilePaths.Count > 0)
                {
                    try
                    {
                        string[] lines = File.ReadAllLines(selectedFilePaths[0]);
                        LoadDataToGrid(lines);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"File load error: {ex.Message}",
                            Strings.MessageError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ButtonShowExtendedStatistics_Click(object sender, EventArgs e)
        {
            // Kontrola, či máme dáta na zobrazenie
            if (processedFiles.Count == 0)
            {
                MessageBox.Show(Strings.MessageNoHistoryToSave,
                    Strings.MessageWarning,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Vytvorenie formulára
                var statisticsForm = new ExtendedStatisticsForm(processedFiles.ToList());

                try
                {
                    // Zobrazenie formulára
                    statisticsForm.ShowDialog(this);
                }
                finally
                {
                    // Uistite sa, že formulár je uvoľnený
                    statisticsForm.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba: {ex.Message}\nStackTrace: {ex.StackTrace}",
                    "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
