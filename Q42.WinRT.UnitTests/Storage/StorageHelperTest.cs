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
    public async Task StorageHelperJsonTest()
    {
        await StorageHelperSaveTest(StorageSerializer.JSON);
        await StorageHelperSaveOverwriteTest(StorageSerializer.JSON);
        await StorageHelperDeleteTest(StorageSerializer.JSON);
        await StorageHelperDeleteNotExistingTest(StorageSerializer.JSON);
        await StorageHelperNotExistingTest(StorageSerializer.JSON);
    }

    [TestMethod]
    public async Task StorageHelperXmlTest()
    {
        await StorageHelperSaveTest(StorageSerializer.XML);
        await StorageHelperSaveOverwriteTest(StorageSerializer.XML);
        await StorageHelperDeleteTest(StorageSerializer.XML);
        await StorageHelperDeleteNotExistingTest(StorageSerializer.XML);
        await StorageHelperNotExistingTest(StorageSerializer.XML);
    }

    [TestMethod]
    public async Task StorageHelperDifferentSerializerTest()
    {
        var myObject = new MyModel() { Name = "Michiel", Age = 29 };

        IStorageHelper<MyModel> sh = new StorageHelper<MyModel>(Windows.Storage.ApplicationData.Current.LocalFolder);

        await sh.SaveAsync(myObject, "myfile");

        IStorageHelper<MyModel> shXml = new StorageHelper<MyModel>(Windows.Storage.ApplicationData.Current.LocalFolder, serializerType:  StorageSerializer.XML);

        var loadedObject = await shXml.LoadAsync("myfile");

        Assert.IsNull(loadedObject);

        await sh.DeleteAsync("myfile");

    }

    public async Task StorageHelperSaveTest(StorageSerializer serializerType)
    {
      var myObject = new MyModel() { Name = "Michiel", Age = 29 };

      IStorageHelper<MyModel> sh = new StorageHelper<MyModel>(Windows.Storage.ApplicationData.Current.LocalFolder, serializerType: serializerType);

      await sh.SaveAsync(myObject, "myfile");

      var loadedObject = await sh.LoadAsync("myfile");

      Assert.AreEqual(myObject.Name, loadedObject.Name);
      Assert.AreEqual(myObject.Age, loadedObject.Age);

      await sh.DeleteAsync("myfile");

    }

    public async Task StorageHelperSaveOverwriteTest(StorageSerializer serializerType)
    {
      var myObject = new MyModel() { Name = "Michiel", Age = 29 };

      IStorageHelper<MyModel> sh = new StorageHelper<MyModel>(Windows.Storage.ApplicationData.Current.LocalFolder, serializerType: serializerType);

      await sh.SaveAsync(myObject, "myfile");

      var newObject = new MyModel() { Name = "Simon", Age = 0 };

      //Save new object
      await sh.SaveAsync(newObject, "myfile");
      var loadedObject = await sh.LoadAsync("myfile");

      Assert.AreEqual(newObject.Name, loadedObject.Name);
      Assert.AreEqual(newObject.Age, loadedObject.Age);

      await sh.DeleteAsync("myfile");

    }

    public async Task StorageHelperDeleteTest(StorageSerializer serializerType)
    {
      var myObject = new MyModel() { Name = "Michiel", Age = 29 };

      IStorageHelper<MyModel> sh = new StorageHelper<MyModel>(Windows.Storage.ApplicationData.Current.LocalFolder, serializerType: serializerType);

      await sh.SaveAsync(myObject, "myfile");

      //Delete saved object
      await sh.DeleteAsync("myfile");

      var loadedObject = await sh.LoadAsync("myfile");
      Assert.IsNull(loadedObject);


    }

    public async Task StorageHelperDeleteNotExistingTest(StorageSerializer serializerType)
    {
        IStorageHelper<MyModel> sh = new StorageHelper<MyModel>(Windows.Storage.ApplicationData.Current.LocalFolder, serializerType: serializerType);

      //Delete non existing object
      await sh.DeleteAsync("myfile6526161651651");

      var loadedObject = await sh.LoadAsync("myfile6526161651651");
      Assert.IsNull(loadedObject);


    }


    public async Task StorageHelperNotExistingTest(StorageSerializer serializerType)
    {
        IStorageHelper<MyModel> sh = new StorageHelper<MyModel>(Windows.Storage.ApplicationData.Current.LocalFolder, serializerType: serializerType);

      var loadedObject = await sh.LoadAsync("myfile561616516");

      Assert.IsNull(loadedObject);

    }
  }
}
