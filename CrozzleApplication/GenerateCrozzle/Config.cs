using System.Collections.Generic;

namespace CrozzleApplication.GenerateCrozzle
{
    public class Config
    {
        #region Constants
        private const string _HorizontalKeyWord = "HORIZONTAL";
        private const string _VerticalKeyWord = "VERTICAL";
        private const int _MinIntersectingWords = 1;
        private const int _MaxIntersectingWords = 100;
        public string HorizontalKeyWord { get { return _HorizontalKeyWord; } }
        public string VerticalKeyWord { get { return _VerticalKeyWord; } }
        public int MaxIntersectingWords { get { return _MaxIntersectingWords; } }
        public int MinIntersectingWords { get { return _MinIntersectingWords; } }
        #endregion

        #region Properties1
        private int _MinNumberOfRows;
        private int _MaxNumberOfRows;
        private int _MinNumberOfCols;
        private int _MaxNumberOfCols;
        public int MinNumberOfRows { get { return _MinNumberOfRows; } set { _MinNumberOfRows=value; }}
        public int MaxNumberOfRows { get { return _MaxNumberOfRows; } set { _MaxNumberOfRows=value; }}
        public int MinNumberOfCols { get { return _MinNumberOfCols; } set { _MinNumberOfCols=value; }}
        public int MaxNumberOfCols { get { return _MaxNumberOfCols; } set { _MaxNumberOfCols=value; }}
        #endregion

        #region Properties2
        private int _PointsPerWord;
        private static int[] _NonIntersectingLetterPoints;
        private static int[] _IntersectingLetterPoints;
        private static int _GroupsPerCrozzleLimit;
        public int PointsPerWord { get { return _PointsPerWord; } set { _PointsPerWord = value; } }
        public static int[] NonIntersectingLetterPoints { get { return _NonIntersectingLetterPoints; } set { _NonIntersectingLetterPoints = value; }}
        public static int[] IntersectingLetterPoints { get { return _IntersectingLetterPoints; } set { _IntersectingLetterPoints = value; }}
        public int GroupsPerCrozzleLimit { get { return _GroupsPerCrozzleLimit; } set { _GroupsPerCrozzleLimit = value; } }
        #endregion

        #region letter points
        private List<char> _LetterIndex = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
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
