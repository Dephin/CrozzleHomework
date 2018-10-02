using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrozzleApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrozzleApplication.GenerateCrozzle;

namespace CrozzleApplication.Tests
{
	[TestClass()]
	public class CrozzleTests_Crozzle
	{
		[TestMethod()]
		public void CrozzleTest_BestWordList()
		{
			int[] nonIntersectingLetterPoints = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			int[] intersectingLetterPoints = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };
			List<String> WordStrList = new List<String>() { "AADEN", "AANYA", "AARON", "AL", "ALAN", "ALEX", "ALEXANDER", "ALEXANDRA", "ALEXIA", "ALI", "ALLA", "ALLAN", "ALLY", "AMY", "ANDREW", "ANN", "ANNE", "ANTHONY", "ARJUN", "ARLA", "ARLO", "ARLY", "ARMANI", "ARMIDA", "ARNOLD", "ARTHUR", "ARVIL", "ARYANA", "ASH", "ASHELY", "ASHLEA", "ASHLEE", "ASHLEIGH", "ASHLEY", "ASHLIE", "ASHLYN", "ASHTON", "ASHTYN", "ASTRID", "ATHENA", "AUBREE", "AUBREY", "AUBRIE", "AUDREY", "AURORA", "AURORE", "AUSTIN", "AXL", "AYDEN", "AYLA" };
			List<String> retWords = Crozzle.CreateCrozzelByGreedyAlgorithm(10,8, WordStrList, intersectingLetterPoints, nonIntersectingLetterPoints);
			foreach (String word in retWords)
				Console.WriteLine(word);
		}
	}
}