using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
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
            ListViewShowPointsValues.Columns.Add("Súbor", 220);
            ListViewShowPointsValues.Columns.Add("Body", 100);
            ListViewShowPointsValues.Columns.Add("Dátum", 220);
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

                    // Aktualizácia titulku formulára
                    var fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    var baseTitle = this.Text.Split('-')[0].Trim(); // Získa pôvodný názov aplikácie
                    this.Text = $"{baseTitle} - {fileName}";
                    ButtonProcessData.Enabled = true; // Povolí tlačidlo po výbere nového súboru

                    try
                    {
                        string[] lines = System.IO.File.ReadAllLines(openFileDialog.FileName);
                        RichTextBoxDataPreview.Lines = lines;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Chyba pri čítaní súboru: {ex.Message}", "Chyba",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                .OrderBy(parts => parts[3])      // Primárne zoradenie podľa mena (4. stĺpec)
                .ThenBy(parts => int.Parse(parts[1]))  // Sekundárne zoradenie podľa druhého stĺpca
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

        private List<string> RemoveLeadingZeros(List<string> lines)
        {
            return lines
                .Select(line =>
                {
                    var parts = line.Split('|');
                    if (parts.Length > 4 && int.TryParse(parts[0], out _))
                    {
                        // Odstránenie núl z prvého stĺpca
                        if (int.TryParse(parts[0], out int firstNum))
                            parts[0] = firstNum.ToString();

                        // Odstránenie núl z druhého stĺpca
                        if (parts.Length > 1 && int.TryParse(parts[1], out int secondNum))
                            parts[1] = secondNum.ToString();

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
                .Where(parts => parts.Length > 6 && int.TryParse(parts[0], out _))
                .GroupBy(parts => $"{parts[1]}|{parts[2]}|{parts[5]}")  // Kľúč z 2., 3. a 6. stĺpca
                .Select(group => group.First())  // Vybrať prvý záznam z každej skupiny
                .OrderBy(parts => parts[3])      // Zoradiť podľa mena
                .Select((parts, index) => $"{index + 1}|{string.Join("|", parts.Skip(1))}") // Prečíslovať
                .ToList();
        }

        private int CalculateTotalPoints(List<string> lines)
        {
            return lines
                .Skip(2)  // Preskočiť hlavičku
                .Select(line => line.Split('|'))
                .Where(parts => parts.Length > 10)  // Kontrola či má riadok dostatok stĺpcov
                .Sum(parts =>
                {
                    // Sčítanie hodnôt len z 10. stĺpca (index 9)
                    if (int.TryParse(parts[9], out int points))
                        return points;
                    return 0;  // Ak nie je možné konvertovať na číslo, vráti 0
                });
        }


        private HashSet<string> processedFiles = new HashSet<string>(); // Zoznam spracovaných súborov

        private bool IsFileAlreadyProcessed(string fileName)// Pomocná metóda na kontrolu či súbor už bol spracovaný
        {
            foreach (ListViewItem item in ListViewShowPointsValues.Items)
            {
                if (item.SubItems[0].Text.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
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
                // Kontrola či súbor už bol spracovaný
                if (processedFiles.Contains(currentFile))
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
                RichTextBoxDataPreview.Lines = processedLines.ToArray();

                string outputPath = await SaveProcessedDataAsync(processedLines);
                int processedPoints = await CalculateTotalPointsAsync(processedLines);

                // Pridanie záznamu do histórie
                AddToHistory(Path.GetFileName(TextBoxSelectedFileDirectory.Text), processedPoints);

                // Po úspešnom spracovaní pridať do zoznamu
                processedFiles.Add(currentFile);

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

        private async Task<List<string>> ProcessDataAsync(List<string> lines)
        {
            return await Task.Factory.StartNew(() =>
            {
                var headerLines = lines.Take(2).ToList();
                var dataLines = lines.Skip(2).ToList();

                if (CheckBoxOmitTheHeader.Checked)
                    dataLines = RemoveLeadingZeros(dataLines);

                if (CheckBoxRemoveDuplicatesRows.Checked)
                {
                    dataLines = RemoveDuplicateRows(dataLines);
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

                foreach (var line in lines.Skip(2))  // Preskočíme prvé 2 riadky (hlavičku)
                {
                    lineCount++;
                    var parts = line.Split('|');

                    // Potrebujeme aspoň 11 stĺpcov (index 10 pre body)
                    if (parts.Length > 10)
                    {
                        string value = parts[10].Trim(); //index 10

                        if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int points) && points >= 0)
                        {
                            totalPoints += points;
                            Console.WriteLine($"Riadok {lineCount}: Pridávam {points} bodov. Celkový súčet: {totalPoints}");
                        }
                        else if (!string.IsNullOrEmpty(value))
                        {
                            Console.WriteLine($"Riadok {lineCount}: Neplatná alebo záporná hodnota: '{value}'");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Riadok {lineCount}: Príliš málo stĺpcov ({parts.Length})");
                    }
                }

                Console.WriteLine($"Celkový počet analyzovaných riadkov: {lineCount}");
                Console.WriteLine($"Celkový súčet bodov: {totalPoints}");
                return totalPoints;
            });
        }

        private async Task<string> SaveProcessedDataAsync(List<string> processedLines)
        {
            return await Task.Factory.StartNew(() =>
            {
                string outputPath;
                if (!string.IsNullOrEmpty(TextBoxSelectOutputFolder.Text))
                {
                    outputPath = Path.Combine(
                        TextBoxSelectOutputFolder.Text,
                        "processed_" + Path.GetFileName(TextBoxSelectedFileDirectory.Text)
                    );
                }
                else
                {
                    outputPath = Path.Combine(
                        Path.GetDirectoryName(TextBoxSelectedFileDirectory.Text),
                        "processed_" + Path.GetFileName(TextBoxSelectedFileDirectory.Text)
                    );
                }

                string targetDirectory = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                File.WriteAllLines(outputPath, processedLines);
                return outputPath;
            });
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

                    if (!string.IsNullOrEmpty(Properties.Settings.Default.LastOutputFolder))
                    {
                        saveDialog.InitialDirectory = Properties.Settings.Default.LastOutputFolder;
                    }

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Výpočet súhrnných informácií
                        int totalPoints = 0;
                        foreach (ListViewItem item in ListViewShowPointsValues.Items)
                        {
                            string pointsText = item.SubItems[1].Text;
                            // Odstráňte všetky medzery a skúste skonvertovať na číslo
                            pointsText = pointsText.Replace(" ", "").Replace(",", "");
                            if (int.TryParse(pointsText, out int points))
                            {
                                totalPoints += points;
                            }
                        }

                        int fileCount = ListViewShowPointsValues.Items.Count;
                        double averagePoints = fileCount > 0 ? (double)totalPoints / fileCount : 0;

                        StringBuilder content = new StringBuilder();
                        content.AppendLine("História spracovaných bodov");
                        content.AppendLine($"Vytvorené: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                        content.AppendLine("--------------------------------------------------");
                        content.AppendLine();

                        // Zápis záznamov
                        foreach (ListViewItem item in ListViewShowPointsValues.Items)
                        {
                            content.AppendLine($"{item.SubItems[0].Text}  Celkové body: {item.SubItems[1].Text}");
                            content.AppendLine($"Dátum spracovania: {item.SubItems[2].Text}");
                            content.AppendLine("-----------------------");
                        }

                        // Zápis súhrnných informácií s vypočítanými hodnotami
                        content.AppendLine();
                        content.AppendLine($"Celkový súčet: {totalPoints:N0}");
                        content.AppendLine($"Počet súborov: {fileCount}");
                        content.AppendLine($"Priemerné body: {averagePoints:N0}");  

                        File.WriteAllText(saveDialog.FileName, content.ToString());

                        MessageBox.Show($"História bola úspešne uložená do súboru:\n{saveDialog.FileName}",
                            "Informácia", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Properties.Settings.Default.LastOutputFolder = Path.GetDirectoryName(saveDialog.FileName);
                        Properties.Settings.Default.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba pri ukladaní histórie: {ex.Message}",
                    "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddToHistory(string fileName, int points)
        {
            // Vytvorenie nového záznamu
            ListViewItem item = new ListViewItem(new[] {
                fileName,
                points.ToString("N0"),
                DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
            });

            // Pridanie do ListView
            ListViewShowPointsValues.Items.Add(item);

            // Aktualizácia štatistík
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            Console.WriteLine("UpdateStatistics sa volá");
            int totalPoints = 0;
            foreach (ListViewItem item in ListViewShowPointsValues.Items)
            {
                string pointsText = item.SubItems[1].Text;
                Console.WriteLine($"Pôvodná hodnota: '{pointsText}'");

                // Odstránenie všetkých bielych znakov a špeciálnych znakov
                pointsText = new string(pointsText.Where(c => char.IsDigit(c)).ToArray());

                Console.WriteLine($"Upravená hodnota pre parsing: '{pointsText}'");

                if (int.TryParse(pointsText, out int points))
                {
                    totalPoints += points;
                    Console.WriteLine($"Úspešne parsované číslo: {points}");
                }
                else
                {
                    Console.WriteLine($"Nepodarilo sa parsovať hodnotu: '{pointsText}'");
                }
            }

            int fileCount = ListViewShowPointsValues.Items.Count;
            double averagePoints = fileCount > 0 ? (double)totalPoints / fileCount : 0;

            // Nastavenie textu labelov s formátovaním
            LabelTotalSum.Text = $"Celkový súčet: {totalPoints:N0}";
            LabelFileCount.Text = $"Počet súborov: {fileCount}";
            LabelAverage.Text = $"Priemerné body: {averagePoints:N0}";

            Console.WriteLine($"Nastavené hodnoty:");
            Console.WriteLine(LabelTotalSum.Text);
            Console.WriteLine(LabelFileCount.Text);
            Console.WriteLine(LabelAverage.Text);
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

        private void ClearHistory()
        {
            ListViewShowPointsValues.Items.Clear();
            processedFiles.Clear();
            UpdateStatistics();
        }
    }
}
