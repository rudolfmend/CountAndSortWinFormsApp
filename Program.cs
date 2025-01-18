using System;
using System.Windows.Forms;

namespace CountAndSortWinFormsAppNetFr4
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
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
