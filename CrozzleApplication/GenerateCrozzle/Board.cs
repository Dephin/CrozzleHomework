using System;
using System.Collections.Generic;

namespace CrozzleApplication.GenerateCrozzle
{
    /// <summary>
    /// A crozzle board grid.
    /// </summary>
    public class Board
    {
        public static ConfigRef Config = new ConfigRef();

        #region Properties

        protected Element[,] _BoardGrid;
        public Element[,] BoardGrid { get { return _BoardGrid; } set { _BoardGrid = value; } }

        protected int _Rows;
        public int Rows { get { return _Rows; } }

        protected int _Cols;
        public int Cols { get { return _Cols; } }

        protected List<ActiveWord> _ActiveWordsList;
        public List<ActiveWord> ActiveWordList { get { return _ActiveWordsList; } }

        protected int _GroupCount;
        public int GroupCount { get { return _GroupCount; } }

        #endregion

        #region Constructors
        public Board(int rows, int cols)
        {
            _BoardGrid = new Element[rows+1, cols+1];
            _ActiveWordsList = new List<ActiveWord>();
            _Rows = rows;
            _Cols = cols;
            _GroupCount = 0;
        }

        public Element this[int row, int col]
        {
            get { return _BoardGrid[row,col]; }
            set { _BoardGrid[row, col] = value; }
        }

        #endregion

        #region Methods: AddWordlist(), AddWord(), ElementIn(), SetElementIn()
        public void AddWordList(List<ActiveWord> wordUsedList)
        {
            foreach (ActiveWord word in wordUsedList)
                AddWord(word);
        }
        public void AddWord(ActiveWord word)
        {
            int row_index = word.RowStart;
            int col_index = word.ColStart;
            int group = ++_GroupCount;

            for (int letterIndex = 0; letterIndex < word.Length; letterIndex++)
            {
                if(_BoardGrid[row_index, col_index] == null)
                {
                    _BoardGrid[row_index, col_index] = new Element(word[letterIndex], word, letterIndex, group);
                }
                else 
                {
                    if(_BoardGrid[row_index, col_index].Letter != word[letterIndex])
                    {
                        throw new Exception("Cannot add word " + word + " as the letter " + word[letterIndex] + " cannot be placed onto the letter " + _BoardGrid[row_index, col_index].Letter + " at [" + row_index + "," + col_index + "]");
                    }
                    
                    if ((word.Orientation == Config.HorizontalKeyWord && _BoardGrid[row_index, col_index].HorizontalWord != null) || (word.Orientation == Config.VerticalKeyWord && _BoardGrid[row_index, col_index].VerticalWord != null))
                    {
                        throw new Exception("Cannot add word " + word + " as it is overlapping another " + word.Orientation + " word.");
                    }
                    
                    if (_BoardGrid[row_index, col_index].Group != group)
                        group = CombineGroups(_BoardGrid[row_index, col_index].Group, group);

                    if(word.Orientation == Config.HorizontalKeyWord)
                        _BoardGrid[row_index, col_index] = new Element(word[letterIndex], word, letterIndex, _BoardGrid[row_index, col_index].VerticalWord, _BoardGrid[row_index, col_index].VerticalWordLetterIndex, group);
                    else
                        _BoardGrid[row_index, col_index] = new Element(word[letterIndex], _BoardGrid[row_index, col_index].HorizontalWord, _BoardGrid[row_index, col_index].HorizontalWordLetterIndex, word, letterIndex, group);
                }
                if (word.Orientation == Config.HorizontalKeyWord)
                    col_index++;
                else
                    row_index++;
            }
            _ActiveWordsList.Add(word);
        }

        public int CombineGroups(int group1, int group2)
        {
            int group = Math.Min(group1, group2);
            int oldgroup = Math.Max(group1, group2);

            foreach (Element element in _BoardGrid)
            {
                if(element != null)
                {
                    if (element.Group == oldgroup)
                        element.Group = group;
                    if (element.Group > oldgroup)
                        element.Group--;
                }
            }
            _GroupCount--;
            return group;
        }

        public Element ElementIn(ActiveWord word, int index)
        {
            int row = word.RowStart;
            int col = word.ColStart;
            if (word.Orientation == Config.HorizontalKeyWord)
                col += index;
            else
                row += index;

            return _BoardGrid[row, col];
        }
        #endregion

    }
}
