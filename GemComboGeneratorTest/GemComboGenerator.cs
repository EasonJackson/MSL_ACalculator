using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GemComboGeneratorTest
{
    [TestClass]
    public class GemComboGeneratorTest
    {
        string[] options = { "A", "B", "C" };

        [TestMethod]
        public void GetCombinationsTest()
        {
            List<string> res = ACalculator.GemComboGenerator.GetCombinations(ref options, 3);
            Assert.AreEqual(res.Count, 10);

        }
    }
}
