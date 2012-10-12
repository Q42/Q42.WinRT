using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Q42.WinRT.Data;
using Q42.WinRT.UnitTests.Storage;

namespace Q42.WinRT.UnitTests.Data
{
    [TestClass]
    public class JsonCacheTest
    {

        [TestMethod]
        public async Task GetCacheTest()
        {
            //Clear the cache
            await JsonCache.ClearAll();

            var result1 = await JsonCache.GetAsync("test", () => LongRunningOperation("result"));
            Assert.AreEqual("result", result1);

            var result2 = await JsonCache.GetAsync("test", () => LongRunningOperation("result 2"));
            Assert.AreEqual("result", result2);


        }

        [TestMethod]
        public async Task ClearCache()
        {
            //Clear the cache
            await JsonCache.ClearAll();

            var result1 = await JsonCache.GetAsync("test", () => LongRunningOperation("result"));
            Assert.AreEqual("result", result1);

            await JsonCache.ClearAll();


            var result2 = await JsonCache.GetAsync("test", () => LongRunningOperation("result 2"));
            Assert.AreEqual("result 2", result2); //Not from cache


        }

        [TestMethod]
        public async Task DeleteCacheKeyTest()
        {
            //Clear the cache
            await JsonCache.ClearAll();

            var result1 = await JsonCache.GetAsync("test", () => LongRunningOperation("result"));
            Assert.AreEqual("result", result1);

            await JsonCache.Delete("test");


            var result2 = await JsonCache.GetAsync("test", () => LongRunningOperation("result 2"));
            Assert.AreEqual("result 2", result2); //Not from cache


        }

        [TestMethod]
        public async Task ForceGetTest()
        {
            //Clear the cache
            await JsonCache.ClearAll();

            var result1 = await JsonCache.GetAsync("test", () => LongRunningOperation("result"));
            Assert.AreEqual("result", result1);

            var result2 = await JsonCache.GetAsync("test", () => LongRunningOperation("result 2"), forceRefresh: true);
            Assert.AreEqual("result 2", result2); //Not from cache


        }

        [TestMethod]
        public async Task SetCacheKeyTest()
        {
            //Clear the cache
            await JsonCache.ClearAll();

            await JsonCache.Set("test", "result set");

            var result = await JsonCache.GetAsync("test", () => LongRunningOperation("result 2"));
            Assert.AreEqual("result set", result);
        }

        [TestMethod]
        public async Task GetCacheKeyTest()
        {
            //Clear the cache
            await JsonCache.ClearAll();

            var emptyResult = await JsonCache.GetFromCache<string>("test");
            Assert.AreEqual(null, emptyResult);

            await JsonCache.Set("test", "result set");

            var result = await JsonCache.GetFromCache<string>("test");
            Assert.AreEqual("result set", result);
        }


        [TestMethod]
        public async Task SetExpireDateValidTest()
        {
            //Clear the cache
            await JsonCache.ClearAll();

            await JsonCache.Set("test", "result set", DateTime.Now.AddDays(1));

            var result = await JsonCache.GetFromCache<string>("test");
            Assert.AreEqual("result set", result);
        }

        [TestMethod]
        public async Task SetExpireDateInValidTest()
        {
            //Clear the cache
            await JsonCache.ClearAll();

            await JsonCache.Set("test", "result set", DateTime.Now.AddDays(-1));

            var emptyResult = await JsonCache.GetFromCache<string>("test");
            Assert.AreEqual(null, emptyResult);
        }


        private async Task<string> LongRunningOperation(string result)
        {
            await Task.Delay(1000);

            return result;
        }
       
    }
}
