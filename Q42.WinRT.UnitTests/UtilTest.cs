using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Q42.WinRT.UnitTests
{
    [TestClass]
    public class UtilTest
    {
        public int TestProperty { get; set; }

        //Test crashes on creating WebView. Can only be done on UI Thread
        //[TestMethod]
        public async Task GetOsVersion()
        {
            string version = await Util.GetOsVersionAsync();

            Assert.AreNotEqual(string.Empty, version);
        }

        [TestMethod]
        public void GetAppVersion()
        {
            string version = Util.GetAppVersion();

            Assert.AreEqual("1.0.0.0", version);
        }

        [TestMethod]
        public void GetPropertyName()
        {
            //Returns property as string
            Assert.AreEqual("TestProperty", Q42.WinRT.Portable.Util.GetPropertyName(() => TestProperty));
        }
    }
}
