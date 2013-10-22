using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Q42.WinRT.Storage;

namespace Q42.WinRT.UnitTests.Storage
{
  [TestClass]
  public class SettingsHelperTest
  {
   
    [TestMethod]
    public void SettingsHelperSaveTest()
    {

      SettingsHelper.Set("myfile", "test");

      var loadedObject = SettingsHelper.Get<string>("myfile");

      Assert.AreEqual("test", loadedObject);

      SettingsHelper.Remove("myfile");

    }

    [TestMethod]
    public void SettingsHelperSaveOverwriteTest()
    {

      SettingsHelper.Set("myfile", "test");


      //Save new object
      SettingsHelper.Set("myfile", "test2");
      var loadedObject = SettingsHelper.Get<string>("myfile");

      Assert.AreEqual("test2", loadedObject);

      SettingsHelper.Remove("myfile");

    }

    [TestMethod]
    public void SettingsHelperDeleteTest()
    {

      SettingsHelper.Set("myfile", "test");

      //Delete saved object
      SettingsHelper.Remove("myfile");

      var loadedObject = SettingsHelper.Get<string>("myfile");
      Assert.IsNull(loadedObject);


    }

    [TestMethod]
    public void SettingsHelperDeleteNotExistingTest()
    {
      //Delete non existing object
      SettingsHelper.Remove("myfile6526161651651");

      var loadedObject = SettingsHelper.Get<string>("myfile6526161651651");
      Assert.IsNull(loadedObject);


    }

    [TestMethod]
    public void SettingsHelperNotExistingTest()
    {
      var loadedObject = SettingsHelper.Get<string>("myfile561616516");
      Assert.IsNull(loadedObject);

    }
  }
}
