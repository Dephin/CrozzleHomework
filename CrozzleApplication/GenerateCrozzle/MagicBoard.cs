using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CrozzleApplication.GenerateCrozzle
{
    /// <summary>
    /// A crozzle board with a dynamic Grid. Allows word groups to shift arount to best suit the board.
    /// </summary>
    public class MagicBoard : Board
    {
        private Config Config = new Config();

        #region Magic Properties

        private int minRow;
        private int maxRow;

        private int minCol;
        private int maxCol;


        public new List<ActiveWord> ActiveWordList { get { return BuildActiveWordList(); } }

        #endregion

        #region Constructors 

        public MagicBoard(int rows, int cols) : base(rows, cols) 
        {
            _BoardGrid = new Element[rows * 3 + 1, cols * 3 + 1];
            minRow = rows;
            maxRow = rows;
            minCol = cols;
            maxCol = cols;
        }

        public new Element this[int row, int col]
        {
            get { return _BoardGrid[row + minRow - 1, col + minCol -1]; }
            set { _BoardGrid[row + minRow - 1, col + minCol - 1] = value; }
        }

        #endregion

        #region Best Word Methods: GetBestWord(), TryAddWord(), IsSameWord(), WordIntersectionCount()

        public List<ActiveWord> GetBestWord(List<Word> wordlist)
        {           
            List<ActiveWord> BestWords = new List<ActiveWord>(); // The best Word Group          
            List<List<ActiveWord>> Possibilities = new List<List<ActiveWord>>();  // A list of word groups that could be added
            int TotalElements = 0;

            List<Word> SortedWordlist = new List<Word>();
            foreach (Word word in wordlist)
            {
                SortedWordlist.Add(new Word(word.String));
            }

            SortedWordlist = SortByScore(SortedWordlist);

            for (int rowIndex = minRow; rowIndex <= maxRow; rowIndex++)
            {
                for (int colIndex = minCol; colIndex <= maxCol; colIndex++)
                {
                    Element element = _BoardGrid[rowIndex, colIndex];
                    bool newGroup = false;

                    if (element == null && _GroupCount >= Config.GroupsPerCrozzleLimit)
                        continue;

                    if (element != null)
                    {
                        TotalElements++;

                        if (element.HorizontalWord != null && element.VerticalWord != null)
                            continue;
                    }
                        
                    Regex letter = new Regex(@"^\w");
                    if (element != null)
                        letter = new Regex(element.Letter.ToString());

                    foreach (Word word in SortedWordlist)
                    {
                        foreach (Match letterIndex in letter.Matches(word.String))
                        {
                            List<ActiveWord> WordCombo;

                            WordCombo = new List<ActiveWord>();
                            int RowStart_H = rowIndex;
                            int ColStart_H = colIndex - letterIndex.Index;
                            WordCombo.Add(word.MakeActiveWord(RowStart_H, ColStart_H, Config.HorizontalKeyWord));

                            // add vertical
                            if (TryAddWord(ref WordCombo))
                            {
                                if (element == null)
                                {  
                                    int rowIndex2 = rowIndex;
                                    int colIndex2 = colIndex;

                                    foreach (Char element2 in word.String)
                                    {
                                        Regex letter2 = new Regex(element2.ToString());
                                        foreach (Word word2 in SortedWordlist)
                                        {
                                            if (word2.String == word.String)
                                                continue;

                                            foreach (Match letter2Index in letter2.Matches(word2.String))
                                            {
                                                List<ActiveWord> WordCombo2 = new List<ActiveWord>();
                                                int RowStart2_V = rowIndex2 - letter2Index.Index;
                                                int ColStart2_V = colIndex2;
                                                WordCombo2.Add(word2.MakeActiveWord(RowStart2_V, ColStart2_V, Config.VerticalKeyWord));
                                                
                                                if (TryAddWord(ref WordCombo2))
                                                {
                                                    WordCombo.Add(WordCombo2[0]);
                                                    Possibilities.Add(WordCombo);
                                                    newGroup = true;
                                                    break;
                                                }
                                            }
                                            if (newGroup == true)
                                                break;
                                        }
                                        if (newGroup == false)
                                            colIndex2++;
                                        else
                                            break;
                                    }
                                }
                                else
                                {
                                    Possibilities.Add(WordCombo);
                                }
                            }

                            //add vertically
                            WordCombo = new List<ActiveWord>();
                            int RowStart_V = rowIndex - letterIndex.Index;
                            int ColStart_V = colIndex;
                            WordCombo.Add(word.MakeActiveWord(RowStart_V, ColStart_V, Config.VerticalKeyWord));
                            if (TryAddWord(ref WordCombo))
                            {
                                if (element == null)
                                {
                                    int rowIndex2 = rowIndex;
                                    int colIndex2 = colIndex;

                                    foreach (Char element2 in word.String)
                                    {
                                        Regex letter2 = new Regex(element2.ToString());
                                        foreach (Word word2 in SortedWordlist)
                                        {
                                            if (word2.String == word.String)
                                                continue;

                                            foreach (Match letter2Index in letter2.Matches(word2.String))
                                            {
                                                List<ActiveWord> WordCombo2 = new List<ActiveWord>();
                                                int RowStart2_H = rowIndex2;
                                                int ColStart2_H = colIndex2 - letter2Index.Index;
                                                WordCombo2.Add(word2.MakeActiveWord(RowStart2_H, ColStart2_H, Config.HorizontalKeyWord));

                                                if (TryAddWord(ref WordCombo2))
                                                {
                                                    WordCombo.Add(WordCombo2[0]);
                                                    Possibilities.Add(WordCombo);
                                                    newGroup = true;
                                                    break;
                                                }
                                            }
                                            if (newGroup == true)
                                                break;
                                        }
                                        if (newGroup == false)
                                            rowIndex2++;
                                        else
                                            break;
                                    }
                                }
                                else
                                {
                                    Possibilities.Add(WordCombo);
                                }
                            }

                            if (newGroup == true)
                                break;
                        }
                        if (newGroup == true)
                            break;
                    }
                }
            }
            
            if (TotalElements == 0)
            {
                SortedWordlist = SortByScore(SortedWordlist);
                BestWords.Add(SortedWordlist[0].MakeActiveWord(_Rows+2,_Cols+2, Config.VerticalKeyWord));
            }
            else
            {
                if (Possibilities.Count > 0)
                {
                    Possibilities = SortByScore(Possibilities);

                    BestWords = Possibilities[0];
                }

            }
            
            return BestWords;
        }

        private bool TryAddWord(ref List<ActiveWord> testWord)
        {

            bool result = true;

            foreach (ActiveWord word in testWord)
            {
                int rowIndex = word.RowStart;
                int colIndex = word.ColStart;
                int wordIntersections = 0;

                for (int letterIndex = 0; letterIndex < word.Length; letterIndex++)
                {
                    int newHeight = Math.Max(maxRow, rowIndex) - (Math.Min(minRow, rowIndex) - 1);
                    int newWidth = Math.Max(maxCol, colIndex) - (Math.Min(minCol, colIndex) - 1);
                    if (newHeight > _Rows || newWidth > _Cols)
                    {
                        result = false;
                        break;
                    }

                    Element element = _BoardGrid[rowIndex, colIndex];
                    if (element != null)
                    {
                        if ((wordIntersections + 1) > Config.MaxIntersectingWords)
                        {
                            result = false;
                            break;
                        }
                        wordIntersections++;

                        if (element.HorizontalWord != null)
                        {
                            if (WordIntersectionCount(element.HorizontalWord) >= Config.MaxIntersectingWords)
                            {
                                result = false;
                                break;
                            }
                        }
                        else if (element.VerticalWord != null)
                        {
                            if (WordIntersectionCount(element.VerticalWord) >= Config.MaxIntersectingWords)
                            {
                                result = false;
                                break;
                            }
                        }


                        if (element.Letter != word[letterIndex] || (element.HorizontalWord != null && word.Orientation == Config.HorizontalKeyWord) || (element.VerticalWord != null && word.Orientation == Config.VerticalKeyWord))
                        {
                            result = false;
                            break;
                        }
                    }

                    word.ActiveScore -= Config.PointsForNonIntersecting(word[letterIndex]); // Subtract non-intersecting letter points
                    word.ActiveScore += Config.PointsForIntersecting(word[letterIndex]); // Add intersecting letter points

                    if (word.Orientation == Config.HorizontalKeyWord)
                        colIndex++;
                    else
                        rowIndex++;
                }

                if (result == true)
                {
                    rowIndex = word.RowStart;
                    colIndex = word.ColStart;

                    if (word.Orientation == Config.HorizontalKeyWord)
                        if (_BoardGrid[rowIndex, colIndex - 1] != null || _BoardGrid[rowIndex, word.ColEnd + 1] != null)
                            result = false;

                    if (word.Orientation == Config.VerticalKeyWord)
                        if (_BoardGrid[rowIndex - 1, colIndex] != null || _BoardGrid[word.RowEnd + 1, colIndex] != null)
                            result = false;

                    for (int letterIndex = 0; letterIndex < word.Length; letterIndex++)
                    {
                        if (word.Orientation == Config.HorizontalKeyWord)
                        {
                            Element element = _BoardGrid[rowIndex, colIndex + letterIndex];
                            Element elementAbove = _BoardGrid[rowIndex - 1, colIndex + letterIndex];
                            Element elementBelow = _BoardGrid[rowIndex + 1, colIndex + letterIndex];

                            if ((elementAbove != null && !IsSameWord(element, elementAbove)) || (elementBelow != null && !IsSameWord(element, elementBelow)))
                            {
                                result = false;
                                break;
                            }
                        }
                        else
                        {
                            Element element = _BoardGrid[rowIndex + letterIndex, colIndex];
                            Element elementLeft = _BoardGrid[rowIndex + letterIndex, colIndex - 1];
                            Element elementRight = _BoardGrid[rowIndex + letterIndex, colIndex + 1];

                            if ((elementLeft != null && !IsSameWord(element, elementLeft)) || (elementRight != null && !IsSameWord(element, elementRight)))
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }  
            
            return result;
        }

        private bool IsSameWord(Element element1, Element element2)
        {
            bool result = false;
            if(element1 != null && element2 != null)
            {
                // If same horizontal word
                if (element1.HorizontalWord != null && element2.HorizontalWord != null)
                    if (element1.HorizontalWord.String == element2.HorizontalWord.String)
                        result = true;
                // If same vertical word
                if (element1.VerticalWord != null && element2.VerticalWord != null)
                    if (element1.VerticalWord.String == element2.VerticalWord.String)
                        result = true;
            }
            return result;
        }


        private int WordIntersectionCount(ActiveWord word)
        {
            int count = 0;
            int rowIndex = word.RowStart;
            int colIndex = word.ColStart;

            for (int letterIndex = 0; letterIndex < word.Length; letterIndex++)
            {
                if (_BoardGrid[rowIndex, colIndex] != null)
                    if (_BoardGrid[rowIndex, colIndex].HorizontalWord != null && _BoardGrid[rowIndex, colIndex].VerticalWord != null)
                        count++;
                if (word.Orientation == Config.HorizontalKeyWord)
                    colIndex++;
                else
                    rowIndex++;
            }
            return count;
        }

        #endregion

        #region Sorting Methods: SortByScore()


        private List<List<ActiveWord>> SortByScore(List<List<ActiveWord>> wordList)
        {
            List<List<ActiveWord>> tempList = new List<List<ActiveWord>>();
            while(wordList.Count > 0)
            {
                int topWordIndex = 0;
                int topWordScore = 0;
                int topWordElements = 0;
                for (int index = 0; index < wordList.Count; index++)
                {
                    int score = 0;
                    int elements = 0;
                    foreach(ActiveWord word in wordList[index])
                    {
                        score += word.ActiveScore;
                        elements += word.Length;
                    }
                    if (score > topWordScore)
                    {
                        topWordIndex = index;
                        topWordScore = score;
                        topWordElements = elements;
                    }
                    if (score == topWordScore && elements <= topWordElements)
                        topWordIndex = index;
                }
                tempList.Add(wordList[topWordIndex]);
                wordList.RemoveAt(topWordIndex);
            }
            return tempList;
        }


        private List<Word> SortByScore(List<Word> wordList)
        {
            List<Word> tempList = new List<Word>();
            while (wordList.Count > 0)
            {
                int topWordIndex = 0;
                for (int index = 0; index < wordList.Count; index++)
                {
                    if (wordList[index].BaseScore  > wordList[topWordIndex].BaseScore)
                        topWordIndex = index;
                    if (wordList[index].BaseScore == wordList[topWordIndex].BaseScore && wordList[index].Length < wordList[topWordIndex].Length)
                        topWordIndex = index;
                }
                tempList.Add(wordList[topWordIndex]);
                wordList.RemoveAt(topWordIndex);
            }
            return tempList;
        }

        #endregion

        #region Methods: LoseTheMagic(), AddMagicWord() RowCount(), ColCount(), BuildActiveWordList()

        public Board LoseTheMagic()
        {
            Board board = new Board(_Rows, _Cols);
            board.AddWordList(ActiveWordList);
            return board;
        }

        public void AddMagicWord(ActiveWord word)
        {
            AddWord(word);
            minRow = Math.Min(word.RowStart, minRow);
            maxRow = Math.Max(word.RowEnd, maxRow);
            minCol = Math.Min(word.ColStart, minCol);
            maxCol = Math.Max(word.ColEnd, maxCol);
        }

        private int RowCount()
        {
            return maxRow - (minRow - 1);
        }
        private int ColCount()
        {
            return maxCol - (minCol - 1);
        }

        private List<ActiveWord> BuildActiveWordList()
        {
            List<ActiveWord> newList = new List<ActiveWord>();
            foreach (ActiveWord word in _ActiveWordsList)
            {
                newList.Add(new ActiveWord(word.String, word.Orientation, word.RowStart - minRow + 1, word.ColStart - minCol + 1));
            }
            return newList;
        }

        #endregion
    }
}