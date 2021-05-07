Q42.WinRT [Archived]
=========

Open source library for Universal Apps, Windows Phone and Windows 8 C#/XAML applications. This library was originally developed for Windows 8 and is fully compatible with Universal Apps (Windows Phone and Windows 8.1). There's also a version for Windows Phone 8.
The library is focused on web connected and data driven applications. It includes helpers to easily cache data from API calls and cache web images to the local storage.

This library helps to maximize code reuse between Windows 8 and Windows Phone 8

Please checkout the included sample application for Universal Apps, Windows 8 and Windows Phone.

Or download directly from NuGet:
- [Q42.WinRT on NuGet](https://nuget.org/packages/Q42.WinRT)
- [Q42.WinRT.Phone on NuGet](https://nuget.org/packages/Q42.WinRT.Phone)

Looking for the old version compatible with Windows 8.0?
[Last 8.0 commit](https://github.com/Q42/Q42.WinRT/tree/72c32592a8681154c17c246904b4d010c916bd5c) / [NuGet package 1.0.14.42](https://www.nuget.org/packages/Q42.WinRT/1.0.14.42)

## What's included?
With this library comes a fully functional sample application that shows off most of the functionality. There's also a unit test project included.

| Data     | Universal apps (W8.1 and WP81) | Windows Phone 8 | 
| ------------- |:---------:|:-------------:|
| DataLoader      | x |x | 
| DataCache      | x |x |  
| WebDataCache      | x|x|
| StorageHelper      | x|x|
| SettingsHelper      | x|x|
| ImageExtensions.CacheUri      | x|x|

| Converters     | Universal apps (W8.1 and WP81) | Windows Phone 8 | 
| ------------- |:---------:|:-------------:|
| VisibilityConverter      | x |x |  
| InverseVisibilityConverter      | x |x |  
| ByteToStringConverter      | x |x |  
| StringFormatConverter      | x||
| TextToLowerConverter      | x||

| Helpers     | Universal apps (W8.1 and WP81) | Windows Phone 8 | 
| ------------- |:---------:|:-------------:|
| Util.GetAppVersion      | x||
| Util.GetOsVersion      | x||
| Util.GetMachineName      | x||

| Controls     | Universal apps (W8.1 and WP81) | Windows Phone 8 | 
| ------------- |:---------:|:-------------:|
| WrapPanel      | x||
| Background Parallax      | x||
| UserControl Flyout      | x||

### Data
* `DataLoader` - Input a task, enables easy binding to Loading / Finished / Error properties (show progress bar as long as task is running)
* `DataCache` - Input a Cache Key and API call. Will run the task to get the response (for example a web call with json result) and write it to the cache. On next call, will return response directly from cache. Can be used in combination with DataLoader.
* `DataLoader.LoadCacheThenRefreshAsync` - Input two methods which get data from cache and web. Callback will fire twice. You can bind to the dataloader to show a progress bar.
* `WebDataCache` - Input an URI. Will get the data from the web and write raw bytes to local storage. On next call, it will return the cached data from the local storage.
* `StorageHelper` - Save and retreive objects to the local storage. Json or XML serialization is used.
* `SettingsHelper` - Save and retreive settings (wrapper around the different settings api's on Windows 8 or Windows Phone)
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

### Controls
* `WrapPanel` - WrapPanel ported from Silverlight. Allows variable sized controls and wraps to a new line when needed.
* `Background Parallax` - Creates a background parallax effect like on the Windows 8 start screen
* `UserControl Flyout` - Flyout that hosts a UserControl with custom data. Can be used to create a Settings Flyout and have full control over the layout


## How To Install?
Download the source including the sample code from GitHub or install from NuGet:
- [Q42.WinRT](https://nuget.org/packages/Q42.WinRT)
- [Q42.WinRT.Phone](https://nuget.org/packages/Q42.WinRT.Phone)

## How To Use?
Check out the included sample app and unit tests for Windows 8 and Windows Phone.

### DataLoader introduction
Let's say you have a long running async operation which gets some data from the web.

	var result = await LoadWebData();
	
You want to show a nice progress bar when you're busy loading the data. Use the DataLoader for that, it wraps around your existing code. Place this code in your ViewModel:

	//public property you can bind to
	public DataLoader DL { get; set; }
	
	//Place this in the constructor of your ViewModel:
	DL = new DataLoader();
	
	//Somewhere where you want to start loading data
	var result = await DL.LoadAsync(() => LoadWebData());
	
You can now bind your ProgressBar to the DataLoader's IsBusy property in your XAML:
	
	<ProgressBar IsIndeterminate="{Binding DL.IsBusy}"></ProgressBar>
	
For more advanced scenarios, see the included sample apps and unit tests.

### DataCache introduction
Let's say you want to cache the result when you grab some data from an external source

	List<MyData> result = await DataCache.GetAsync("your_key", () => LoadWebData());
	
The next time you do the same call, you get the cached result.
You can combine this with the DataLoader to show a ProgressBar:

	List<MyData> result = await DL.LoadAsync(() => DataCache.GetAsync("your_key", () => LoadWebData()));

For more advanced scenarios, see the included sample apps and unit tests.

## License
Q42.WinRT is licensed under [MIT](http://www.opensource.org/licenses/mit-license.php "Read more about the MIT license form"). Refer to [license.txt](https://github.com/Q42/Q42.WinRT/blob/master/LICENSE.txt) for more information.

## Contributions
Contributions are welcome. Fork this repository and send a pull request if you have something useful to add.

## Credits
* [Michiel Post](http://www.michielpost.nl) ([@michielpostnl](http://twitter.com/michielpostnl)) - core contributor
* [Q42](http://www.q42.nl) ([@q42](http://twitter.com/q42)) - Inventors of products, games and solutions from the Netherlands.

### Open Source Project Credits
Some code is used from other open source projects.

* Newtonsoft.Json is used for object serialization
* WrapPanel is based on http://www.codeproject.com/Articles/24141/WrapPanel-for-Silverlight-2-0
* ParallaxConverter idea is based on http://w8isms.blogspot.nl/2012/06/metro-parallax-background-in-xaml.html

## Related Projects
Other useful WinRT / Windows 8 projects.

* [WinRT XAML Toolkit](http://winrtxamltoolkit.codeplex.com) Lots of UI controls and helpers for XAML projects
* [Callisto](https://github.com/timheuer/callisto) Controls like SettingsFlyout, Menu and more.
