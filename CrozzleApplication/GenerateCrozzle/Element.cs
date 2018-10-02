using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCrozzle.CrozzleElements
{
    public class Element
    {
        ConfigRef Config = new ConfigRef();

        #region Properties

        private char _Letter;
        /// <summary>
        /// The Letter contained in the Crozzle grid elements.
        /// </summary>
        public char Letter { get { return _Letter; } }

        private ActiveWord _HorizontalWord;
        /// <summary>
        /// The horizontal word that element may be apart of. 
        /// </summary>
        public ActiveWord HorizontalWord { get { return _HorizontalWord; } }

        private int _HorizontalWordLetterIndex;
        /// <summary>
        /// The letter's index within the Horizontal word.
        /// </summary>
        public int HorizontalWordLetterIndex { get { return _HorizontalWordLetterIndex; } }

        private ActiveWord _VerticalWord;
        /// <summary>
        /// The vertical word that element may be apart of.
        /// </summary>
        public ActiveWord VerticalWord { get { return _VerticalWord; } }

        private int _VerticalWordLetterIndex;
        /// <summary>
        /// The letter's index within the vertical word.
        /// </summary>
        public int VerticalWordLetterIndex { get { return _VerticalWordLetterIndex; } }

        private int _Group;
        /// <summary>
        /// The word group the element is apart of.
        /// </summary>
        public int Group { get { return _Group; } set { _Group = value; } }

        private int _Score;
        /// <summary>
        /// The elements individual score.
        /// </summary>
        public int Score { get { return _Score; } set { _Group = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// A new nullified Crozzle grid element.
        /// </summary>
        public Element()
        {
            _Letter = ' ';
            _HorizontalWord = null;
            _HorizontalWordLetterIndex = -1;
            _VerticalWord = null;
            _VerticalWordLetterIndex = -1;
            _Group = 0;
            _Score = 0;
        }

        /// <summary>
        /// A new Crozzle grid element (non-intersecting).
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="word"></param>
        /// <param name="word letter index"></param>
        /// <param name="group"></param>
        public Element(char letter, ActiveWord word, int word_letterIndex, int group)
        {
            _Letter = letter;
            if(word.Orientation == Config.HorizontalKeyWord)
            {
                _HorizontalWord = word;
                _HorizontalWordLetterIndex = word_letterIndex;
                _VerticalWord = null;
                _VerticalWordLetterIndex = -1;
            }
            else
            {
                _HorizontalWord = null;
                _HorizontalWordLetterIndex = -1;
                _VerticalWord = word;
                _VerticalWordLetterIndex = word_letterIndex;
            }
            _Group = group;
            _Score = CalcScore();
        }

        /// <summary>
        /// A new Crozzle grid element (intersecting)
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="horizontal word"></param>
        /// <param name="horizontal word letter index"></param>
        /// <param name="vertical word"></param>
        /// <param name="vertical word letter index"></param>
        /// <param name="group"></param>
        public Element(char letter, ActiveWord horizontalWord, int horizontalWord_letterIndex, ActiveWord verticalWord, int verticalWord_letterIndex, int group)
        {
            _Letter = letter;
            _HorizontalWord = horizontalWord;
            _HorizontalWordLetterIndex = horizontalWord_letterIndex;
            _VerticalWord = verticalWord;
            _VerticalWordLetterIndex = verticalWord_letterIndex;
            _Group = group;
            _Score = CalcScore();
        }

        #endregion

        #region Methods: CalcScore(), ToString()

        // Calculates the score for the element.
        private int CalcScore()
        {
            int score = 0;

            // If it's an intersecting element
            if(_HorizontalWord != null && _VerticalWord != null)
            {
                score = Config.PointsForIntersecting(_Letter);
            }

            // Else if it's a non-intersectin element
            else if (_HorizontalWord != null || _VerticalWord != null)
            {
                score = Config.PointsForNonIntersecting(_Letter);
            }

            // Return the score.
            return score;
        }

        public override string ToString()
        {
            return _Letter.ToString();
        }
        #endregion
    }
}
