using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrozzleApplication.GenerateCrozzle
{
    /// <summary>
    /// Represents a word positioned on a Crozzle board.
    /// </summary>
    public class ActiveWord : Word
    {
        #region Properties

        private string _Orientation;
        /// <summary>
        /// Represents starting row position of the word on a Crozzle grid.
        /// </summary>
        public string Orientation { get { return _Orientation; } set { _Orientation = value; } }

        private int _RowStart;
        /// <summary>
        /// Represents start row position of the word on a Crozzle grid.
        /// </summary>
        public int RowStart { get { return _RowStart; } set { _RowStart = value; } }

        /// <summary>
        /// Represents the end row position of the word on a Crozzle grid.
        /// </summary>
        public int RowEnd { get { return CalcRowEnd(); } }

        /// <summary>
        /// Represents the end column position of the word on a Crozzle grid.
        /// </summary>
        public int ColEnd { get { return CalcColEnd(); } }

        private int _ColStart;
        /// <summary>
        /// Represents starting column position of the word on a Crozzle grid.
        /// </summary>
        public int ColStart { get { return _ColStart; } set { _ColStart = value; } }

        private int _ActiveScore;
        /// <summary>
        /// The real score of the word on the Crozzle board.
        /// </summary>
        public int ActiveScore { get { return _ActiveScore; } set { _ActiveScore = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constucts a word without any Crozzle position properties.
        /// </summary>
        /// <param name="word"></param>
        public ActiveWord(string word) : base(word)
        {
            _RowStart = 0;
            _ColStart = 0;
            _BaseScore = CalculateBaseScore();
        }

        /// <summary>
        /// Constructs a word that can be used on a Crozzle grid.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="orientation"></param>
        /// <param name="rowStart"></param>
        /// <param name="colStart"></param>
        public ActiveWord(string word, string orientation, int rowStart, int colStart) : base(word)
        {
            _Orientation = orientation;
            _RowStart = rowStart;
            _ColStart = colStart;
            _BaseScore = CalculateBaseScore();
            _ActiveScore = _BaseScore;
        }

        #endregion

        #region Methods: CalcRowEnd(), CalcColEnd()

        private int CalcRowEnd()
        {
            int result = RowStart;
            if (Orientation == Config.VerticalKeyWord)
                result += Length - 1;
            return result;
        }

        private int CalcColEnd()
        {
            int result = ColStart;
            if (Orientation == Config.HorizontalKeyWord)
                result += Length - 1;
            return result;
        }

        #endregion
    }
}