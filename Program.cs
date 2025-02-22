using System;
using System.Threading;
using System.Windows.Forms;

namespace CountAndSortWinFormsAppNetFr4
{
    static class Program
    {
        // Vytvorenie globálneho identifikátora pre Mutex
        private static string mutexId = "Global\\CountAndSortWinFormsAppNetFr4_UniqueInstance";
        private static Mutex mutex = null;

        [STAThread]
        static void Main()
        {
            bool createdNew;

            try
            {
                mutex = new Mutex(true, mutexId, out createdNew);

                if (!createdNew)
                {
                    MessageBox.Show("Aplikácia je už spustená.",
                        "Upozornenie",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                if (!CheckDotNetVersion())
                {
                    MessageBox.Show("Táto aplikácia vyžaduje .NET Framework 4.0 alebo novší.",
                        "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new SelectFileForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neočakávaná chyba: {ex.Message}",
                    "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (mutex != null)
                {
                    mutex.ReleaseMutex();
                    mutex.Dispose();
                }
            }
        }

        private static bool CheckDotNetVersion()
        {
            try
            {
                return Environment.Version.Major >= 4;
            }
            catch
            {
                return false;
            }
        }
    }
}
