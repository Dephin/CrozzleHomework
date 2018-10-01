using System;

namespace CrozzleApplication.GenerateCrozzle
{
    public class Word
    {
        protected Config Config = new Config();

        #region Properties
        protected string _String;
        public string String { get { return _String; } set { _String = value; } }

        protected int _BaseScore;
        public int BaseScore { get { return _BaseScore; } }

        public int Length { get { return _String.Length; } }

        #endregion

        #region Constructors
        public Word(string word)
        {
            _String = word;
            _BaseScore = CalculateBaseScore();
        }

        public char this[int letterIndex]
        {
            get { return _String[letterIndex]; }
        }
        #endregion

        #region Methods - ToString(), CalculateBaseScore()
        public override string ToString()
        {
            return _String;
        }

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

        public ActiveWord MakeActiveWord(int rowStart, int colStart, string orientation)
        {
            return new ActiveWord(_String, orientation, rowStart, colStart);
        }
        #endregion
    }
}
