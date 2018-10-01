using System.Collections.Generic;
using CrozzleApplication.GenerateCrozzle;

namespace CrozzleApplication
{
    //[TestClass]
    public class Test_MaxScoreCrozzle
    {


        public  void Test_BestWord(string[] args)
        {
//          Arrange
            Config.NonIntersectingLetterPoints = new int[] { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            Config.IntersectingLetterPoints = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 1, 1, 1, 1, 1, 1, 1 };
            MagicBoard CrozzleBoard = new MagicBoard(10,15); // Dynamic Crozzle board - 10rows 15cols
            List<Word> Wordlist = new List<Word>() { new Word("SCOTT"), new Word("AARON"), new Word("ELIZIBETH") };
            string Expected = "AARON";

            // Act
            List<ActiveWord> BestWord = CrozzleBoard.GetBestWord(Wordlist);
            string Actual = BestWord[0].String;

            // Assert
//            Assert.AreEqual(Expected, Actual);
        }




//        [TestMethod]
//        public void GetBestWord_EmptyGridBoard_Test()
//        {
//            // Arrange
//            Config.NonIntersectingLetterPoints = new int[] { 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
//            Config.IntersectingLetterPoints = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 1, 1, 1, 1, 1, 1, 1 };
//            MagicBoard CrozzleBoard = new MagicBoard(10,15); // Dynamic Crozzle board - 10rows 15cols
//            List<Word> Wordlist = new List<Word>() { new Word("SCOTT"), new Word("AARON"), new Word("ELIZIBETH") };
//            string Expected = "AARON";
//
//            // Act
//            List<ActiveWord> BestWord = CrozzleBoard.GetBestWord(Wordlist);
//            string Actual = BestWord[0].String;
//
//            // Assert
//            Assert.AreEqual(Expected, Actual);
//
//        }
    }
}