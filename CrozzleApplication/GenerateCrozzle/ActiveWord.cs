namespace CrozzleApplication.GenerateCrozzle
{
    public class ActiveWord : Word
    {
        #region Properties

        private string _Orientation;
        public string Orientation { get { return _Orientation; } set { _Orientation = value; } }

        private int _RowStart;
        public int RowStart { get { return _RowStart; } set { _RowStart = value; } }

        public int RowEnd { get { return CalcRowEnd(); } }
        public int ColEnd { get { return CalcColEnd(); } }

        private int _ColStart;
        public int ColStart { get { return _ColStart; } set { _ColStart = value; } }

        private int _ActiveScore;
        public int ActiveScore { get { return _ActiveScore; } set { _ActiveScore = value; } }

        #endregion

        #region Constructors

        public ActiveWord(string word) : base(word)
        {
            _RowStart = 0;
            _ColStart = 0;
            _BaseScore = CalculateBaseScore();
        }

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