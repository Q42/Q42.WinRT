using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q42.WinRT.Storage;
using Windows.Storage;

namespace Q42.WinRT.Data
{
  /// <summary>
  /// Used as a wrapper around the stored file to keep metadata
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class CacheObject<T>
  {
    /// <summary>
    /// Expire date of cached file
    /// </summary>
    public DateTime? ExpireDateTime { get; set; }

    /// <summary>
    /// Actual file being stored
    /// </summary>
    public T File { get; set; }

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
  /// Stores objects as json in the localstorage
  /// </summary>
  public static class JsonCache
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
    /// <returns></returns>
    public async static Task<T> GetAsync<T>(string key, Func<Task<T>> generate, DateTime? expireDate = null, bool forceRefresh = false)
    {
      object value;

      //Force bypass of cache?
      if (!forceRefresh)
      {
        //Check cache
        value = await GetFromCache<T>(key).ConfigureAwait(false);
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
    /// <returns></returns>
    public async static Task<T> GetFromCache<T>(string key)
    {
      StorageHelper<CacheObject<T>> storage = new StorageHelper<CacheObject<T>>(StorageType.Local, CacheFolder);

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
        Delete(key);

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
    /// <returns></returns>
    public static Task Set<T>(string key, T value, DateTime? expireDate = null)
    {
      StorageHelper<CacheObject<T>> storage = new StorageHelper<CacheObject<T>>(StorageType.Local, CacheFolder);

      CacheObject<T> cacheFile = new CacheObject<T>() { File = value, ExpireDateTime = expireDate };

      return storage.SaveAsync(cacheFile, key);
    }

    /// <summary>
    /// Delete key from cache
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Task Delete(string key)
    {
      StorageHelper<object> storage = new StorageHelper<object>(StorageType.Local, CacheFolder);
      return storage.DeleteAsync(key);
    }

    /// <summary>
    /// Clear the complete cache
    /// </summary>
    /// <returns></returns>
    public static Task ClearAll()
    {
      return Task.Run(async () =>
          {
            StorageHelper<object> storage = new StorageHelper<object>(StorageType.Local, CacheFolder);
            var folder = await storage.GetFolderAsync().ConfigureAwait(false);

            try
            {
              await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (UnauthorizedAccessException)
            {
            }

            //foreach (var file in await folder.GetFilesAsync())
            //{
            //    await file.DeleteAsync();
            //}

          });
    }
  }
}
