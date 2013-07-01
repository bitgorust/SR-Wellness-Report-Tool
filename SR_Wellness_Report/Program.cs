using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SR_Wellness_Report
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //MessageBox.Show("It's my SHOW TIME!\nJust have a rest and watch my show!");
            Application.Run(new FormExport());
            Application.Run(new FormReport());
        }
    }
}
