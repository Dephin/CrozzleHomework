using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CrozzleApplication.GenerateCrozzle;

namespace CrozzleApplication
{
    static class Program
    {
        // <summary>
        // The main entry point for the application.
        // </summary>
        [STAThread]
        static void Main()
        {
            #region Library
            ClassLibrary1.Class1.log();
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CrozzleViewerForm());
        }
    }
}