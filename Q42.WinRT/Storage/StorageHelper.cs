using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

#if WINDOWS_PHONE
using Q42.WinRT.Phone;
#endif

namespace Q42.WinRT.Storage
{
  /// <summary>
  /// Possible storage locations
  /// </summary>
  public enum StorageType
  {
    
    /// <summary>Local</summary>
    Local,

#if NETFX_CORE
    /// <summary>Temporary</summary>
    Temporary,
    /// <summary>Roaming</summary>
    Roaming,
#endif
  }

  /// <summary>
  /// Save object to local storage, serializes as json and writes object to a file
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class StorageHelper<T> : IStorageHelper<T>
  {
    private readonly string _fileExtension = ".json";

    private ApplicationData _appData = Windows.Storage.ApplicationData.Current;
    private StorageType _storageType;
    private string _subFolder;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="StorageType"></param>
    /// <param name="subFolder"></param>
    public StorageHelper(StorageType StorageType, string subFolder = null)
    {
      _storageType = StorageType;
      _subFolder = subFolder;
    }

    /// <summary>
    /// Delete file
    /// </summary>
    /// <param name="fileName"></param>
    public async Task DeleteAsync(string fileName)
    {
      fileName = fileName + _fileExtension;
      try
      {
        StorageFolder folder = await GetFolderAsync().ConfigureAwait(false);

        var contains = await folder.ContainsFileAsync(fileName).ConfigureAwait(false);
        if (contains)
        {
          var file = await folder.GetFileAsync(fileName);
          await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Save object from file
    /// </summary>
    /// <param name="Obj"></param>
    /// <param name="fileName"></param>
    public async Task SaveAsync(T Obj, string fileName)
    {

      fileName = fileName + _fileExtension;
      try
      {
        if (Obj != null)
        {
          //Get file
          StorageFile file = null;
          StorageFolder folder = await GetFolderAsync().ConfigureAwait(false);
          file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);


          //Serialize object
          var json = JsonConvert.SerializeObject(Obj);

          //Write content to file
          await FileIO.WriteTextAsync(file, json);
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>
    /// Load object from file
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public async Task<T> LoadAsync(string fileName)
    {
      fileName = fileName + _fileExtension;
      try
      {

        StorageFile file = null;
        StorageFolder folder = await GetFolderAsync().ConfigureAwait(false);

        var contains = await folder.ContainsFileAsync(fileName).ConfigureAwait(false);
        if (contains)
        {
          file = await folder.GetFileAsync(fileName);

          var data = await FileIO.ReadTextAsync(file);

          //Deserialize to object
          T result = JsonConvert.DeserializeObject<T>(data);

          return result;
        }
        else
        {
          return default(T);
        }

      }
      catch (Exception)
      {
        //Unable to load contents of file
        throw;
      }
    }

    /// <summary>
    /// Get folder based on storagetype
    /// </summary>
    /// <returns></returns>
    public async Task<StorageFolder> GetFolderAsync()
    {
      StorageFolder folder;
      switch (_storageType)
      {
        case StorageType.Local:
          folder = _appData.LocalFolder;
          break;
#if NETFX_CORE
        case StorageType.Roaming:
          folder = _appData.RoamingFolder;
          break;
        case StorageType.Temporary:
          folder = _appData.TemporaryFolder;
          break;
#endif
        default:
          throw new Exception(String.Format("Unknown StorageType: {0}", _storageType));
      }

      if (!string.IsNullOrEmpty(_subFolder))
      {
        folder = await folder.CreateFolderAsync(_subFolder, CreationCollisionOption.OpenIfExists);
      }

      return folder;
    }

    /// <summary>
    /// Deletes all files in current folder
    /// </summary>
    /// <returns></returns>
    public async Task DeleteAllFiles()
    {
      StorageFolder folder = await GetFolderAsync().ConfigureAwait(false);

      try
      {
        await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
      }
      catch (UnauthorizedAccessException)
      {
      }
    }

    /// <summary>
    /// Clear the complete cache
    /// </summary>
    /// <returns></returns>
    public static Task ClearLocalAll()
    {
      return Task.Run(async () =>
      {
        StorageHelper<object> storage = new StorageHelper<object>(StorageType.Local);
        var folder = await storage.GetFolderAsync().ConfigureAwait(false);

        foreach (var sub in await folder.GetFoldersAsync())
        {
          try
          {
            await sub.DeleteAsync(StorageDeleteOption.PermanentDelete);
          }
          catch (UnauthorizedAccessException)
          {
          }
        }

        foreach (var file in await folder.GetFilesAsync())
        {
          try
          {
            await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
          }
          catch (UnauthorizedAccessException)
          {
          }
        }
      });
    }

  }
}
