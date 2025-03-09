using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CountAndSortWinFormsAppNetFr4
{
    public class ClientAnalyzer
    {
        public List<ClientInfo> ProcessBatchFile(string filePath)
        { 
            var clients = new Dictionary<string, ClientInfo>();

            try
            {
                // Kontrola existencie súboru
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"{Properties.Strings.FileNotFound} {filePath}"); // File not found:
                }

                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length < 3)
                {
                    // Nedostatočný počet riadkov pre spracovanie
                    return new List<ClientInfo>();
                }

                // Extrakcia hlavičky dávky (prvý riadok)
                string headerLine = lines[0];
                string[] headerParts = headerLine.Split('|');

                // Získanie identifikátora poistovne
                string insuranceProvider = string.Empty; //- identifikátor poistovne je 4-miestny reťazec ( 2400, 2500, 2700, 25UA)
                if (headerParts.Length > 4 && !string.IsNullOrEmpty(headerParts[4]))
                {
                    insuranceProvider = headerParts[4];
                }

                // Druhý riadok zvyčajne obsahuje údaje o odosielateľovi, preskakujeme

                // Spracovanie riadkov s údajmi klientov (od 3. riadku)
                for (int i = 2; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    string[] parts = line.Split('|');
                    if (parts.Length < 15) continue; // Preskočiť neúplné riadky

                    // Extrakcia základných údajov o klientovi
                    string personalId = parts[2]; // Rodné číslo
                    string name = parts[3];       // Meno
                    string code = parts.Length > 4 ? parts[4] : ""; // Kód diagnózy/výkonu
                    string serviceCode = parts.Length > 5 ? parts[5] : ""; // Kód služby

                    // Extrakcia bodov (stĺpec 11)
                    int points = 0;
                    if (parts.Length > 10 && !string.IsNullOrEmpty(parts[10]))
                    {
                        int.TryParse(parts[10], out points);
                    }

                    // Extrakcia koeficientu (tretí od konca)
                    decimal coefficient = 0.02M; // Predvolená hodnota
                    if (parts.Length > 32 && !string.IsNullOrEmpty(parts[32]))
                    {
                        decimal.TryParse(parts[32].Replace(',', '.'),
                                       System.Globalization.NumberStyles.Any,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       out coefficient);
                    }

                    // Výpočet hodnoty v eurách
                    decimal value = points * coefficient;

                    // Vytvorenie alebo aktualizácia klienta v slovníku
                    string identifier = $"{personalId} {name}";
                    if (!clients.ContainsKey(identifier))
                    {
                        clients[identifier] = new ClientInfo
                        {
                            PersonalId = personalId,
                            Name = name,
                            InsuranceProvider = insuranceProvider,
                            Coefficient = coefficient
                        };
                    }

                    var client = clients[identifier];
                    client.ServiceCount++;
                    client.TotalPoints += points;
                    client.TotalValue += value;

                    // Uloženie unikátnych kódov služieb a diagnóz
                    if (!string.IsNullOrEmpty(code) && !client.Codes.Contains(code))
                    {
                        client.Codes.Add(code);
                    }

                    if (!string.IsNullOrEmpty(serviceCode) && !client.ServiceCodes.Contains(serviceCode))
                    {
                        client.ServiceCodes.Add(serviceCode); // Uloženie unikátnych kódov služieb
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{Properties.Strings.MessageProcessingError} {filePath}: {ex.Message}"); // Error processing filee
                throw; // Preposielame výnimku vyšším vrstvám
            }

            return clients.Values.ToList();
        }

        // Metóda na agregovanie klientov z viacerých súborov
        public List<ClientInfo> AggregateClients(List<ClientInfo> clients)
        {
            var aggregatedClients = new Dictionary<string, ClientInfo>();

            foreach (var client in clients)
            {
                if (!aggregatedClients.ContainsKey(client.Identifier))
                {
                    aggregatedClients[client.Identifier] = new ClientInfo
                    {
                        PersonalId = client.PersonalId,
                        Name = client.Name,
                        InsuranceProvider = client.InsuranceProvider,
                        Coefficient = client.Coefficient
                    };
                }

                var aggregatedClient = aggregatedClients[client.Identifier];
                aggregatedClient.ServiceCount += client.ServiceCount;
                aggregatedClient.TotalPoints += client.TotalPoints;
                aggregatedClient.TotalValue += client.TotalValue;

                // Spojenie zoznamov kódov
                foreach (var code in client.Codes)
                {
                    if (!aggregatedClient.Codes.Contains(code))
                    {
                        aggregatedClient.Codes.Add(code);
                    }
                }

                // Spojenie zoznamov kódov služieb
                foreach (var serviceCode in client.ServiceCodes)
                {
                    if (!aggregatedClient.ServiceCodes.Contains(serviceCode))
                    {
                        aggregatedClient.ServiceCodes.Add(serviceCode);
                    }
                }
            }

            return aggregatedClients.Values.ToList();
        }

        // Pomocná metóda na získanie top N klientov podľa bodov
        public List<ClientInfo> GetTopClientsByPoints(List<ClientInfo> clients, int count)
        {
            return clients
                .OrderByDescending(c => c.TotalPoints)
                .Take(count)
                .ToList();
        }

        // Pomocná metóda na získanie top N klientov podľa hodnoty
        public List<ClientInfo> GetTopClientsByValue(List<ClientInfo> clients, int count)
        {
            return clients
                .OrderByDescending(c => c.TotalValue)
                .Take(count)
                .ToList();
        }
    }
}
