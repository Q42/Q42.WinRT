#if NETFX_CORE
using Windows.Storage;
#elif WINDOWS_PHONE
using System.IO.IsolatedStorage;
#endif

namespace Q42.WinRT.Storage
{
  /// <summary>
  /// Settings helper for compatibility between Windows Phone and Windows 8
  /// </summary>
  public static class SettingsHelper
  {

    /// <summary>
    /// Set setting value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void Set<T>(string key, T value)
    {
#if NETFX_CORE
      ApplicationData.Current.LocalSettings.Values[key] = value;
#elif WINDOWS_PHONE
      IsolatedStorageSettings.ApplicationSettings[key] = value;
      IsolatedStorageSettings.ApplicationSettings.Save();
#endif
    }


    /// <summary>
    /// Check if the key exists
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool Contains(string key)
    {
      bool isContains = false;
#if NETFX_CORE
      isContains = ApplicationData.Current.LocalSettings.Values.Keys.Contains(key);
#elif WINDOWS_PHONE
      isContains = IsolatedStorageSettings.ApplicationSettings.Contains(key);
#endif
      return isContains;
    }


    /// <summary>
    /// Gets value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T Get<T>(string key)
    {
      T value = default(T);
#if NETFX_CORE
      value = (T)ApplicationData.Current.LocalSettings.Values[key];
#elif WINDOWS_PHONE
      try
      {
        value = (T)IsolatedStorageSettings.ApplicationSettings[key];
      }
      catch (System.Collections.Generic.KeyNotFoundException)
      {
        value = default(T);
      }
#endif
      return value;
    }


    /// <summary>
    /// Gets value, returns default value if value does not exist
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T Get<T>(string key, T defaultValue)
    {
      bool isContains = SettingsHelper.Contains(key);
      if (!isContains)
      {
        return defaultValue;
      }
      return Get<T>(key);
    }

    /// <summary>
    /// Removes a value from the settings
    /// </summary>
    /// <param name="key"></param>
    public static void Remove(string key)
    {
#if NETFX_CORE
      ApplicationData.Current.LocalSettings.Values.Remove(key);
#elif WINDOWS_PHONE
      IsolatedStorageSettings.ApplicationSettings.Remove(key);
#endif
    }
  }
}
