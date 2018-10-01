using System.Collections.Generic;

namespace CrozzleApplication.GenerateCrozzle
{
    public class ConfigRef
    {
        #region readonly

        public static readonly string HorizontalKeyWord = "HORIZONTAL";
        public static readonly string VerticalKeyWord = "VERTICAL";

        #endregion

        #region Properties

        public int MinIntersectingWords;
        public int MaxIntersectingWords;
        public int MinNumberOfRows;
        public int MaxNumberOfRows;
        public int MinNumberOfCols;
        public int MaxNumberOfCols;
        public int PointsPerWord;
        public int[] NonIntersectingLetterPoints;
        public int[] IntersectingLetterPoints;
        public int GroupsPerCrozzleLimit;

        #endregion

        #region letter points

        private List<char> _LetterIndex = new List<char>()
        {
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z'
        };

        // Non-Intersecting letter points
        public int PointsForNonIntersecting(char letter)
        {
            return NonIntersectingLetterPoints[_LetterIndex.IndexOf(letter)];
        }

        public void PointsForNonIntersecting(char letter, int points)
        {
            NonIntersectingLetterPoints[_LetterIndex.IndexOf(letter)] = points;
        }

        // Intersecting letter points
        public int PointsForIntersecting(char letter)
        {
            return IntersectingLetterPoints[_LetterIndex.IndexOf(letter)];
        }

        public void PointsForIntersecting(char letter, int points)
        {
            IntersectingLetterPoints[_LetterIndex.IndexOf(letter)] = points;
        }

        #endregion
    }
}