using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCrozzle.CrozzleElements
{
    /// <summary>
    /// Represents a word avalible for use on a Crozzle board.
    /// </summary>
    public class Word
    {
        protected ConfigRef Config = new ConfigRef();

        #region Properties
        protected string _String;
        /// <summary>
        /// Represents the letters of the word.
        /// </summary>
        public string String { get { return _String; } set { _String = value; } }

        protected int _BaseScore;
        /// <summary>
        /// The default score for a word given the current configuation.
        /// </summary>
        public int BaseScore { get { return _BaseScore; } }

        /// <summary>
        /// Return the number of letters in the word.
        /// </summary>
        public int Length { get { return _String.Length; } }

        #endregion

        #region Constructors
        /// <summary>
        /// Constucts a word without any Crozzle position properties.
        /// </summary>
        /// <param name="word"></param>
        public Word(string word)
        {
            _String = word;
            _BaseScore = CalculateBaseScore();
        }

        /// <summary>
        /// Direct access to letter values.
        /// </summary>
        /// <param name="letterIndex"></param>
        /// <returns>Returns the letter at the index.</returns>
        public char this[int letterIndex]
        {
            get { return _String[letterIndex]; }
        }
        #endregion

        #region Methods - ToString(), CalculateBaseScore()
        /// <summary>
        /// The word represented as a string.
        /// </summary>
        /// <returns>Returns the word as a string.</returns>
        public override string ToString()
        {
            return _String;
        }

        /// <summary>
        /// Calculates the word score given all letters are non intersecting.
        /// </summary>
        /// <returns></returns>
        protected int CalculateBaseScore()
        {
            int points = 0;
            foreach(char letter in _String)
            {
                points += Config.PointsForNonIntersecting(letter);
                points += (int)Math.Round((double)(Config.PointsForIntersecting(letter) / 2));
            }
            return points;
        }

        /// <summary>
        /// Converts the word to an Active word by assigning its location on a Crozzle grid.
        /// </summary>
        /// <param name="rowStart"></param>
        /// <param name="colStart"></param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public ActiveWord MakeActiveWord(int rowStart, int colStart, string orientation)
        {
            return new ActiveWord(_String, orientation, rowStart, colStart);
        }
        #endregion
    }
}
