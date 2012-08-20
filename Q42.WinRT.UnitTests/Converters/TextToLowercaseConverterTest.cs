using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Q42.WinRT.Converters;

namespace Q42.WinRT.UnitTests.Converters
{
    [TestClass]
    public class TextToLowercaseConverterTest
    {
        [TestMethod]
        public void TestLowercase()
        {
            var input = "lower";

            TextToLowerConverter converter = new TextToLowerConverter();
           string result = (string)converter.Convert(input, typeof(string), null, null);

           Assert.AreEqual(input, result);
        }

        [TestMethod]
        public void TestUppercase()
        {
            var input = "LOWER";

            TextToLowerConverter converter = new TextToLowerConverter();
            string result = (string)converter.Convert(input, typeof(string), null, null);

            Assert.AreEqual("lower", result);
        }

        [TestMethod]
        public void TestMixedcase()
        {
            var input = "LoWeR";

            TextToLowerConverter converter = new TextToLowerConverter();
            string result = (string)converter.Convert(input, typeof(string), null, null);

            Assert.AreEqual("lower", result);
        }
    }
}
