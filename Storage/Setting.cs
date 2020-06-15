using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace Storage
{
    public interface ISetting
    {
        /// <summary>Register for DataChanged Event</summary>
        event TypedEventHandler<ApplicationData, object> ApplicationDataChanged;
        
        /// <summary>Returns if a setting is found in the specified storage strategy</summary>
        /// <param name="key">Key of the setting in storage</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>Boolean: true if found, false if not found</returns>
        bool SettingExists(string key, StorageStrategies location = StorageStrategies.Local);

        /// <summary>Reads and converts a setting into specified type T</summary>
        /// <typeparam name="T">Specified type into which to value is converted</typeparam>
        /// <param name="key">Key of the setting in storage</param>
        /// <param name="otherwise">Return value if key is not found or convert fails</param>
        /// <param name="location">Location storage strategy</param>
        /// <returns>Specified type T</returns>
        T GetSetting<T>(string key, T otherwise = default(T), StorageStrategies location = StorageStrategies.Local);

        /// <summary>Sets a setting in specified storage strategy</summary>
        /// <typeparam name="T">Specified type of object to serialize</typeparam>
        /// <param name="key">Key of the setting in storage</param>
        /// <param name="value">Instance of object to be serialized and written</param>
        /// <param name="location">Location storage strategy</param>
        void SetSetting<T>(string key, T value, StorageStrategies location = StorageStrategies.Local);

        /// <summary>Deletes a setting in specified storage strategy</summary>        
        /// <param name="key">Path to the file in storage</param>        
        /// <param name="location">Location storage strategy</param>
        void DeleteSetting(string key, StorageStrategies location = StorageStrategies.Local);
    }
    internal class Setting : ISetting
    {
        private static ISetting _setting;

        public event TypedEventHandler<ApplicationData, object> ApplicationDataChanged
        {
            add { ApplicationData.Current.DataChanged += value; }
            remove { ApplicationData.Current.DataChanged -= value; }
        }

        internal static ISetting Create() => _setting ?? (_setting = new Setting());

        public void DeleteSetting(string key, StorageStrategies location = StorageStrategies.Local)
        {
            switch (location)
            {
                case StorageStrategies.Local:
                    ApplicationData.Current.LocalSettings.Values.Remove(key);
                    break;
                case StorageStrategies.Roaming:
                    ApplicationData.Current.RoamingSettings.Values.Remove(key);
                    break;
                default:
                    throw new NotSupportedException(location.ToString());
            }
        }

        public T GetSetting<T>(string key, T otherwise = default, StorageStrategies location = StorageStrategies.Local)
        {
            try
            {
                if (!(SettingExists(key, location)))
                    return otherwise;
                switch (location)
                {
                    case StorageStrategies.Local:
                        return (T)ApplicationData.Current.LocalSettings.Values[key.ToString()];
                    case StorageStrategies.Roaming:
                        return (T)ApplicationData.Current.RoamingSettings.Values[key.ToString()];
                    default:
                        throw new NotSupportedException(location.ToString());
                }
            }
            catch { return otherwise; }
        }

        public void SetSetting<T>(string key, T value, StorageStrategies location = StorageStrategies.Local)
        {
            switch (location)
            {
                case StorageStrategies.Local:
                    ApplicationData.Current.LocalSettings.Values[key.ToString()] = value;
                    break;
                case StorageStrategies.Roaming:
                    ApplicationData.Current.RoamingSettings.Values[key.ToString()] = value;
                    break;
                default:
                    throw new NotSupportedException(location.ToString());
            }
        }

        public bool SettingExists(string key, StorageStrategies location = StorageStrategies.Local)
        {
            switch (location)
            {
                case StorageStrategies.Local:
                    return ApplicationData.Current.LocalSettings.Values.ContainsKey(key);
                case StorageStrategies.Roaming:
                    return ApplicationData.Current.RoamingSettings.Values.ContainsKey(key);
                default:
                    throw new NotSupportedException(location.ToString());
            }
        }
    }
}
