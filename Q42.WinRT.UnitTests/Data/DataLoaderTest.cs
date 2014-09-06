using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Q42.WinRT.Portable.Data;
using Q42.WinRT.Data;

namespace Q42.WinRT.UnitTests.Data
{
    [TestClass]
    public class DataLoaderTest
    {
        [TestMethod]
        public void DataLoaderInitializeTest()
        {
            DataLoader a = new DataLoader();

            Assert.AreEqual(LoadingState.None, a.LoadingState);

            Assert.IsFalse(a.IsBusy);
            Assert.IsFalse(a.IsFinished);
            Assert.IsFalse(a.IsError);

        }


        [TestMethod]
        public async Task DataLoaderLoadTest()
        {
            DataLoader a = new DataLoader();

            var task = a.LoadAsync(() => LongRunningOperation());

            Assert.AreEqual(LoadingState.Loading, a.LoadingState);

            Assert.IsTrue(a.IsBusy);
            Assert.IsFalse(a.IsFinished);
            Assert.IsFalse(a.IsError);

            Assert.AreEqual("result", await task);

            Assert.IsFalse(a.IsBusy);
            Assert.IsTrue(a.IsFinished);
            Assert.IsFalse(a.IsError);
        }

        [TestMethod]
        public async Task DataLoaderLoadErrorTest()
        {
            //false indicates: Do not swallow exception!
            DataLoader a = new DataLoader(false);

            var task = a.LoadAsync(() => LongRunningOperationThrowsError());

            Assert.AreEqual(LoadingState.Loading, a.LoadingState);

            Assert.IsTrue(a.IsBusy);
            Assert.IsFalse(a.IsFinished);
            Assert.IsFalse(a.IsError);

            try
            {
                Assert.AreEqual("result", await task);
            }
            catch { }

            Assert.IsFalse(a.IsBusy);
            Assert.IsFalse(a.IsFinished);
            Assert.IsTrue(a.IsError);
        }

        [TestMethod]
        public async Task DataLoaderLoadErrorSwallowTest()
        {
            //Swallow exceptions by default
            DataLoader a = new DataLoader();

            var task = a.LoadAsync(() => LongRunningOperationThrowsError());

            Assert.AreEqual(LoadingState.Loading, a.LoadingState);

            Assert.IsTrue(a.IsBusy);
            Assert.IsFalse(a.IsFinished);
            Assert.IsFalse(a.IsError);

            //Exception is swallowed
            Assert.AreEqual(null, await task);

            Assert.IsFalse(a.IsBusy);
            Assert.IsFalse(a.IsFinished);
            Assert.IsTrue(a.IsError);
        }

        [TestMethod]
        public async Task DataLoaderLoadErrorHandleTest()
        {
            DataLoader a = new DataLoader();

            var task = a.LoadAsync(() => LongRunningOperationThrowsError(), errorCallback: e =>
            {
                //Handle error
            });

            Assert.AreEqual(LoadingState.Loading, a.LoadingState);

            Assert.IsTrue(a.IsBusy);
            Assert.IsFalse(a.IsFinished);
            Assert.IsFalse(a.IsError);

            //Exception is handled, result is null
            Assert.AreEqual(null, await task);

            Assert.IsFalse(a.IsBusy);
            Assert.IsFalse(a.IsFinished);
            Assert.IsTrue(a.IsError);
        }


        [TestMethod]
        public async Task DataLoaderCacheThenRefreshTest()
        {
            DataLoader a = new DataLoader();

            int count = 0;

            await a.LoadCacheThenRefreshAsync(() => LongRunningOperation(), () => LongRunningOperation(),
                (result) => {
                    count++;
                });

            Assert.AreEqual(LoadingState.Finished, a.LoadingState);

            Assert.IsTrue(a.IsFinished);
            Assert.IsFalse(a.IsBusy);
            Assert.IsFalse(a.IsError);

            Assert.AreEqual(2, count);

        }

        [TestMethod]
        public async Task DataLoaderLoadFallbackToCacheAsyncTest()
        {
            DataLoader a = new DataLoader();

            int count = 0;

            await a.LoadFallbackToCacheAsync(() => LongRunningOperation(), () => LongRunningOperation(),
                (result) =>
                {
                    count++;
                });

            Assert.AreEqual(LoadingState.Finished, a.LoadingState);

            Assert.IsTrue(a.IsFinished);
            Assert.IsFalse(a.IsBusy);
            Assert.IsFalse(a.IsError);

            Assert.AreEqual(1, count);

        }

        [TestMethod]
        public async Task DataLoaderLoadFallbackToCacheAsync_FailTest()
        {
            DataLoader a = new DataLoader();

            int count = 0;

            await a.LoadFallbackToCacheAsync(() => LongRunningOperationThrowsError(), () => LongRunningOperation(),
                (result) =>
                {
                    count++;
                }, errorCallback: e =>
                {
                    count++;
                    //Handle error
                });

            Assert.AreEqual(LoadingState.Finished, a.LoadingState);

            Assert.IsTrue(a.IsFinished);
            Assert.IsFalse(a.IsBusy);
            Assert.IsFalse(a.IsError);

            Assert.AreEqual(2, count);

        }

        [TestMethod]
        public async Task DataLoaderCacheCombinationTest()
        {
          DataLoader a = new DataLoader();

          var result = await a.LoadAsync(() => DataCache.GetAsync("testkey", async () => await LongRunningOperation("result_1"), expireDate: DateTime.Now.AddDays(8)));
          Assert.AreEqual("result_1", result);

          //Should get value from cache
          var result_2 = await a.LoadAsync(() => DataCache.GetAsync("testkey", async () => await LongRunningOperation("result_2"), expireDate: DateTime.Now.AddDays(8)));
          Assert.AreEqual("result_1", result_2);

        }



        private async Task<string> LongRunningOperation()
        {
            await Task.Delay(1000);

            return "result";
        }

        private async Task<string> LongRunningOperation(string result)
        {
          await Task.Delay(1000);

          return result;
        }


        private async Task<string> LongRunningOperationThrowsError()
        {
            await Task.Delay(1000);

            throw new Exception("Error during long running operation");
            
        }


    }
}
