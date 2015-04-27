using System;
using System.Linq;
using System.Threading.Tasks;
using Q42.WinRT.Storage;
using System.Collections.Generic;
using Windows.Storage;

namespace Q42.WinRT.Data
{
  /// <summary>
  /// Used as a wrapper around the stored file to keep metadata
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class CacheObject
  {
    /// <summary>
    /// Expire date of cached file
    /// </summary>
    public DateTime? ExpireDateTime { get; set; }

    /// <summary>
    /// Is the cache file valid?
    /// </summary>
    public bool IsValid
    {
      get
      {
        return (ExpireDateTime == null || ExpireDateTime.Value > DateTime.Now);
      }
    }
  }

  /// <summary>
  /// Used as a wrapper around the stored file to keep metadata
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class CacheObject<T> : CacheObject
  {
    /// <summary>
    /// Actual file being stored
    /// </summary>
    public T File { get; set; }
  }

  /// <summary>
  /// Stores objects as json or xml in the localstorage
  /// </summary>
  public static class DataCache
  {
    private static readonly string CacheFolder = "_jsoncache";

    /// <summary>
    /// Get object based on key, or generate the value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="generate"></param>
    /// <param name="expireDate"></param>
    /// <param name="forceRefresh"></param>
    /// <param name="serializerType">JSON or XML serializer</param>
    /// <returns></returns>
    public async static Task<T> GetAsync<T>(string key, Func<Task<T>> generate, DateTime? expireDate = null, bool forceRefresh = false, StorageSerializer serializerType = StorageSerializer.JSON)
    {
      object value;

      //Force bypass of cache?
      if (!forceRefresh)
      {
        //Check cache
        value = await GetFromCache<T>(key, serializerType).ConfigureAwait(false);
        if (value != null)
        {
          return (T)value;
        }
      }

      value = await generate().ConfigureAwait(false);
      await Set(key, value, expireDate).ConfigureAwait(false);

      return (T)value;

    }

    /// <summary>
    /// Get value from cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="serializerType">JSON or XML serializer</param>
    /// <returns></returns>
    public async static Task<T> GetFromCache<T>(string key, StorageSerializer serializerType = StorageSerializer.JSON)
    {
        IStorageHelper<CacheObject<T>> storage = new StorageHelper<CacheObject<T>>(StorageType.Local, CacheFolder, serializerType);

      //Get cache value
      var value = await storage.LoadAsync(key).ConfigureAwait(false);

      if (value == null)
        return default(T);
      else if (value.IsValid)
        return value.File;
      else
      {
        //Delete old value
        //Do not await
        Delete(key, serializerType);

        return default(T);
      }
    }

    /// <summary>
    /// Set value in cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expireDate"></param>
    /// <param name="serializerType">JSON or XML serializer</param>
    /// <returns></returns>
    public static Task Set<T>(string key, T value, DateTime? expireDate = null, StorageSerializer serializerType = StorageSerializer.JSON)
    {
      IStorageHelper<CacheObject<T>> storage = new StorageHelper<CacheObject<T>>(StorageType.Local, CacheFolder, serializerType);

      CacheObject<T> cacheFile = new CacheObject<T>() { File = value, ExpireDateTime = expireDate };

      return storage.SaveAsync(cacheFile, key);
    }

    /// <summary>
    /// Delete key from cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="serializerType">JSON or XML serializer</param>
    /// <returns></returns>
    public static Task Delete(string key, StorageSerializer serializerType = StorageSerializer.JSON)
    {
      IStorageHelper<object> storage = new StorageHelper<object>(StorageType.Local, CacheFolder, serializerType);

      return storage.DeleteAsync(key);
    }

    /// <summary>
    /// Clear the complete cache
    /// </summary>
    /// <returns></returns>
    public static Task ClearAll()
    {
      IStorageHelper<object> storage = new StorageHelper<object>(StorageType.Local, CacheFolder);
      return storage.DeleteAllFiles();
    }

    /// <summary>
    /// Clear expired cache files
    /// </summary>
    /// <returns></returns>
    public static async Task ClearInvalid(StorageSerializer serializerType = StorageSerializer.JSON)
    {
      StorageHelper<CacheObject> storage = new StorageHelper<CacheObject>(StorageType.Local, CacheFolder, serializerType);
      var validExtension = storage.GetFileExtension();
      var folder = await storage.GetFolderAsync().ConfigureAwait(false);

      var files = await folder.GetFilesAsync();

      foreach(var file in files.Where(x => x.FileType == validExtension))
      {
        var loadedFile = await storage.LoadAsync(file.DisplayName).ConfigureAwait(false);

        if(loadedFile != null && !loadedFile.IsValid)
          await file.DeleteAsync();

      }

    }

    /// <summary>
    /// Clears the cache untill the total size is below the maxSize
    /// </summary>
    /// <param name="maxSize">MaxSize in bytes</param>
    /// <returns></returns>
    public static async Task Clear(ulong maxSize)
    {
      StorageHelper<object> storage = new StorageHelper<object>(StorageType.Local, CacheFolder);
      var folder = await storage.GetFolderAsync();

      await Clear(folder, maxSize);
     
    }

    public static async Task Clear(this StorageFolder folder, ulong maxSize)
    {
      var files = await folder.GetFilesAsync();

      var list = new List<FileMetaData>();

      foreach (var file in files)
      {
        var props = await file.GetBasicPropertiesAsync();

        list.Add(new FileMetaData() { Name = file.Name, Size = props.Size, Modified = props.DateModified });
      }

      //Order list by Modified date so it deletes old files first
      list = list.OrderBy(x => x.Modified).ToList();

      await Clear(list, maxSize).ConfigureAwait(false);

    }

    private static async Task Clear(List<FileMetaData> list, ulong maxSize)
    {
      if (list == null || !list.Any())
        return;

      var total = (ulong)list.Sum(x => (long)x.Size);

      if(total > maxSize)
      {
        //Delete file
        StorageHelper<object> storage = new StorageHelper<object>(StorageType.Local, CacheFolder);
        var folder = await storage.GetFolderAsync();
        var file = await folder.GetFileAsync(list.First().Name);
        await file.DeleteAsync(StorageDeleteOption.PermanentDelete);

        list.RemoveAt(0);

        //Recursive
        await Clear(list, maxSize).ConfigureAwait(false);
     
      }
    }

    /// <summary>
    ///     Touches a file to update the DateModified property.
    ///     http://stackoverflow.com/questions/12604110/an-elegant-performant-way-to-touch-a-file-in-update-modifiedtime-in-winrt
    /// </summary>
    public static async Task TouchAsync(this StorageFile file)
    {
      using (var touch = await file.OpenTransactedWriteAsync())
      {
        await touch.CommitAsync();
      }
    }


    internal class FileMetaData
    {
      public string Name { get; set; }
      public ulong Size { get; set; }
      public DateTimeOffset Modified { get; set; }
    }
  }
}
