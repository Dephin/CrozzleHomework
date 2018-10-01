using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CrozzleApplication.GenerateCrozzle;

namespace CrozzleApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
//        [STAThread]
        static void Main()
        {
            //            #region Library
            //            ClassLibrary1.Class1.log();
            //            #endregion
            //
            //            Application.EnableVisualStyles();
            //            Application.SetCompatibleTextRenderingDefault(false);
            //            Application.Run(new CrozzleViewerForm());


            Config.NonIntersectingLetterPoints = new int[] { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            Config.IntersectingLetterPoints = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 1, 1, 1, 1, 1, 1, 1 };
            MagicBoard CrozzleBoard = new MagicBoard(10, 15); // Dynamic Crozzle board - 10rows 15cols
            List<Word> Wordlist = new List<Word>() { new Word("AAOAT"), new Word("AARON"), new Word("ELIZIBETH") };
            string Expected = "AARON";

            // Act
            List<ActiveWord> BestWord = CrozzleBoard.GetBestWord(Wordlist);
            string Actual = BestWord[0].String;
        }
    }
}