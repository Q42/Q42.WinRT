using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Q42.WinRT.Data;
using System.Collections.Generic;

namespace Q42.WinRT.UnitTests.Data
{
    [TestClass]
    public class DataCacheTest
    {

        [TestMethod]
        public async Task GetCacheTest()
        {
            //Clear the cache
            await DataCache.ClearAll();

            var result1 = await DataCache.GetAsync("test", () => LongRunningOperation("result"));
            Assert.AreEqual("result", result1);

            var result2 = await DataCache.GetAsync("test", () => LongRunningOperation("result 2"));
            Assert.AreEqual("result", result2);


        }

        [TestMethod]
        public async Task GetCacheNullValueTest()
        {
          //Clear the cache
          await DataCache.ClearAll();

          var result1 = await DataCache.GetAsync("test", () => LongRunningOperation(null));
          Assert.AreEqual(null, result1);

          var result2 = await DataCache.GetAsync("test", () => LongRunningOperation("result 2"));
          Assert.AreEqual("result 2", result2);


        }

        [TestMethod]
        public async Task ClearCache()
        {
            //Clear the cache
            await DataCache.ClearAll();

            var result1 = await DataCache.GetAsync("test", () => LongRunningOperation("result"));
            Assert.AreEqual("result", result1);

            await DataCache.ClearAll();


            var result2 = await DataCache.GetAsync("test", () => LongRunningOperation("result 2"));
            Assert.AreEqual("result 2", result2); //Not from cache


        }

        [TestMethod]
        public async Task ClearInvalidCache()
        {
          //Clear the cache
          await DataCache.ClearAll();

          var result1 = await DataCache.GetAsync("test1", () => LongRunningOperation("result1"), DateTime.Now.AddDays(-1));
          var result2 = await DataCache.GetAsync("test2", () => LongRunningOperation("result2"));

          await DataCache.ClearInvalid();


          var result1_new = await DataCache.GetAsync("test1", () => LongRunningOperation("result1_new"));
          var result2_new = await DataCache.GetAsync("test2", () => LongRunningOperation("result2_new"));

          Assert.AreEqual("result1_new", result1_new); //Not from cache
          Assert.AreEqual("result2", result2_new); //From cache


        }

        [TestMethod]
        public async Task DeleteCacheKeyTest()
        {
            //Clear the cache
            await DataCache.ClearAll();

            var result1 = await DataCache.GetAsync("test", () => LongRunningOperation("result"));
            Assert.AreEqual("result", result1);

            await DataCache.Delete("test");


            var result2 = await DataCache.GetAsync("test", () => LongRunningOperation("result 2"));
            Assert.AreEqual("result 2", result2); //Not from cache


        }

        [TestMethod]
        public async Task DeleteNotExistingCacheKeyTest()
        {
          //Clear the cache
          await DataCache.ClearAll();

          await DataCache.Delete("not_exist");

        }

        [TestMethod]
        public async Task ForceGetTest()
        {
            //Clear the cache
            await DataCache.ClearAll();

            var result1 = await DataCache.GetAsync("test", () => LongRunningOperation("result"));
            Assert.AreEqual("result", result1);

            var result2 = await DataCache.GetAsync("test", () => LongRunningOperation("result 2"), forceRefresh: true);
            Assert.AreEqual("result 2", result2); //Not from cache


        }

        [TestMethod]
        public async Task SetCacheKeyTest()
        {
            //Clear the cache
            await DataCache.ClearAll();

            await DataCache.Set("test", "result set");

            var result = await DataCache.GetAsync("test", () => LongRunningOperation("result 2"));
            Assert.AreEqual("result set", result);
        }

        [TestMethod]
        public async Task GetCacheKeyTest()
        {
            //Clear the cache
            await DataCache.ClearAll();

            var emptyResult = await DataCache.GetFromCache<string>("test");
            Assert.AreEqual(null, emptyResult);

            await DataCache.Set("test", "result set");

            var result = await DataCache.GetFromCache<string>("test");
            Assert.AreEqual("result set", result);
        }


        [TestMethod]
        public async Task SetExpireDateValidTest()
        {
            //Clear the cache
            await DataCache.ClearAll();

            await DataCache.Set("test", "result set", DateTime.Now.AddDays(1));

            var result = await DataCache.GetFromCache<string>("test");
            Assert.AreEqual("result set", result);
        }

        [TestMethod]
        public async Task SetExpireDateInValidTest()
        {
            //Clear the cache
            await DataCache.ClearAll();

            await DataCache.Set("test", "result set", DateTime.Now.AddDays(-1));

            var emptyResult = await DataCache.GetFromCache<string>("test");
            Assert.AreEqual(null, emptyResult);
        }


        private async Task<string> LongRunningOperation(string result)
        {
            await Task.Delay(1000);

            return result;
        }

        [TestMethod]
        public async Task ClearCacheMaxSizeTest()
        {
          //Clear the cache
          await DataCache.ClearAll();

          await DataCache.Set("first", "first result");
          await DataCache.Set("second", "second result");

          await DataCache.Clear(100);

          //First is gone
          var firstResult = await DataCache.GetFromCache<string>("first");
          Assert.IsNull(firstResult);

          //Second is still available
          var secondResult = await DataCache.GetFromCache<string>("second");
          Assert.AreEqual("second result", secondResult);


        }

    }
}
