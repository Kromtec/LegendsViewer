using System;
using System.Windows.Forms;

namespace LegendsViewer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
            {
                Application.Run(new FrmLegendsViewer(args[0]));
            }
            else
            {
                Application.Run(new FrmLegendsViewer());
            }
        }
    }
}
