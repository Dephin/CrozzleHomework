using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crozzle2.CrozzleElements
{
    public class ConfigRef
    {
        #region Constants
        private const int _MinNumberOfRows = 4;
        private const int _MaxNumberOfRows = 400;
        private const int _MinNumberOfCols = 8;
        private const int _MaxNumberOfCols = 800;
        private const int _MinNumberOfHorizontalWords = 0;
        private const int _MinNumberOfVerticalWords = 0;
        private const int _MinNumberOfWords = 10;
        private const int _MaxNumberOfWords = 1000;
        private const string _HorizontalKeyWord = "HORIZONTAL";
        private const string _VerticalKeyWord = "VERTICAL";
        private const int _MinIntersectingWords = 1;
        private const int _MaxIntersectingWords = 2;
        private const int _MinHorizontalSpacing = 1;
        private const int _MinWordSpacing = 1; 

		private List<char> _LetterIndex = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        #endregion

        #region Properties

        // Crozzle Dependent
        public int MinNumberOfRows { get { return _MinNumberOfRows; } }
        public int MaxNumberOfRows { get { return _MaxNumberOfRows; } }
        public int MinNumberOfCols { get { return _MinNumberOfCols; } }
        public int MaxNumberOfCols { get { return _MaxNumberOfCols; } }
        public int MinNumberOfHorizontalWords { get { return _MinNumberOfHorizontalWords; } }
        public int MinNumberOfVerticalWords { get { return _MinNumberOfVerticalWords; } }
        public int MinNumberOfWords { get { return _MinNumberOfWords; } }
        public int MaxNumberOfWords { get { return _MaxNumberOfWords; } }
        
        public int MinIntersectingWords { get { return _MinIntersectingWords; } }
        public int MaxIntersectingWords { get { return _MaxIntersectingWords; } }
        public int MinWordSpacing { get { return _MinWordSpacing; } }

        // Configuration File Dependent
        private static int _GroupsPerCrozzleLimit;
        public int GroupsPerCrozzleLimit { get { return _GroupsPerCrozzleLimit; } set { _GroupsPerCrozzleLimit = value; } }
        private static int _PointsPerWord;
        public int PointsPerWord { get { return _PointsPerWord; } set { _PointsPerWord = value; } }
        private static int[] _NonIntersectingLetterPoints = new int[26];
        public int[] NonIntersectingLetterPoints { get { return _NonIntersectingLetterPoints; } set { _NonIntersectingLetterPoints = value; } }
        private static int[] _IntersectingLetterPoints = new int[26];
        public int[] IntersectingLetterPoints { get { return _IntersectingLetterPoints; } set { _IntersectingLetterPoints = value; } }

        // Keywords
        public string HorizontalKeyWord { get { return _HorizontalKeyWord; } }
        public string VerticalKeyWord { get { return _VerticalKeyWord; } }
        #endregion

        #region Set static property methods

        // Non-Intersecting letter points
        public int PointsForNonIntersecting(char letter)
        {
            return _NonIntersectingLetterPoints[_LetterIndex.IndexOf(letter)];
        }
        public void PointsForNonIntersecting(char letter, int points)
        {
            _NonIntersectingLetterPoints[_LetterIndex.IndexOf(letter)] = points;
        }

        // Intersecting letter points
        public int PointsForIntersecting(char letter)
        {
            return _IntersectingLetterPoints[_LetterIndex.IndexOf(letter)];
        }
        public void PointsForIntersecting(char letter, int points)
        {
            _IntersectingLetterPoints[_LetterIndex.IndexOf(letter)] = points;
        }

        #endregion
    }
}
