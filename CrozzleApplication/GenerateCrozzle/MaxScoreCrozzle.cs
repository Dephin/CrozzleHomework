using System;
using System.Collections.Generic;
using System.Timers;

namespace CrozzleApplication.GenerateCrozzle
{
    public class MaxScoreCrozzle
    {
        Configuration Config;

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
            Config = configuration;
            Board board = new Board(Config.MaximumNumberOfRows, Config.MaximumNumberOfColumns);
            _Grid = board;

            _WordList = new List<Word>();
            foreach (String wordStr in wordList.List)
                _WordList.Add(new Word(wordStr));
        }

        public Element this[int row, int col]
        {
            get { return _Grid[row, col]; }
            set { _Grid[row, col] = value; }
        }
        #endregion

        #region Validation
        public bool Validate()
        {
            bool result = true;

            _ValidationErrorList = new List<string>();
            _ValidationResult = true;

            if (Validate_NoDuplicates() != true)
                result = false;

//            if (Validate_MaxGroups() != true)
//                result = false;

            if (Validate_Intersections() != true)
                result = false;

            _ValidationResult = result;
            return result;
        }

        private bool Validate_NoDuplicates()
        {
            bool result = true;
            foreach (Word word in _WordList)
            {
                int wordCount = 0;
                foreach (ActiveWord listword in _ActiveWordList)
                {
                    if (word.String == listword.String)
                        wordCount++;
                }

                if (wordCount > 1)
                {
                    _ValidationErrorList.Add("The word " + word + " was used more than once.");
                    result = false;
                }
            }

            return result;
        }

//        private bool Validate_MaxGroups()
//        {
//            bool result = true;
//            if (_Grid.GroupCount > Config.GroupCount)
//            {
//                _ValidationErrorList.Add("The number of word groups (" + _Grid.GroupCount +
//                                         ") exceeds the groups per Crozzle limit of " + Config.GroupsPerCrozzleLimit +
//                                         ".");
//                result = false;
//            }
//
//            return result;
//        }
        private bool Validate_Intersections()
        {
            bool result = true;
            foreach (ActiveWord word in _Grid.ActiveWordList)
            {
                int horizontalIntersectionCount = 0;
                for (int letterIndex = 0; letterIndex < word.Length; letterIndex++)
                {
                    if (_Grid.ElementIn(word, letterIndex).HorizontalWord != null)
                        horizontalIntersectionCount++;
                }
                if (horizontalIntersectionCount < Config.MinimumHorizontalWords || horizontalIntersectionCount > Config.MaximumHorizontalWords)
                    return false;

                int verticaleIntersectionCount = 0;
                for (int letterIndex = 0; letterIndex < word.Length; letterIndex++)
                {
                    if (_Grid.ElementIn(word, letterIndex).VerticalWord != null)
                        verticaleIntersectionCount++;
                }
                if (verticaleIntersectionCount < Config.MinimumVerticalWords || verticaleIntersectionCount > Config.MaximumVerticalWords)
                    return false;
            }

            return result;
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