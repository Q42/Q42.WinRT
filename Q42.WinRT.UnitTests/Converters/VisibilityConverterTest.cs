using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Q42.WinRT.Converters;
using Windows.UI.Xaml;

namespace Q42.WinRT.UnitTests.Converters
{
    [TestClass]
    public class VisibilityConverterTest
    {
        [TestMethod]
        public void TestBoolTrue()
        {
            var input = true;

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestBoolFalse()
        {
            var input = false;

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestInt1()
        {
            var input = 1;

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestInt80()
        {
            var input = 80;

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestInt0()
        {
            var input = 0;

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestString()
        {
            var input = "test";

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestStringEmpty()
        {
            var input = "";

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestStringNull()
        {
            string input = null;

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestObjectNull()
        {
            object input = null;

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestObjectNotNull()
        {
            object input = new VisibilityConverter();

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

    }
}
