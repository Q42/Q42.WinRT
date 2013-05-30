Q42.WinRT
=========

Open source library for Windows Phone and Windows 8 C#/XAML applications. This library was originally developed for Windows 8, but now also includes a Portable Class Library which is compatible with WIndows Phone.
The library is focused on web connected and data driven applications. It includes helpers to easily cache data from API calls and cache web images to the local storage.

### Included in PCL (Compatible with Windows Phone and Win8)
DataLoader which functions as a wrapper around a Task<T> method. You can bind a ProgressBar or ProgressRing to the DataLoader and show it as long as the task is running.

Please checkout the included sample application.

Or download directly from NuGet:
[Q42.WinRT on NuGet](https://nuget.org/packages/Q42.WinRT)
[Q42.WinRT.Portable on NuGet](https://nuget.org/packages/Q42.WinRT.Portable)

## What`s included?
With this library comes a fully functional sample application that shows off most of the functionality. There`s also a unit test project included.

### Controls
* `WrapPanel` - WrapPanel ported from Silverlight. Allows variable sized controls and wraps to a new line when needed.
* `Background Parallax` - Creates a background parallax effect like on the Windows 8 start screen
* `Settings Flyout` - Flyout that hosts a UserControl with custom data. Can be used to create a Settings Flyout and have full control over the layout

### Data
* `DataLoader` - Input a task, enables easy binding to Loading / Finished / Error properties (show progress bar as long as task is running)
* `JsonCache` - Input a Cache Key and API call. Will run the task to get the response (for example a web call with json result) and write it to the cache. On next call, will return response directly from cache. Can be used in combination with DataLoader.
* `DataLoader.LoadCacheThenRefreshAsync` - Input two methods which get data from cache and web. Callback will fire twice. You can bind to the dataloader to show a progress bar.
* `WebDataCache` - Input an URI. Will get the data from the web and write raw bytes to local storage. On next call, it will return the cached data from the local storage.
* `StorageHelper` - Save and retreive objects to the local storage. Json serialization is used.
* `ImageExtensions.CacheUri` - Alternative to Source property for images. Will get image from web and cache it. On a next run, will return cached image.

### Converters
* `VisibilityConverter` - Convert anything to visibility, works with bools, ints, strings, objects etc
* `InverseVisibilityConverter` - Inverse of the VisibilityConverter
* `ByteToStringConverter` - Converts bytes to a nice string, like 5.14 MB
* `StringFormatConverter` - String format helper
* `TextToLowerConverter` - Converts all text to lower text

### Helpers
* `Util.GetAppVersion` - Gets the application version as formatted string (1.0.0.0)
* `Util.GetOsVersion` - Gets the version of the OS (for now: Windows NT 6.2, can change in the future)
* `Util.GetMachineName` - Returns the machine name

## How To install?
Download the source including the sample code from GitHub or get the assembly from NuGet [Q42.WinRT on NuGet](https://nuget.org/packages/Q42.WinRT).

## License
Q42.WinRT is licensed under [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form"). Refer to [license.txt](https://github.com/Q42/Q42.WinRT/blob/master/LICENSE.txt) for more information.

## Contributions
Contributions are welcome. Fork this repository and send a pull request if you have something useful to add.

## Credits
* [Q42](http://www.q42.nl) ([@q42](http://twitter.com/q42)) - Inventors of products, games and solutions from the Netherlands.
* [Michiel Post](http://www.michielpost.nl) ([@michielpostnl](http://twitter.com/michielpostnl)) - core contributor

### Open Source Project Credits
Some code is used from other open source projects.

* Newtonsoft.Json is used for object serialization
* WrapPanel is based on http://www.codeproject.com/Articles/24141/WrapPanel-for-Silverlight-2-0
* ParallaxConverter idea is based on http://w8isms.blogspot.nl/2012/06/metro-parallax-background-in-xaml.html

## Related Projects
Other useful WinRT / Windows 8 projects.

* [WinRT XAML Toolkit](http://winrtxamltoolkit.codeplex.com) Lots of UI controls and helpers for XAML projects
* [Callisto](https://github.com/timheuer/callisto) Controls like SettingsFlyout, Menu and more.