using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Q42.WinRT.Phone
{
  /// <summary>
  /// FileIO helper because FileIO is not available in Windows Phone
  /// </summary>
  internal static class FileIO
  {
    internal async static Task WriteTextAsync(IStorageFile storageFile, string text)
    {
      using (Stream stream = await storageFile.OpenStreamForWriteAsync())
      {
        byte[] content = Encoding.UTF8.GetBytes(text);
        await stream.WriteAsync(content, 0, content.Length);
      }
    }

    internal async static Task WriteBytesAsync(IStorageFile storageFile, byte[] bytes)
    {
      using (Stream stream = await storageFile.OpenStreamForWriteAsync())
      {
        await stream.WriteAsync(bytes, 0, bytes.Length);
      }
    }

    internal async static Task<string> ReadTextAsync(IStorageFile storageFile)
    {
      string text;

      using (IRandomAccessStream accessStream = await storageFile.OpenReadAsync())
      {
        using (Stream stream = accessStream.AsStreamForRead((int)accessStream.Size))
        {
          byte[] content = new byte[stream.Length];
          await stream.ReadAsync(content, 0, (int)stream.Length);

          text = Encoding.UTF8.GetString(content, 0, content.Length);
        }
      }

      return text;
    }
  }
}
