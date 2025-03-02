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
                    MessageBox.Show("The application is already running.\n\nAplikácia je už spustená.",
                        "Warning  (Upozornenie)",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                if (!CheckSystemRequirements())
                {
                    MessageBox.Show("This application requires Windows Vista SP2 or later operating system.\n\nTáto aplikácia vyžaduje Windows Vista SP2 alebo novší operačný systém.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new SelectFileForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error! \n Neočakávaná chyba:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private static bool CheckSystemRequirements()
        {
            try
            {
                // Kontrola verzie .NET Framework
                if (Environment.Version.Major < 4)
                {
                    return false;
                }

                // Kontrola verzie Windows
                // - Windows Vista = 6.0
                // - Windows 7 = 6.1
                // - Windows 8 = 6.2
                // - Windows 8.1 = 6.3
                // - Windows 10 = 10.0
                OperatingSystem os = Environment.OSVersion;
                Version minimumVersion = new Version(6, 0); // Windows Vista

                return os.Platform == PlatformID.Win32NT && os.Version >= minimumVersion;
            }
            catch
            {
                return false;
            }
        }
    }
}
