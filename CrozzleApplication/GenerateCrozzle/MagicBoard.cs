using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crozzle2.CrozzleElements
{
    /// <summary>
    /// A crozzle board with a dynamic Grid. Allows word groups to shift arount to best suit the board.
    /// </summary>
    public class MagicBoard : Board
    {
        private ConfigRef Config = new ConfigRef();

        #region Magic Properties

        private int realRows { get { return _BoardGrid.GetLength(0); } }
        private int height { get { return RowCount(); } }
        private int minRow;
        private int maxRow;

        private int realCols { get { return _BoardGrid.GetLength(1); } }
        private int width { get { return ColCount(); } }
        private int minCol;
        private int maxCol;

        /// <summary>
        /// The words that that are positioned on the crozzle grid.
        /// </summary>
        public new List<ActiveWord> ActiveWordList { get { return BuildActiveWordList(); } }

        #endregion

        #region Constructors 

        public MagicBoard(int rows, int cols) : base(rows, cols) 
        {
            // Create a board with a margin around it equal to the board size.
            // Essentually this is a board 3 times the size;
            _BoardGrid = new Element[rows * 3 + 1, cols * 3 + 1];
            // Specifies the first element of the center board.
            minRow = rows + 2;
            maxRow = rows + 2;
            minCol = cols + 2;
            maxCol = cols + 2;
        }

        public new Element this[int row, int col]
        {
            get { return _BoardGrid[row + minRow - 1, col + minCol -1]; }
            set { _BoardGrid[row + minRow - 1, col + minCol - 1] = value; }
        }

        #endregion

        #region Best Word Methods: GetBestWord(), TryAddWord(), IsSameWord(), WordIntersectionCount()

        /// <summary>
        /// Gets the best word that will fit on a Crozzle board grid from a list of words.
        /// </summary>
        /// <param name="Words list"></param>
        /// <returns></returns>
        public List<ActiveWord> GetBestWord(List<Word> wordlist)
        {           
            List<ActiveWord> BestWords = new List<ActiveWord>(); // The best Word Group          
            List<List<ActiveWord>> Possibilities = new List<List<ActiveWord>>();  // A list of word groups that could be added
            int TotalElements = 0;


            // Create deep copy of word list
            List<Word> SortedWordlist = new List<Word>();
            foreach (Word word in wordlist)
            {
                SortedWordlist.Add(new Word(word.String));
            }
            // Sort Wordlist  
            SortedWordlist = SortByScore(SortedWordlist);

            // For each row and each col in the grid
            for (int rowIndex = minRow; rowIndex <= maxRow; rowIndex++)
            {
                for (int colIndex = minCol; colIndex <= maxCol; colIndex++)
                {
                    // Get the element
                    Element element = _BoardGrid[rowIndex, colIndex];
                    bool newGroup = false;

                    // If it's null and a new group can't be added, move on.
                    if (element == null && _GroupCount >= Config.GroupsPerCrozzleLimit)
                        continue;

                    if (element != null)
                    {
                        TotalElements++;
                        
                        // If it's already an intersecting element, move on.
                        if (element.HorizontalWord != null && element.VerticalWord != null)
                            continue;
                    }
                        
                    
                    // Find all words in the list that contain the letter.
                    // If it's null, return every word.
                    Regex letter = new Regex(@"^\w");
                    if (element != null)
                        letter = new Regex(element.Letter.ToString());

                    foreach (Word word in SortedWordlist)
                    {
                        // For each matching letter in the word
                        foreach (Match letterIndex in letter.Matches(word.String))
                        {
                            List<ActiveWord> WordCombo;

                            // Try add horizontally
                            WordCombo = new List<ActiveWord>();
                            int RowStart_H = rowIndex;
                            int ColStart_H = colIndex - letterIndex.Index;
                            WordCombo.Add(word.MakeActiveWord(RowStart_H, ColStart_H, Config.HorizontalKeyWord));

                            // If the word can be added
                            if (TryAddWord(ref WordCombo))
                            {
                                // If the word forms a new group, a second vertical joining word must also be found.
                                if (element == null)
                                {  
                                    int rowIndex2 = rowIndex;
                                    int colIndex2 = colIndex;

                                    // For each letter in the word.
                                    foreach (Char element2 in word.String)
                                    {
                                        // For each word, test if it contains the letter.
                                        Regex letter2 = new Regex(element2.ToString());
                                        foreach (Word word2 in SortedWordlist)
                                        {
                                            // Test it's not the same word.
                                            if (word2.String == word.String)
                                                continue;

                                            // For each matching letter in the word
                                            foreach (Match letter2Index in letter2.Matches(word2.String))
                                            {
                                                // Test if this second word can be added (vertically since first word is horizontal).
                                                List<ActiveWord> WordCombo2 = new List<ActiveWord>();
                                                int RowStart2_V = rowIndex2 - letter2Index.Index;
                                                int ColStart2_V = colIndex2;
                                                WordCombo2.Add(word2.MakeActiveWord(RowStart2_V, ColStart2_V, Config.VerticalKeyWord));
                                                
                                                // If it can be added, add the word to the word combo and then add this to the Possibilities list.
                                                if (TryAddWord(ref WordCombo2))
                                                {
                                                    WordCombo.Add(WordCombo2[0]);
                                                    Possibilities.Add(WordCombo);
                                                    // Since the wordlist was already sorted, the first word combo found will be the top scoring, so break from finding word combos.
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

                            //Try add vertically
                            WordCombo = new List<ActiveWord>();
                            int RowStart_V = rowIndex - letterIndex.Index;
                            int ColStart_V = colIndex;
                            WordCombo.Add(word.MakeActiveWord(RowStart_V, ColStart_V, Config.VerticalKeyWord));
                            if (TryAddWord(ref WordCombo))
                            {
                                // If this is a new word group
                                if (element == null)
                                {
                                    int rowIndex2 = rowIndex;
                                    int colIndex2 = colIndex;

                                    // For each letter in the word.
                                    foreach (Char element2 in word.String)
                                    {
                                        // For each word, test if it contains the letter.
                                        Regex letter2 = new Regex(element2.ToString());
                                        foreach (Word word2 in SortedWordlist)
                                        {
                                            // Test it's not the same word.
                                            if (word2.String == word.String)
                                                continue;

                                            // For each matching letter in the word
                                            foreach (Match letter2Index in letter2.Matches(word2.String))
                                            {
                                                // Test if this second word can be added (horizontally since first word is vertical).
                                                List<ActiveWord> WordCombo2 = new List<ActiveWord>();
                                                int RowStart2_H = rowIndex2;
                                                int ColStart2_H = colIndex2 - letter2Index.Index;
                                                WordCombo2.Add(word2.MakeActiveWord(RowStart2_H, ColStart2_H, Config.HorizontalKeyWord));

                                                // If it can be added, add the word to the word combo and then add this to the Possibilities list.
                                                if (TryAddWord(ref WordCombo2))
                                                {
                                                    WordCombo.Add(WordCombo2[0]);
                                                    Possibilities.Add(WordCombo);
                                                    // Since the wordlist was already sorted, the first word combo found will be the top scoring, so break from finding word combos.
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
            
            // If this is the first word to be added
            if (TotalElements == 0)
            {
                // Add it in the center of the grid [ Rows+1, Cols+1 ] as a horizontal word
                SortedWordlist = SortByScore(SortedWordlist);
                BestWords.Add(SortedWordlist[0].MakeActiveWord(_Rows,_Cols, Config.VerticalKeyWord));
            }
            else
            {
                // If possibilities contains values
                if (Possibilities.Count > 0)
                {
                    // Sort the Posibilities list by score
                    Possibilities = SortByScore(Possibilities);

                    // Best word is the first in the list
                    BestWords = Possibilities[0];
                }

            }
            
            return BestWords;
        }

        /// <summary>
        /// Test wether a word can be added to the grid.
        /// </summary>
        /// <param name="testWord"></param>
        /// <returns></returns>
        private bool TryAddWord(ref List<ActiveWord> testWord)
        {

            // Sometimes a word can only be added if a second word (or more) is added with it, hence the word is a actually becomes a list of words.
            bool result = true;

            // word[0] is always the word to be tested
            foreach (ActiveWord word in testWord)
            {
                int rowIndex = word.RowStart;
                int colIndex = word.ColStart;
                int wordIntersections = 0;

                // Check if word can fit onto the board - Checks each letter
                for (int letterIndex = 0; letterIndex < word.Length; letterIndex++)
                {

                    // Check element fits on board
                    int newHeight = Math.Max(maxRow, rowIndex) - (Math.Min(minRow, rowIndex) - 1);
                    int newWidth = Math.Max(maxCol, colIndex) - (Math.Min(minCol, colIndex) - 1);
                    if (newHeight > _Rows || newWidth > _Cols)
                    {
                        result = false;
                        break;
                    }

                    // If there is an element
                    Element element = _BoardGrid[rowIndex, colIndex];
                    if (element != null)
                    {
                        // Check the intersection count is not greater than the limit
                        if ((wordIntersections + 1) > Config.MaxIntersectingWords)
                        {
                            result = false;
                            break;
                        }
                        wordIntersections++;

                        // Check the intersecting word intersection count is not greater than the limit
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


                        // Check the letter is a match - If( letters match || both words are horrizontal || both words are vertical )
                        if (element.Letter != word[letterIndex] || (element.HorizontalWord != null && word.Orientation == Config.HorizontalKeyWord) || (element.VerticalWord != null && word.Orientation == Config.VerticalKeyWord))
                        {
                            result = false;
                            break;
                        }
                    }

                    // Calculate Score
                    word.ActiveScore -= Config.PointsForNonIntersecting(word[letterIndex]); // Subtract non-intersecting letter points
                    word.ActiveScore += Config.PointsForIntersecting(word[letterIndex]); // Add intersecting letter points

                    // Increment Element
                    if (word.Orientation == Config.HorizontalKeyWord)
                        colIndex++;
                    else
                        rowIndex++;
                }

                // Check Spacing - Word may fit onto board but may be too close to another word
                if (result == true)
                {
                    rowIndex = word.RowStart;
                    colIndex = word.ColStart;

                    // If horizontal word && (there is an element before it || there is an element after it)
                    if (word.Orientation == Config.HorizontalKeyWord)
                        if (_BoardGrid[rowIndex, colIndex - 1] != null || _BoardGrid[rowIndex, word.ColEnd + 1] != null)
                            result = false;
                    // If vertical word && (there is an element before it || there is an element after it)
                    if (word.Orientation == Config.VerticalKeyWord)
                        if (_BoardGrid[rowIndex - 1, colIndex] != null || _BoardGrid[word.RowEnd + 1, colIndex] != null)
                            result = false;


                    // For each element where letter will be placed, check the spaceing around it for other words
                    for (int letterIndex = 0; letterIndex < word.Length; letterIndex++)
                    {
                        // If adding a horizontal word - Test the grid elements above and below for words.
                        if (word.Orientation == Config.HorizontalKeyWord)
                        {
                            Element element = _BoardGrid[rowIndex, colIndex + letterIndex];
                            Element elementAbove = _BoardGrid[rowIndex - 1, colIndex + letterIndex];
                            Element elementBelow = _BoardGrid[rowIndex + 1, colIndex + letterIndex];

                            // If another word is found, word cannot be added.
                            if ((elementAbove != null && !IsSameWord(element, elementAbove)) || (elementBelow != null && !IsSameWord(element, elementBelow)))
                            {
                                result = false;
                                break;
                            }

                            /*
                                Currently when an element (letters on the grid) is found above or below the word, the 
                                word is rejected. Further processing could be done here to make this much better 
                                however as if these letters could form another word, this would be prefered. 

                                The application designed was to then search the word list to see if a vertical 
                                word could be added, thus ensuring all conecting letters are still apart of a word. This
                                would benifit the Medium and Hard crozzles that allow words to touch if they still form
                                a word. [If only I had more time...]                         
                            */
                        }
                        // If adding a vertical word - Test the grid elements left and right for words.
                        else
                        {
                            Element element = _BoardGrid[rowIndex + letterIndex, colIndex];
                            Element elementLeft = _BoardGrid[rowIndex + letterIndex, colIndex - 1];
                            Element elementRight = _BoardGrid[rowIndex + letterIndex, colIndex + 1];

                            // If another word is found, word cannot be added.
                            if ((elementLeft != null && !IsSameWord(element, elementLeft)) || (elementRight != null && !IsSameWord(element, elementRight)))
                            {
                                result = false;
                                break;
                            }

                            /*
                                Currently when an element (letters on the grid) is found left or right of the word, the 
                                word is rejected. Further processing could be done here to make this much better 
                                however as if these letters could form another word, this would be prefered. 

                                The application designed was to then search the word list to see if a vertical 
                                word could be added, thus ensuring all conecting letters are still apart of a word. This
                                would benifit the Medium and Hard crozzles that allow words to touch if they still form
                                a word. [If only I had more time...]                         
                            */
                        }
                    }
                }
            }  
            
            // Return if the word can be added or not.
            return result;
        }

        /// <summary>
        /// Tests if the word in two elements is apart of the same word.
        /// </summary>
        /// <param name="element1"></param>
        /// <param name="element2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Counts the number of intersections in a word.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private int WordIntersectionCount(ActiveWord word)
        {
            int count = 0;
            int rowIndex = word.RowStart;
            int colIndex = word.ColStart;

            for (int letterIndex = 0; letterIndex < word.Length; letterIndex++)
            {
                // If there is an element 
                if (_BoardGrid[rowIndex, colIndex] != null)
                    if (_BoardGrid[rowIndex, colIndex].HorizontalWord != null && _BoardGrid[rowIndex, colIndex].VerticalWord != null)
                        count++;
                // Increment board index
                if (word.Orientation == Config.HorizontalKeyWord)
                    colIndex++;
                else
                    rowIndex++;
            }
            return count;
        }

        #endregion

        #region Sorting Methods: SortByScore()

        /// <summary>
        /// Sorts a word list by its active score.
        /// </summary>
        /// <param name="wordList"></param>
        /// <returns></returns>
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
                    // If word doesnt require as many word to be placed
                    if (score == topWordScore && elements <= topWordElements)
                        topWordIndex = index;
                }
                tempList.Add(wordList[topWordIndex]);
                wordList.RemoveAt(topWordIndex);
            }
            return tempList;
        }

        /// <summary>
        /// Sorts a word list by its base score.
        /// </summary>
        /// <param name="wordList"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts the magic board grid to a standard board grid.
        /// </summary>
        /// <returns></returns>
        public Board LoseTheMagic()
        {
            Board board = new Board(_Rows, _Cols);
            board.AddWordList(ActiveWordList);
            return board;
        }

        /// <summary>
        /// Adds a word to the magic board.
        /// </summary>
        /// <param name="word"></param>
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

        /// <summary>
        /// Rebuilds the word list based on the current magic grid.
        /// </summary>
        /// <returns></returns>
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