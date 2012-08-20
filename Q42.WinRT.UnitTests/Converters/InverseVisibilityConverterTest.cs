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
    public class InverseInverseVisibilityConverterTest
    {
        [TestMethod]
        public void TestBoolTrue()
        {
            var input = true;

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestBoolFalse()
        {
            var input = false;

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestInt1()
        {
            var input = 1;

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestInt80()
        {
            var input = 80;

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestInt0()
        {
            var input = 0;

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestString()
        {
            var input = "test";

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestStringEmpty()
        {
            var input = "";

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestStringNull()
        {
            string input = null;

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestObjectNull()
        {
            object input = null;

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestObjectNotNull()
        {
            object input = new InverseVisibilityConverter();

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

    }
}
