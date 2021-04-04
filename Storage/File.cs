using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Storage
{
    public interface IFile
    {
        /// <summary>Returns if a file is found in the specified storage strategy</summary>
        /// <param name="key">Path of the file in storage</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>Boolean: true if found, false if not found</returns>
        Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local);
        Task<bool> FileExistsAsync(string key, StorageFolder folder);

        /// <summary>Deletes a file in the specified storage strategy</summary>
        /// <param name="key">Path of the file in storage</param>
        /// <param name="location">Location storage strategy</param>
        Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local);

        /// <summary>Reads and deserializes a file into specified type T</summary>
        /// <typeparam name="T">Specified type into which to deserialize file content</typeparam>
        /// <param name="key">Path to the file in storage</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>Specified type T</returns>
        Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local);

        /// <summary>Serializes an object and write to file in specified storage strategy</summary>
        /// <typeparam name="T">Specified type of object to serialize</typeparam>
        /// <param name="key">Path to the file in storage</param>
        /// <param name="value">Instance of object to be serialized and written</param>
        /// <param name="location">Location storage strategy</param>
        Task<bool> WriteFileAsync<T>(string key, T value, StorageStrategies location = StorageStrategies.Local);
    }
    internal class File : IFile
    {
        private static IFile _file;
        internal static IFile Create() => _file ?? (_file = new File());

        public async Task<bool> DeleteFileAsync(string key, StorageStrategies location = StorageStrategies.Local)
        {
            var _File = await GetIfFileExistsAsync(key, location);
            if (_File != null)
                await _File.DeleteAsync();
            return !(await FileExistsAsync(key, location));
        }

        public async Task<bool> FileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local)
        {
            return (await GetIfFileExistsAsync(key, location)) != null;
        }

        public async Task<bool> FileExistsAsync(string key, StorageFolder folder)
        {
            return (await GetIfFileExistsAsync(key, folder)) != null;
        }

        public async Task<T> ReadFileAsync<T>(string key, StorageStrategies location = StorageStrategies.Local)
        {
            try
            {
                // fetch file
                var _File = await GetIfFileExistsAsync(key, location);
                if (_File == null)
                    return default;
                // read content
                var _String = await FileIO.ReadTextAsync(_File);
                // convert to obj
                var _Result = Deserialize<T>(_String);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> WriteFileAsync<T>(string key, T value, StorageStrategies location = StorageStrategies.Local)
        {
            // create file
            var file = await CreateFileAsync(key, location, CreationCollisionOption.ReplaceExisting);
            // convert to string
            var serialized = Serialize(value);
            // save string to file
            await FileIO.WriteTextAsync(file, serialized);
            // result
            return await FileExistsAsync(key, location);
        }

        private async Task<StorageFile> GetIfFileExistsAsync(string key, StorageStrategies location = StorageStrategies.Local, CreationCollisionOption option = CreationCollisionOption.FailIfExists)
        {
            StorageFile retval;
            try
            {
                switch (location)
                {
                    case StorageStrategies.Local:
                        retval = await ApplicationData.Current.LocalFolder.GetFileAsync(key);
                        break;
                    case StorageStrategies.Roaming:
                        retval = await ApplicationData.Current.RoamingFolder.GetFileAsync(key);
                        break;
                    case StorageStrategies.Temporary:
                        retval = await ApplicationData.Current.TemporaryFolder.GetFileAsync(key);
                        break;
                    default:
                        throw new NotSupportedException(location.ToString());
                }
            }
            catch (FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine("GetIfFileExistsAsync:FileNotFoundException");
                return null;
            }

            return retval;
        }

        private async Task<StorageFile> GetIfFileExistsAsync(string key, StorageFolder folder, CreationCollisionOption option = CreationCollisionOption.FailIfExists)
        {
            StorageFile retval;
            try
            {
                retval = await folder.GetFileAsync(key);
            }
            catch (FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine("GetIfFileExistsAsync:FileNotFoundException");
                return null;
            }
            return retval;
        }

        private async Task<StorageFile> CreateFileAsync(string key, StorageStrategies location = StorageStrategies.Local, CreationCollisionOption option = CreationCollisionOption.OpenIfExists)
        {
            switch (location)
            {
                case StorageStrategies.Local:
                    return await ApplicationData.Current.LocalFolder.CreateFileAsync(key, option);
                case StorageStrategies.Roaming:
                    return await ApplicationData.Current.RoamingFolder.CreateFileAsync(key, option);
                case StorageStrategies.Temporary:
                    return await ApplicationData.Current.TemporaryFolder.CreateFileAsync(key, option);
                default:
                    throw new NotSupportedException(location.ToString());
            }
        }

        private string Serialize(object objectToSerialize)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    var serializer = new DataContractJsonSerializer(objectToSerialize.GetType());
                    serializer.WriteObject(stream, objectToSerialize);
                    stream.Position = 0;
                    StreamReader reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Serialize:" + e.Message);
                    return string.Empty;
                }
            }
        }

        private static T Deserialize<T>(string jsonString)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                try
                {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    return (T)serializer.ReadObject(stream);
                }
                catch (Exception) { throw; }
            }
        }
    }
}
