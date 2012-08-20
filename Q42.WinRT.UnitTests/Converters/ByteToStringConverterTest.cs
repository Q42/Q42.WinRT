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
    public class ByteToStringConverterTest
    {
        [TestMethod]
        public void TestInt()
        {
            int input = 1;

            ByteToStringConverter converter = new ByteToStringConverter();
            string result = (string)converter.Convert(input, typeof(double), null, null);

            Assert.AreEqual("1 KB", result);
        }

        [TestMethod]
        public void TestLargeInt()
        {
            int input = 100000000;

            ByteToStringConverter converter = new ByteToStringConverter();
            string result = (string)converter.Convert(input, typeof(double), null, null);

            Assert.AreEqual("95.37 MB", result);
        }

        [TestMethod]
        public void TestDouble()
        {
            double input = 1;

            ByteToStringConverter converter = new ByteToStringConverter();
            string result = (string)converter.Convert(input, typeof(double), null, null);

            Assert.AreEqual("1 KB", result);
        }
    }
}
