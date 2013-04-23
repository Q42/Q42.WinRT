using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Q42.WinRT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.WinRT.UnitTests.Data
{
    [TestClass]
    public class WebDataCacheTest
    {

        [TestMethod]
        public async Task GetAsyncTest()
        {
            await WebDataCache.GetAsync(new Uri("http://www.google.com/favicon.ico"), true);
        }

        [TestMethod]
        public async Task GetAsyncLongUriTest()
        {
            int length = 500;

            var baseUri = "http://www.google.com/favicon.ico?";
            var q = new string('x', length - baseUri.Length);

            await WebDataCache.GetAsync(new Uri(baseUri + q), true);
        }

        [TestMethod]
        public async Task AsyncFileValidFilename()
        {
          string key = Extensions.ToCacheKey(new Uri("http://afisha.tut.by/film.php?fid=2710"));

          Assert.IsFalse(key.Contains("/"));
        }
    }
   
}
