using System;
using System.Collections.Generic;

namespace CrozzleApplication.GenerateCrozzle
{
    /// <summary>
    /// A crozzle board grid.
    /// </summary>
    public class Board
    {
        ConfigRef Config = new ConfigRef();

        #region Properties

        protected Element[,] _BoardGrid;
        /// <summary>
        /// The grid board.
        /// </summary>
        public Element[,] BoardGrid { get { return _BoardGrid; } set { _BoardGrid = value; } }

        protected int _Rows;
        /// <summary>
        /// The number of rows in the crozzle grid. 
        /// </summary>
        public int Rows { get { return _Rows; } }

        protected int _Cols;
        /// <summary>
        /// The number of rows in the crozzle grid.
        /// </summary>
        public int Cols { get { return _Cols; } }

        protected List<ActiveWord> _ActiveWordsList;
        /// <summary>
        /// The words that that are positioned on the crozzle grid.
        /// </summary>
        public List<ActiveWord> ActiveWordList { get { return _ActiveWordsList; } }

        protected int _GroupCount;
        /// <summary>
        /// The number of groups on the crozzle board.
        /// </summary>
        public int GroupCount { get { return _GroupCount; } }

        #endregion

        #region Constructors 

        /// <summary>
        /// Creates a board grid for a Crozzle. 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public Board(int rows, int cols)
        {
            _BoardGrid = new Element[rows+1, cols+1];
            _ActiveWordsList = new List<ActiveWord>();
            _Rows = rows;
            _Cols = cols;
            _GroupCount = 0;
        }

        /// <summary>
        /// Direct access to the Crozzle grid.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public Element this[int row, int col]
        {
            get { return _BoardGrid[row,col]; }
            set { _BoardGrid[row, col] = value; }
        }

        #endregion

        #region Methods: AddWordlist(), AddWord(), ElementIn(), SetElementIn()

        /// <summary>
        /// Add each word in a word list to the Crozzle grid.
        /// </summary>
        /// <param name="wordUsedList"></param>
        public void AddWordList(List<ActiveWord> wordUsedList)
        {
            foreach (ActiveWord word in wordUsedList)
                AddWord(word);
        }

        /// <summary>
        /// Add a word to the Crozzle grid.
        /// </summary>
        /// <param name="word"></param>
        public void AddWord(ActiveWord word)
        {
            int row_index = word.RowStart;
            int col_index = word.ColStart;
            int group = ++_GroupCount;

            // Add each letter as an element
            for (int letterIndex = 0; letterIndex < word.Length; letterIndex++)
            {
                // If no element in grid square add a new one 
                if(_BoardGrid[row_index, col_index] == null)
                {
                    _BoardGrid[row_index, col_index] = new Element(word[letterIndex], word, letterIndex, group);
                }
                // Else add intersecting element
                else 
                {
                    // Check letters are the same
                    if(_BoardGrid[row_index, col_index].Letter != word[letterIndex])
                    {

                        throw new Exception("Cannot add word " + word + " as the letter " + word[letterIndex] + " cannot be placed onto the letter " + _BoardGrid[row_index, col_index].Letter + " at [" + row_index + "," + col_index + "]");
                    }
                    
                    // Check not overlapping word of same orientation
                    if ((word.Orientation == Config.HorizontalKeyWord && _BoardGrid[row_index, col_index].HorizontalWord != null) || (word.Orientation == Config.VerticalKeyWord && _BoardGrid[row_index, col_index].VerticalWord != null))
                    {

                        throw new Exception("Cannot add word " + word + " as it is overlapping another " + word.Orientation + " word.");
                    }
                    
                    // Check if groups can be combined
                    if (_BoardGrid[row_index, col_index].Group != group)
                        group = CombineGroups(_BoardGrid[row_index, col_index].Group, group);

                    // Add the element to the grid
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
            // Add the word to the words used list
            _ActiveWordsList.Add(word);
        }

        /// <summary>
        /// Combine two groups on a Crozzle grid.
        /// </summary>
        /// <param name="group1"></param>
        /// <param name="group2"></param>
        /// <returns></returns>
        public int CombineGroups(int group1, int group2)
        {
            // Group used will be the smaller int
            int group = Math.Min(group1, group2);
            int oldgroup = Math.Max(group1, group2);

            // For each element of old group update it to the group
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
            
            // Take 1 from the group count 
            _GroupCount--;

            // Return the group used
            return group;
        }

        /// <summary>
        /// REturns the element a word at a specified index.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the element in a word to a new element. 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="index"></param>
        /// <param name="element"></param>
        public void SetElementIn(ActiveWord word, int index, Element element)
        {
            int row = word.RowStart;
            int col = word.ColStart;
            if (word.Orientation == Config.HorizontalKeyWord)
                col += index;
            else
                row += index;

            _BoardGrid[row, col] = element;
        }

        #endregion

    }
}
