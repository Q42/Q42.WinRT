using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;
using Q42.WinRT;

namespace Q42.WinRT.Storage
{
    public enum StorageType
    {
        Roaming, Local, Temporary
    }

    /// <summary>
    /// Save object to local storage, serializes as json and writes object to a file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StorageHelper<T>
    {
        private readonly string _fileExtension = ".json";

        private ApplicationData _appData = Windows.Storage.ApplicationData.Current;
        private StorageType _storageType;
        private string _subFolder;

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
                StorageFolder folder = await GetFolderAsync();

                if (await folder.ContainsFileAsync(fileName))
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
                    StorageFolder folder = await GetFolderAsync();
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
                StorageFolder folder = await GetFolderAsync();

                if (await folder.ContainsFileAsync(fileName))
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
                case StorageType.Roaming:
                    folder = _appData.RoamingFolder;
                    break;
                case StorageType.Local:
                    folder = _appData.LocalFolder;
                    break;
                case StorageType.Temporary:
                    folder = _appData.TemporaryFolder;
                    break;
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
        /// Clear the complete cache
        /// </summary>
        /// <returns></returns>
        public static Task ClearLocalAll()
        {
            return Task.Run(async () =>
            {
                StorageHelper<object> storage = new StorageHelper<object>(StorageType.Local);
                var folder = await storage.GetFolderAsync();

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
