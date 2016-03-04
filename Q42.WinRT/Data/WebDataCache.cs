using Q42.WinRT.Storage;
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
        private static readonly string IndexCacheFile = "_index";

        private static Dictionary<string, string> _files;

        private static readonly object Lock = new object();

        private static StorageFolder _storageFolder;

        public static async Task Init(StorageFolder folder = null)
        {
          if (folder == null)
            folder = Windows.Storage.ApplicationData.Current.LocalFolder;
          _storageFolder = folder;

          _files = await GetIndexFile().ConfigureAwait(false);

          if (_files == null)
          {

            _files = new Dictionary<string, string>();

            await SaveIndexFile().ConfigureAwait(false);

          }

        }

        private static async Task SaveIndexFile()
        {
            try
            {
                var folder = await GetFolderAsync().ConfigureAwait(false);

                IStorageHelper<Dictionary<string, string>> storage = new StorageHelper<Dictionary<string, string>>(_storageFolder, CacheFolder, StorageSerializer.JSON);

                await storage.SaveAsync(_files, IndexCacheFile);
            }
            catch (Exception)
            {

            }


        }


        /// <summary>
        /// Get index file
        /// </summary>
        /// <returns></returns>
        public async static Task<Dictionary<string, string>> GetIndexFile()
        {

            var folder = await GetFolderAsync().ConfigureAwait(false);

            IStorageHelper<Dictionary<string, string>> storage = new StorageHelper<Dictionary<string, string>>(_storageFolder, CacheFolder, StorageSerializer.JSON);

            //Get cache value
            var value = await storage.LoadAsync(IndexCacheFile).ConfigureAwait(false);

            if (value == null) { return default(Dictionary<string, string>); }
            else
                return value;

        }

        /// <summary>
        /// Returns file reference in dictionary
        /// </summary>
        /// <param name="sUri"></param>
        /// <returns></returns>
        public static KeyValuePair<string, string> GetFileReferenceByUri(string sUri)
        {
            return _files.Where(x => x.Value == sUri).FirstOrDefault();
        }

        /// <summary>
        /// Returns file reference in dictionary
        /// </summary>
        /// <param name="sUri"></param>
        /// <returns></returns>
        public static KeyValuePair<string, string> GetFileReferenceByUri(Uri uri)
        {
            return GetFileReferenceByUri(uri.ToString());
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

            string sUri = uri.ToString();

            StorageFile file = null;

            //Try get the data from the cache

            bool exist = false;
            KeyValuePair<string, string> fileRef;

            lock (Lock)
            {
                fileRef = GetFileReferenceByUri(sUri);
                exist = !string.IsNullOrEmpty(fileRef.Key);
            }

            //If file is not available or we want to force getting this file
            if (!exist || forceGet)
            {
                //else, load the data
                fileRef = await SetAsyncRef(uri).ConfigureAwait(false);
                var folder = await GetFolderAsync().ConfigureAwait(false);
                file = await folder.GetFileAsync(fileRef.Key).AsTask().ConfigureAwait(false);
            }

            if (file == null)
            {
                var folder = await GetFolderAsync().ConfigureAwait(false);
                file = await folder.GetFileAsync(fileRef.Key);
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

            string value = uri.ToString();

            //Try get the data from the cache

            var contains = false;
            KeyValuePair<string, string> fileRef;

            lock (Lock)
            {
                fileRef = GetFileReferenceByUri(uri.ToString());
                contains = !string.IsNullOrEmpty(fileRef.Key);
            }

            if (!contains)
            {
                //else, load the data
                fileRef = await SetAsyncRef(uri).ConfigureAwait(false);
            }


#if NETFX_CORE
            string localUri = string.Format("ms-appdata:///local/{0}/{1}", CacheFolder, fileRef.Key);

#elif WINDOWS_PHONE
            string localUri = string.Format("isostore:/{0}/{1}", CacheFolder, fileRef.Key);
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
        private static async Task<KeyValuePair<string,string>> SetAsyncRef(Uri uri)
        {
            string sUri = uri.ToString();

            var reg = new KeyValuePair<string, string>(Guid.NewGuid().ToString(), sUri);

            var folder = await GetFolderAsync().ConfigureAwait(false);

            using (HttpClient webClient = new HttpClient())
            {
                var bytes = await webClient.GetByteArrayAsync(uri).ConfigureAwait(false);

                //Save data to cache
                var file = await folder.CreateFileAsync(reg.Key, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteBytesAsync(file, bytes);
                lock (Lock)
                {
                    _files.Add(reg.Key, reg.Value);
                }
                await SaveIndexFile().ConfigureAwait(false);
                return reg;
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
                        var reg = _files.Where(x => x.Key == file.Name).FirstOrDefault();

                        if (!reg.Equals(default(KeyValuePair<string, string>)))
                        {
                            _files.Remove(reg.Key);
                        }
                    }

                    await file.DeleteAsync();
                    await SaveIndexFile().ConfigureAwait(false);
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
                await folder.DeleteAsync().AsTask().ConfigureAwait(false);
                await SaveIndexFile().ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException uex)
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
