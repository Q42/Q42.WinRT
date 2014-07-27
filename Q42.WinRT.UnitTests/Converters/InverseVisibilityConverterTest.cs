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
        public void TestEmptyCollection()
        {
          IEnumerable<string> input = new List<string>();

          InverseVisibilityConverter converter = new InverseVisibilityConverter();
          Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

          Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestFilledCollection()
        {
          IEnumerable<string> input = new List<string>() { "test" };

          InverseVisibilityConverter converter = new InverseVisibilityConverter();
          Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

          Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestEmptyIntCollection()
        {
          IEnumerable<int> input = new List<int>();

          InverseVisibilityConverter converter = new InverseVisibilityConverter();
          Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

          Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void TestFilledIntCollection()
        {
          IEnumerable<int> input = new List<int>() { 1, 2, 3 };

          InverseVisibilityConverter converter = new InverseVisibilityConverter();
          Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

          Assert.AreEqual(Visibility.Collapsed, result);
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

        [TestMethod]
        public void TestVisibiliyVisible()
        {
            Visibility input = Visibility.Visible;

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void TestVisibiliyCollapsed()
        {
            Visibility input = Visibility.Collapsed;

            InverseVisibilityConverter converter = new InverseVisibilityConverter();
            Visibility result = (Visibility)converter.Convert(input, typeof(Visibility), null, null);

            Assert.AreEqual(Visibility.Visible, result);
        }

    }
}
