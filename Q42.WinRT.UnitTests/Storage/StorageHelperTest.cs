using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Q42.WinRT.Storage;

namespace Q42.WinRT.UnitTests.Storage
{
    [TestClass]
    public class StorageHelperTest
    {
        public class MyModel
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        [TestMethod]
        public async Task StorageHelperSaveTest()
        {
            var myObject = new MyModel() { Name = "Michiel", Age = 29 };

            StorageHelper<MyModel> sh = new StorageHelper<MyModel>(StorageType.Local);

            await sh.SaveAsync(myObject, "myfile");

            var loadedObject = await sh.LoadAsync("myfile");

            Assert.AreEqual(myObject.Name, loadedObject.Name);
            Assert.AreEqual(myObject.Age, loadedObject.Age);

            await sh.DeleteAsync("myfile");

        }

        [TestMethod]
        public async Task StorageHelperSaveOverwriteTest()
        {
            var myObject = new MyModel() { Name = "Michiel", Age = 29 };

            StorageHelper<MyModel> sh = new StorageHelper<MyModel>(StorageType.Local);

            await sh.SaveAsync(myObject, "myfile");

            var newObject = new MyModel() { Name = "Simon", Age = 0 };

            //Save new object
            await sh.SaveAsync(newObject, "myfile");
            var loadedObject = await sh.LoadAsync("myfile");

            Assert.AreEqual(newObject.Name, loadedObject.Name);
            Assert.AreEqual(newObject.Age, loadedObject.Age);

            await sh.DeleteAsync("myfile");

        }

        [TestMethod]
        public async Task StorageHelperDeleteTest()
        {
            var myObject = new MyModel() { Name = "Michiel", Age = 29 };

            StorageHelper<MyModel> sh = new StorageHelper<MyModel>(StorageType.Local);

            await sh.SaveAsync(myObject, "myfile");

            //Delete saved object
            await sh.DeleteAsync("myfile");

            var loadedObject = await sh.LoadAsync("myfile");
            Assert.IsNull(loadedObject);


        }

        [TestMethod]
        public async Task StorageHelperDeleteNotExistingTest()
        {
            StorageHelper<MyModel> sh = new StorageHelper<MyModel>(StorageType.Local);

            //Delete non existing object
            await sh.DeleteAsync("myfile6526161651651");

            var loadedObject = await sh.LoadAsync("myfile6526161651651");
            Assert.IsNull(loadedObject);


        }


        [TestMethod]
        public async Task StorageHelperNotExistingTest()
        {
            StorageHelper<MyModel> sh = new StorageHelper<MyModel>(StorageType.Local);

            var loadedObject = await sh.LoadAsync("myfile561616516");

            Assert.IsNull(loadedObject);

        }
    }
}
