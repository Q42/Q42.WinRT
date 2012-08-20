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
        /// <param name="forceRefresh"></param>
        /// <returns></returns>
        public async static Task<T> GetAsync<T>(string key, Func<Task<T>> generate, bool forceRefresh = false)
        {
            object value;

            //Force bypass of cache?
            if (!forceRefresh)
            {
                //Check cache
                value = await GetFromCache<T>(key);
                if (value != null)
                {
                    return (T)value;
                }
            }

            value = await generate();
            await Set(key, value);

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
            StorageHelper<T> storage = new StorageHelper<T>(StorageType.Local, CacheFolder);

            //Get cache value
            object value = await storage.LoadAsync(key);

            return (T)value;
        }

        /// <summary>
        /// Set value in cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task Set<T>(string key, T value)
        {
            StorageHelper<T> storage = new StorageHelper<T>(StorageType.Local, CacheFolder);
            return storage.SaveAsync(value, key);
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
                    var folder = await storage.GetFolderAsync();

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
