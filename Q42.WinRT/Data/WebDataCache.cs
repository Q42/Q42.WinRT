using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;

#if WINDOWS_PHONE
using Q42.WinRT.Phone;
#endif

namespace Q42.WinRT.Data
{
    /// <summary>
    /// Can cache Uri data
    /// </summary>
    public static class WebDataCache
    {
        private static readonly string CacheFolder = "_webdatacache";

        private static HashSet<string> _files;

        private static readonly object Lock = new object();

        public static async Task Init()
        {
            _files=new HashSet<string>();

            var folder = await GetFolderAsync().ConfigureAwait(false);
            _files.Clear();
            var files = await folder.GetFilesAsync();
            foreach (var file in files)
            {
                _files.Add(file.Name);
            }
        }

        /// <summary>
        /// Stores webdata in cache based on uri as key
        /// Returns file
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="forceGet"></param>
        /// <returns></returns>
        public async static Task<StorageFile> GetAsync(Uri uri, bool forceGet = false)
        {
            if (_files == null)
            {
                throw new Exception("Use Init to initialize the cache first");
            }

            string key = uri.ToCacheKey();

            StorageFile file = null;

            //Try get the data from the cache

            bool exist;
            lock (Lock)
            {
                exist = _files.Contains(key);
            }

            //If file is not available or we want to force getting this file
            if (!exist || forceGet)
            {
                //else, load the data
                file = await SetAsync(uri).ConfigureAwait(false);
            }

            if (file == null)
            {
                var folder = await GetFolderAsync().ConfigureAwait(false);
                file = await folder.GetFileAsync(key);
            }

            return file;
        }

        /// <summary>
        /// Stores webdata in cache based on uri as key
        /// Returns local uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async static Task<Uri> GetLocalUriAsync(Uri uri)
        {
            if (_files == null)
            {
                throw new Exception("Use Init to initialize the cache first");
            }

            //Ignore these uri schemes
            if (uri.Scheme == "ms-resource"
                || uri.Scheme == "ms-appx"
                || uri.Scheme == "ms-appdata"
              || uri.Scheme == "isostore")
                return uri;

            string key = uri.ToCacheKey();

            //Try get the data from the cache

            var contains = false;
            lock (Lock)
            {
                contains = _files.Contains(key);
            }

            if (!contains)
            {
                //else, load the data
                await SetAsync(uri).ConfigureAwait(false);
            }


#if NETFX_CORE
            string localUri = string.Format("ms-appdata:///local/{0}/{1}", CacheFolder, key);
            
#elif WINDOWS_PHONE
          string localUri = string.Format("isostore:/{0}/{1}", CacheFolder, key);
#endif


            return new Uri(localUri);

        }

        /// <summary>
        /// Get the cache folder to read/write
        /// </summary>
        /// <returns></returns>
        private static async Task<StorageFolder> GetFolderAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;

            if (!string.IsNullOrEmpty(CacheFolder))
            {
                folder = await folder.CreateFolderAsync(CacheFolder, CreationCollisionOption.OpenIfExists);
            }

            //StorageFolder.GetFolderFromPathAsync

            return folder;
        }

        /// <summary>
        /// Insert given uri in cache. Data will be loaded from the web
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static async Task<StorageFile> SetAsync(Uri uri)
        {
            string key = uri.ToCacheKey();

            var folder = await GetFolderAsync().ConfigureAwait(false);

            using (HttpClient webClient = new HttpClient())
            {
                var bytes = await webClient.GetByteArrayAsync(uri).ConfigureAwait(false);

                //Save data to cache
                var file = await folder.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteBytesAsync(file, bytes);
                lock (Lock)
                {
                    _files.Add(file.Name);
                }
                return file;
            }
        }

        /// <summary>
        /// Delete from cache based on Uri (=key)
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Task Delete(Uri uri)
        {
            if (_files == null)
            {
                throw new Exception("Use Init to initialize the cache first");
            }

            return Task.Run(async () =>
                {
                    var file = await GetAsync(uri).ConfigureAwait(false);
                    lock (Lock)
                    {
                        _files.Remove(file.Name);
                    }
                    await file.DeleteAsync();                    
                });
        }

        /// <summary>
        /// Clear the complete webcache
        /// </summary>
        /// <returns></returns>
        public static async Task ClearAll()
        {
            if (_files == null)
            {
                throw new Exception("Use Init to initialize the cache first");
            }

            var folder = await GetFolderAsync().ConfigureAwait(false);
            lock (Lock)
            {
                _files.Clear();
            }

            try
            {
                await folder.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask().ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        public static async Task Clear(ulong maxSize)
        {
            var folder = await GetFolderAsync().ConfigureAwait(false);

            await folder.Clear(maxSize).ConfigureAwait(false);
            await Init().ConfigureAwait(false);
        }

        public static async Task Clear(TimeSpan maxAge)
        {
            var folder = await GetFolderAsync().ConfigureAwait(false);

            await folder.Clear(maxAge).ConfigureAwait(false);
            await Init().ConfigureAwait(false);
        }
    }
}
