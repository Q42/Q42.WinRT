using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Q42.WinRT.Converters;
using System.Collections.Generic;

#if NETFX_CORE
using Windows.UI.Xaml;
#elif WINDOWS_PHONE
using System.Windows;
#endif

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
        public void TestEmptyCollection()
        {
          IEnumerable<string> input = new List<string>();

          VisibilityConverter converter = new VisibilityConverter();
          Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

          Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestFilledCollection()
        {
          IEnumerable<string> input = new List<string>() { "test" } ;

          VisibilityConverter converter = new VisibilityConverter();
          Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

          Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestEmptyIntCollection()
        {
          IEnumerable<int> input = new List<int>();

          VisibilityConverter converter = new VisibilityConverter();
          Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

          Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestFilledIntCollection()
        {
          IEnumerable<int> input = new List<int>() { 1,2,3 };

          VisibilityConverter converter = new VisibilityConverter();
          Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

          Assert.AreEqual(Visibility.Visible, result);
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

        [TestMethod]
        public void TestVisibiliyVisible()
        {
            Visibility input = Visibility.Visible;

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestVisibiliyCollapsed()
        {
            Visibility input = Visibility.Collapsed;

            VisibilityConverter converter = new VisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

    }
}
