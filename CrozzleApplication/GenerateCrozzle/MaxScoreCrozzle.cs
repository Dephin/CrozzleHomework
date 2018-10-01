using System;
using System.Collections.Generic;
using System.Timers;

namespace CrozzleApplication.GenerateCrozzle
{
    public class MaxScoreCrozzle
    {
        private ConfigRef Config = new ConfigRef();

        #region Constants
        private const int MaxBuildRunTime = 400000;
        #endregion

        #region Properties

        private Board _Grid;
        public Board Grid
        {
            get { return _Grid; }
            set { _Grid = value; }
        }

        public int Rows
        {
            get { return _Grid.Rows; }
        }

        public int Cols
        {
            get { return _Grid.Cols; }
        }

        private List<Word> _WordList;

        public List<Word> Wordlist
        {
            get { return _WordList; }
            set { _WordList = value; }
        }

        private List<ActiveWord> _ActiveWordList;

        public List<ActiveWord> ActiveWordList
        {
            get { return _ActiveWordList; }
        }

        public int Score
        {
            get { return CalcScore(); }
        }

        public int GroupCount
        {
            get { return _Grid.GroupCount; }
        }

        protected bool _ValidationResult;
        public bool ValidationResult
        {
            get { return _ValidationResult; }
        }

        protected List<string> _ValidationErrorList;

        public List<string> ValidationErrorList
        {
            get { return _ValidationErrorList; }
        }

        private Timer timer;

        #endregion

        #region Constructors

        public MaxScoreCrozzle(Configuration configuration, WordList wordList)
        {
            ConstructConfig(configuration);
            Word.Config = Config;
            Board.Config = Config;
            MagicBoard.Config = Config;

            _WordList = new List<Word>();
            foreach (String word in wordList.List)
                _WordList.Add(new Word(word));
            _ActiveWordList = new List<ActiveWord>();

            Board board = new Board(configuration.MaximumNumberOfRows, configuration.MaximumNumberOfColumns);
            board.AddWordList(_ActiveWordList);


            _Grid = board;
            _Grid = new Board(Config.MinNumberOfRows, Config.MinNumberOfRows);
            _WordList = new List<Word>();
            _ActiveWordList = new List<ActiveWord>();
            _WordList = new List<Word>();
            _ActiveWordList = new List<ActiveWord>();
        }

        public void ConstructConfig(Configuration configuration)
        {
            Config.MinNumberOfRows = configuration.MinimumNumberOfRows;
            Config.MaxNumberOfRows = configuration.MaximumNumberOfRows;
            Config.MinNumberOfCols = configuration.MinimumNumberOfColumns;
            Config.MaxNumberOfCols = configuration.MaximumNumberOfColumns;
            Config.PointsPerWord = configuration.PointsPerWord;
            Config.NonIntersectingLetterPoints = configuration.NonIntersectingPointsPerLetter;
            Config.IntersectingLetterPoints = configuration.IntersectingPointsPerLetter;
            Config.GroupsPerCrozzleLimit = configuration.MaximumNumberOfGroups;
        }

        public Element this[int row, int col]
        {
            get { return _Grid[row, col]; }
            set { _Grid[row, col] = value; }
        }
        #endregion

        #region Generate Optimal
        public void GenerateOptimal()
        {
            bool status = false;

            timer = new Timer(MaxBuildRunTime);

            timer.Start();

            List<Word> Words = new List<Word>();
            foreach (Word word in _WordList)
                Words.Add(new Word(word.String));
            int wordsAdded = 0;
            MagicBoard Grid = new MagicBoard(_Grid.Rows, _Grid.Cols); // Dynamic Crozzle board

            while (timer.Enabled && status == false)
            {
                List<ActiveWord> BestWords;
                while ((BestWords = Grid.GetBestWord(Words)).Count > 0)
                {
                    foreach (ActiveWord BestWord in BestWords)
                    {
                        Grid.AddMagicWord(BestWord);

                        for (int index = 0; index < Words.Count; index++)
                            if (Words[index].String == BestWord.String)
                                Words.RemoveAt(index);

                        wordsAdded++;
                    }
                }
                status = true;
            }

            _Grid = Grid.LoseTheMagic();
            _ActiveWordList = _Grid.ActiveWordList;
        }
        #endregion

        #region Methods: CalcScore()
        private int CalcScore()
        {
            int score = 0;

            foreach (Element letter in _Grid.BoardGrid)
                if (letter != null)
                    score += letter.Score;

            score += _Grid.ActiveWordList.Count * Config.PointsPerWord;

            return score;
        }
        #endregion
    }
}