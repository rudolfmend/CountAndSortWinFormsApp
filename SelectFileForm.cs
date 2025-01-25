using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        private const int HEADER_LINES = 2;
        private const int COLUMN_NAME_INDEX = 3;        // Index stĺpca s menom
        private const int COLUMN_ID_INDEX = 1;          // Index pre sekundárne triedenie
        private const int COLUMN_POINTS_INDEX = 10;     // Index stĺpca s bodmi
        private const int MIN_COLUMNS_REQUIRED = 11;    // Minimálny počet stĺpcov
        private const string COLUMN_SEPARATOR = "|";    // Oddeľovač stĺpcov

        // Konštanty pre formátovanie výstupu
        private const string DATE_TIME_FORMAT = "dd.MM.yyyy HH:mm:ss";
        private const string NUMBER_FORMAT = "N0";
        private const string PROCESSED_PREFIX = "processed_";

        // Konštanty pre ListView
        private const int COLUMN_WIDTH_FILENAME = 220;
        private const int COLUMN_WIDTH_POINTS = 100;
        private const int COLUMN_WIDTH_DATE = 220;
        

        public SelectFileForm()
        {
            InitializeComponent();

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

            // Inicializácia stĺpcov pre ListView
            ListViewShowPointsValues.Columns.Add("Súbor", COLUMN_WIDTH_FILENAME);
            ListViewShowPointsValues.Columns.Add("Body", COLUMN_WIDTH_POINTS);
            ListViewShowPointsValues.Columns.Add("Dátum", COLUMN_WIDTH_DATE);

        }

        private class ProcessedFileInfo
        {
            // properties for unique identification
            public string FileName { get; set; }  // Added missing property
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
                //SourceDirectory = Path.GetDirectoryName(TextBoxSelectedFileDirectory.Text);
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

        private HashSet<ProcessedFileInfo> processedFiles = new HashSet<ProcessedFileInfo>();

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

            //Add only if it does not already exist with the same identifier / Pridať len ak ešte neexistuje s rovnakým identifikátorom
            if (!processedFiles.Any(f => f.UniqueIdentifier == newFileInfo.UniqueIdentifier))
            {
                processedFiles.Add(newFileInfo);
            }

            //Updating ListView / Aktualizácia ListView
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

            UpdateStatistics();
        }

        private void ButtonSelectAFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Všetky súbory (*.*)|*.*|súbory (*.001)|*.001|súbory (*.002)|*.002|Textové súbory (*.txt)|*.txt";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    TextBoxSelectedFileDirectory.Text = openFileDialog.FileName;
                    ButtonProcessData.Enabled = true;

                    try
                    {
                        string[] lines = File.ReadAllLines(openFileDialog.FileName);

                        // Reset DataGridView
                        DataGridPreview.Rows.Clear();
                        DataGridPreview.Columns.Clear();

                        // Určenie maximálneho počtu stĺpcov
                        int maxColumns = lines
                            .Select(line => line.Split('|').Length)
                            .Max();

                        // Pridanie checkbox stĺpca
                        DataGridPreview.Columns.Add(new DataGridViewCheckBoxColumn
                        {
                            Name = "Selected",
                            HeaderText = "",
                            Width = 30
                        });

                        // Pridanie dátových stĺpcov
                        for (int i = 0; i < maxColumns; i++)
                        {
                            DataGridPreview.Columns.Add(new DataGridViewTextBoxColumn
                            {
                                Name = $"Column{i}",
                                HeaderText = $"Stĺpec {i + 1}",
                                Width = 100,
                                ReadOnly = true
                            });
                        }

                        // Naplnenie dát
                        foreach (string line in lines)
                        {
                            string[] parts = line.Split('|');
                            int rowIndex = DataGridPreview.Rows.Add();
                            var row = DataGridPreview.Rows[rowIndex];

                            // Checkbox
                            row.Cells[0].Value = false;

                            // Dáta
                            for (int i = 0; i < parts.Length && i < maxColumns; i++)
                            {
                                row.Cells[i + 1].Value = parts[i];
                            }
                        }

                        // Nastavenia DataGridView
                        DataGridPreview.AllowUserToAddRows = false;
                        DataGridPreview.AllowUserToDeleteRows = false;
                        DataGridPreview.MultiSelect = true;
                        DataGridPreview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                        // Povoliť editáciu len pre checkbox stĺpec
                        foreach (DataGridViewColumn col in DataGridPreview.Columns)
                        {
                            col.ReadOnly = col.Name != "Selected";
                        }

                        // Aktualizácia titulku
                        var fileName = Path.GetFileName(openFileDialog.FileName);
                        var baseTitle = this.Text.Split('-')[0].Trim();
                        this.Text = $"{baseTitle} - {fileName}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Chyba pri čítaní súboru: {ex.Message}",
                            "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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
                .Select(line => line.Split('|'))
                .Where(parts => parts.Length > 4 && int.TryParse(parts[0], out _))
                .OrderBy(parts => parts[COLUMN_NAME_INDEX])      // Primárne zoradenie podľa mena (4. stĺpec)
                .ThenBy(parts => int.Parse(parts[COLUMN_ID_INDEX])) // Sekundárne zoradenie podľa druhého stĺpca
                .Select(parts => string.Join("|", parts))
                .ToList();
        }

        private List<string> RenumberFirstColumn(List<string> lines)
        {
            int newNumber = 1;
            return lines
                .Select(line =>
                {
                    var parts = line.Split('|');
                    if (parts.Length > 4 && int.TryParse(parts[0], out _))
                    {
                        parts[0] = newNumber++.ToString();
                        return string.Join("|", parts);
                    }
                    return line;
                })
                .ToList();
        }

        private List<string> RemoveDuplicateRows(List<string> lines)
        {
            return lines
                .Select(line => line.Split('|'))
                .Where(parts => parts.Length > MIN_COLUMNS_REQUIRED && int.TryParse(parts[0], out _))
                .GroupBy(parts => $"{parts[COLUMN_ID_INDEX]}|{parts[2]}|{parts[5]}")// Kľúč z 2., 3. a 6. stĺpca
                .Select(group => group.First())  // Vybrať prvý záznam z každej skupiny
                .OrderBy(parts => parts[COLUMN_NAME_INDEX]) // Zoradiť podľa mena
                .Select((parts, index) => $"{index + 1}|{string.Join("|", parts.Skip(1))}") // Prečíslovať
                .ToList();
        }

        private int CalculateTotalPoints(List<string> lines)
        {
            return lines
                .Skip(2)  // Preskočiť hlavičku
                .Select(line => line.Split('|'))
                .Where(parts => parts.Length > COLUMN_POINTS_INDEX)  // Kontrola či má riadok dostatok stĺpcov
                .Sum(parts =>
                {
                    // Sčítanie hodnôt len z 11. stĺpca (index 10)
                    if (int.TryParse(parts[COLUMN_POINTS_INDEX], out int points))
                        return points;
                    return 0;  // Ak nie je možné konvertovať na číslo, vráti 0
                });
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

                if (!File.Exists(TextBoxSelectedFileDirectory.Text))
                {
                    MessageBox.Show("Vybraný súbor neexistuje.", "Chyba",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string currentFile = Path.GetFileName(TextBoxSelectedFileDirectory.Text);
                string currentFilePath = TextBoxSelectedFileDirectory.Text;
                string sourceDirectory = Path.GetDirectoryName(TextBoxSelectedFileDirectory.Text);

                if (IsFileAlreadyProcessed(currentFilePath))
                {
                    var result = MessageBox.Show(
                        "Tento súbor už bol spracovaný. Chcete ho spracovať znova?",
                        "Upozornenie",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                        return;
                }

                if (string.IsNullOrEmpty(TextBoxSelectedFileDirectory.Text))
                {
                    MessageBox.Show("Prosím, najprv vyberte súbor.", "Upozornenie",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var originalLines = (await Task.Factory.StartNew(() =>
                    File.ReadAllLines(TextBoxSelectedFileDirectory.Text))).ToList();
                int originalPoints = await CalculateTotalPointsAsync(originalLines);

                var processedLines = await ProcessDataAsync(originalLines);
                string outputPath = await SaveProcessedDataAsync(processedLines);
                int processedPoints = await CalculateTotalPointsAsync(processedLines);

                // Pass source directory to AddToHistory
                AddToHistory(currentFilePath, processedPoints, sourceDirectory);

                MessageBox.Show(
                    $"Údaje boli spracované a uložené do súboru:\n{outputPath}\n" +
                    $"\nPočet zmazaných riadkov: {originalLines.Count - processedLines.Count}" +
                    $"\nPôvodný počet riadkov: {originalLines.Count}\nPočet spracovaných riadkov: {processedLines.Count}" +
                    $"\n\nPôvodný počet bodov: {originalPoints}" +
                    $"\nPočet bodov po spracovaní: {processedPoints}",
                    "Hotovo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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
            }
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
                    unselectedRows.Add(string.Join("|", cells));
                }
            }
            return unselectedRows;
        }

        private async Task<List<string>> ProcessDataAsync(List<string> lines)
        {
            return await Task.Factory.StartNew(() =>
            {
                var headerLines = lines.Take(2).ToList();
                var dataLines = lines.Skip(2).ToList();
                // Získame len neoznačené riadky
                var unselectedRows = GetUnselectedRows();

                if (CheckBoxRemoveDuplicatesRows.Checked)
                {
                    var originalCount = dataLines.Count;
                    dataLines = RemoveDuplicateRows(dataLines);
                    //duplicatesRemoved = originalCount - dataLines.Count;
                    //// Store duplicate count for current file
                    //currentFileInfo.RemovedDuplicates = duplicatesRemoved;
                }
                else
                {
                    if (CheckBoxSortByName.Checked)
                        dataLines = SortLinesByName(dataLines);

                    if (CheckBoxRenumberTheOrder.Checked)
                        dataLines = RenumberFirstColumn(dataLines);
                }

                return headerLines.Concat(dataLines).ToList();
            });
        }

        private async Task<int> CalculateTotalPointsAsync(List<string> lines)
        {
            return await Task.Factory.StartNew(() =>
            {
                int totalPoints = 0;
                int lineCount = 0;
                int errorCount = 0;

                foreach (var line in lines.Skip(HEADER_LINES))
                {
                    lineCount++;
                    var parts = line.Split(COLUMN_SEPARATOR[0]);

                    if (parts.Length <= COLUMN_POINTS_INDEX)
                    {
                        Debug.WriteLine($"Riadok {lineCount}: Nedostatočný počet stĺpcov ({parts.Length})");
                        errorCount++;
                        continue;
                    }

                    string value = parts[COLUMN_POINTS_INDEX].Trim();
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

        // Pomocná metóda na kontrolu prístupu k priečinku
        private bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                // Pokúsime sa vytvoriť testovací súbor
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

        // Pomocná trieda pre File operácie
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
                folderDialog.Description = "Vyberte priečinok pre uloženie spracovaného súboru";

                // Ak už bol predtým vybraný priečinok, začneme od neho
                if (!string.IsNullOrEmpty(TextBoxSelectOutputFolder.Text) &&
                    System.IO.Directory.Exists(TextBoxSelectOutputFolder.Text))
                {
                    folderDialog.SelectedPath = TextBoxSelectOutputFolder.Text;
                }

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    TextBoxSelectOutputFolder.Text = folderDialog.SelectedPath;

                    // Uloženie novej cesty
                    Properties.Settings.Default.LastOutputFolder = folderDialog.SelectedPath;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void SaveHistoryToFile()
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Textové súbory (*.txt)|*.txt|Všetky súbory (*.*)|*.*";
                saveDialog.FilterIndex = 1;
                saveDialog.DefaultExt = "txt";
                saveDialog.FileName = $"historia_bodov_{DateTime.Now:yyyyMMdd}.txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveDialog.FileName, ListViewShowPointsValues.Text);
                    MessageBox.Show("História bola úspešne uložená.", "Informácia",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ButtonSaveHistory_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListViewShowPointsValues.Items.Count == 0)
                {
                    MessageBox.Show("Nie je k dispozícii žiadna história na uloženie.",
                        "Upozornenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Textové súbory (*.txt)|*.txt|Všetky súbory (*.*)|*.*";
                    saveDialog.FilterIndex = 1;
                    saveDialog.DefaultExt = "txt";
                    saveDialog.FileName = $"historia_bodov_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder content = new StringBuilder();
                        content.AppendLine("História spracovaných bodov");
                        content.AppendLine($"Vytvorené: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                        content.AppendLine("--------------------------------------------------");
                        content.AppendLine();

                        // Použite distinct na odstránenie duplicít
                        var uniqueFiles = processedFiles.GroupBy(f => f.UniqueIdentifier)
                                                      .Select(g => g.First());

                        foreach (var file in uniqueFiles)
                        {
                            content.AppendLine($"{file.FileName}  Celkové body: {file.Points:N0}");
                            content.AppendLine(file.SourceDirectory);
                            content.AppendLine($"Dátum spracovania: {file.ProcessedTime:dd.MM.yyyy HH:mm:ss}");
                            content.AppendLine("-----------------------");
                        }

                        content.AppendLine($"Celkový počet zmazaných duplicít: {0}"); // Upravte podľa potreby
                        content.AppendLine($"Celkový súčet: {uniqueFiles.Sum(f => f.Points):N0}");
                        content.AppendLine($"Počet súborov: {uniqueFiles.Count()}");
                        int avgPoints = uniqueFiles.Any() ? (int)(uniqueFiles.Sum(f => f.Points) / uniqueFiles.Count()) : 0;
                        content.AppendLine($"Priemerné body: {avgPoints:N0}");

                        File.WriteAllText(saveDialog.FileName, content.ToString());
                        MessageBox.Show($"História bola úspešne uložená do súboru:\n{saveDialog.FileName}",
                            "Informácia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba pri ukladaní histórie: {ex.Message}",
                    "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                int totalPoints = 0;
                int fileCount = ListViewShowPointsValues.Items.Count;
                int errorCount = 0;

                Debug.WriteLine($"Začínam aktualizáciu štatistík. Počet položiek: {fileCount}");

                foreach (ListViewItem item in ListViewShowPointsValues.Items)
                {
                    Debug.WriteLine($"Spracovávam položku: {item.SubItems[0].Text}");
                    Debug.WriteLine($"Hodnota bodov (raw): {item.SubItems[1].Text}");

                    // Odstránime všetky whitespace znaky
                    string pointsText = new string(item.SubItems[1].Text.Where(c => !char.IsWhiteSpace(c)).ToArray());
                    Debug.WriteLine($"Hodnota bodov (očistená): {pointsText}");

                    if (int.TryParse(pointsText, out int points))
                    {
                        totalPoints += points;
                        Debug.WriteLine($"Úspešne pripočítané body: {points}, nový súčet: {totalPoints}");
                    }
                    else
                    {
                        errorCount++;
                        Debug.WriteLine($"CHYBA: Nepodarilo sa spracovať hodnotu: '{item.SubItems[1].Text}'");
                    }
                }

                double averagePoints = fileCount > 0 ? (double)totalPoints / fileCount : 0;

                Debug.WriteLine($"Finálne hodnoty:");
                Debug.WriteLine($"Celkový súčet: {totalPoints}");
                Debug.WriteLine($"Počet súborov: {fileCount}");
                Debug.WriteLine($"Priemer: {averagePoints}");

                LabelTotalSum.Text = $"Celkový súčet: {totalPoints:N0}";
                LabelFileCount.Text = $"Počet súborov: {fileCount}";
                LabelAverage.Text = $"Priemerné body: {averagePoints:N0}";

                if (errorCount > 0)
                {
                    Debug.WriteLine($"Počet chýb pri výpočte štatistík: {errorCount}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Chyba pri aktualizácii štatistík: {ex.Message}");
                MessageBox.Show("Nastala chyba pri aktualizácii štatistík.",
                    "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisableButtons()
        {
            ButtonProcessData.Enabled = false;
            ButtonSelectAFile.Enabled = false;
            ButtonSelectOutputFolder.Enabled = false;
        }

        private void EnableButtons()
        {
            ButtonSelectAFile.Enabled = true;
            ButtonSelectOutputFolder.Enabled = true;
            // ButtonProcessData zostane zakázané, kým sa nevyberie nový súbor
        }

        private void LoadDataToGrid(string[] lines)
        {
            DataGridPreview.Rows.Clear();
            foreach (string line in lines)
            {
                var parts = line.Split('|');
                // Pridanie riadku s checkboxom
                int rowIndex = DataGridPreview.Rows.Add(false, parts); // false je pre checkbox
                var row = DataGridPreview.Rows[rowIndex];

                // Nastavenie ďalších buniek
                for (int i = 0; i < parts.Length; i++)
                {
                    row.Cells[i + 1].Value = parts[i];
                }
            }
        }

        private void ClearHistory()
        {
            ListViewShowPointsValues.Items.Clear();
            processedFiles.Clear();
            UpdateStatistics();
        }

        private void CheckBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DataGridPreview.Rows)
            {
                row.Cells["Selected"].Value = CheckBoxSelectAll.Checked;
            }
        }

        private string GetUniqueFileName(string filePath)
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

        private async Task<string> SaveProcessedDataAsync(List<string> processedLines)
        {
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    string outputPath;
                    string originalDirectory = Path.GetDirectoryName(TextBoxSelectedFileDirectory.Text);
                    string originalFileName = Path.GetFileName(TextBoxSelectedFileDirectory.Text);

                    Debug.WriteLine($"Pôvodný adresár: {originalDirectory}");
                    Debug.WriteLine($"Pôvodný súbor: {originalFileName}");

                    if (!string.IsNullOrEmpty(TextBoxSelectOutputFolder.Text) &&
                        Directory.Exists(TextBoxSelectOutputFolder.Text) &&
                        HasWriteAccessToFolder(TextBoxSelectOutputFolder.Text))
                    {
                        outputPath = Path.Combine(
                            TextBoxSelectOutputFolder.Text,
                            PROCESSED_PREFIX + originalFileName
                        );
                        Debug.WriteLine($"Použitý výstupný adresár: {TextBoxSelectOutputFolder.Text}");
                    }
                    else
                    {
                        outputPath = Path.Combine(
                            originalDirectory,
                            PROCESSED_PREFIX + originalFileName
                        );
                        Debug.WriteLine("Použitý pôvodný adresár");
                    }

                    string uniqueOutputPath = GetUniqueFileName(outputPath);
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
    }
}
