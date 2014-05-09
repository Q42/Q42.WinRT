using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Q42.WinRT.Data;
using Q42.WinRT.Portable.Data;

namespace Q42.WinRT.UniversalSampleApp.ViewModel
{
    public class DataExampleViewModel : ViewModelBase
    {
        private string _result;

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        private string _result2;

        public string Result2
        {
            get { return _result2; }
            set
            {
                _result2 = value;
                RaisePropertyChanged("Result2");
            }
        }

        private string _result3;

        public string Result3
        {
            get { return _result3; }
            set
            {
                _result3 = value;
                RaisePropertyChanged("Result3");
            }
        }

        private string _cacheRefreshResult;

        public string CacheRefreshResult
        {
            get { return _cacheRefreshResult; }
            set
            {
                _cacheRefreshResult = value;
                RaisePropertyChanged("CacheRefreshResult");
            }
        }
        

        public DataLoader StartLongRunningDataLoader { get; set; }
        public DataLoader StartLongRunningWithExceptionDataLoader { get; set; }
        public DataLoader CacheDataLoader { get; set; }
        public DataLoader CacheWithExceptionDataLoader { get; set; }
        public DataLoader SourceABDataLoader { get; set; }
        public DataLoader CacheRefreshDataLoader { get; set; }
        public DataLoader FailCacheDataLoader { get; set; }
        public DataLoader FailCacheSuccessDataLoader { get; set; }



        public RelayCommand StartLongRunningCommand { get; set; }
        public RelayCommand StartLongRunningWithExceptionCommand { get; set; }
        public RelayCommand CacheCommand { get; set; }
        public RelayCommand CacheWithExceptionCommand { get; set; }
        public RelayCommand SourceABCommand { get; set; }
        public RelayCommand CacheRefreshCommand { get; set; }
        public RelayCommand ClearCacheCommand { get; set; }
        public RelayCommand FailCacheCommand { get; set; }
        public RelayCommand FailCacheSuccessCommand { get; set; }

        public RelayCommand ClearWebDataCacheCommand { get; set; }
        public RelayCommand GetUriCommand { get; set; }

        public DataExampleViewModel()
        {
            StartLongRunningDataLoader = new DataLoader();
            StartLongRunningWithExceptionDataLoader = new DataLoader(); //swallow exceptions by default
            CacheDataLoader = new DataLoader();
            CacheWithExceptionDataLoader = new DataLoader(); //swallow exceptions by default
            SourceABDataLoader = new DataLoader(); //swallow exceptions by default
            CacheRefreshDataLoader = new DataLoader(); //swallow exceptions by default
            FailCacheDataLoader = new DataLoader(); 
            FailCacheSuccessDataLoader = new DataLoader();


            StartLongRunningCommand = new RelayCommand(() => StartLongRunningAction());
            StartLongRunningWithExceptionCommand = new RelayCommand(() => StartLongRunningWithExceptionAction());
            CacheCommand = new RelayCommand(() => CacheAction());
            CacheWithExceptionCommand = new RelayCommand(() => CacheWithExceptionAction());
            SourceABCommand = new RelayCommand(() => SourceABAction());
            CacheRefreshCommand = new RelayCommand(() => CacheRefreshAction());
            ClearCacheCommand = new RelayCommand(() => ClearCacheAction());
            FailCacheCommand = new RelayCommand(() => FailCacheAction());
            FailCacheSuccessCommand = new RelayCommand(() => FailCacheSuccessAction());

            ClearWebDataCacheCommand = new RelayCommand(() => ClearWebDataCacheCommandAction());
            GetUriCommand = new RelayCommand(() => GetUriCommandAction());

        }

       
        /// <summary>
        /// Shows data loader for long running operation
        /// </summary>
        private async void StartLongRunningAction()
        {
            //Fire task
            //Will show loader in the UI
            string result = await StartLongRunningDataLoader.LoadAsync(() => LongRunningOperation());
        }

        /// <summary>
        /// Long running operation with exception as result
        /// </summary>
        private async void StartLongRunningWithExceptionAction()
        {
            //Fire task
            //Will show loader in the UI
            string result = await StartLongRunningWithExceptionDataLoader.LoadAsync(() => LongRunningOperationWithException());
        }

        /// <summary>
        /// Get data from cache (or insert into cache if it's not there yet)
        /// </summary>
        private async void CacheAction()
        {
            //Fire task
            //Will show loader in the UI
            string result = await CacheDataLoader.LoadAsync(() => DataCache.GetAsync("samplekey", () => LongRunningOperation()));
        }


        /// <summary>
        /// Get from cache with Exception result
        /// </summary>
        private async void CacheWithExceptionAction()
        {
            //Fire task
            //Will show loader in the UI
            string result = await CacheWithExceptionDataLoader.LoadAsync(() => DataCache.GetAsync("samplekey_exception", () => LongRunningOperationWithException(), expireDate: DateTime.Now.AddDays(1)));


        }

        /// <summary>
        /// First load data from source A then source B
        /// </summary>
        private void SourceABAction()
        {
            Result = string.Empty;

            //Fire task
            //Will show loader in the UI
            SourceABDataLoader.LoadCacheThenRefreshAsync(() => LongRunningOperation("first result"), () => LongRunningOperation("refresh result"), x =>
                {
                    Result = x;
                });
        }

        private void FailCacheAction()
        {
            Result3 = string.Empty;

            //Fire task
            //Will show loader in the UI
            FailCacheDataLoader.LoadFallbackToCacheAsync(() => LongRunningOperationWithException(), () => LongRunningOperation("source B result"), x =>
            {
                Result3 = x;
            });
        }

        private void FailCacheSuccessAction()
        {
            Result2 = string.Empty;

            //Fire task
            //Will show loader in the UI
            FailCacheSuccessDataLoader.LoadFallbackToCacheAsync(() => LongRunningOperation("source A result"), () => LongRunningOperation("source B result"), x =>
            {
                Result2 = x;
            });
        }



        /// <summary>
        /// First return cached result, then refresh this result with live value
        /// </summary>
        private void CacheRefreshAction()
        {
            CacheRefreshResult = string.Empty;

            //Fire task
            //Will show loader in the UI
            CacheRefreshDataLoader.LoadCacheThenRefreshAsync(() => DataCache.GetFromCache<string>("key6"), () => DataCache.GetAsync("key6", () => LongRunningOperation(DateTime.Now.Second.ToString()), expireDate: DateTime.Now.AddDays(1), forceRefresh: true), x =>
            {
                CacheRefreshResult = x;
            });
        }

        /// <summary>
        /// Load uri and set data in cache
        /// </summary>
        private async void GetUriCommandAction()
        {
            var result = await WebDataCache.GetAsync(new Uri("http://microsoft.com"), forceGet: false);

            if (result == null)
            {
            }
        }

        /// <summary>
        /// Clear WebDataCache
        /// </summary>
        private void ClearWebDataCacheCommandAction()
        {
            WebDataCache.ClearAll();
        }

        /// <summary>
        /// Clears the DataCache
        /// </summary>
        private void ClearCacheAction()
        {
            DataCache.ClearAll();
        }



        /// <summary>
        /// Sample long running operation
        /// </summary>
        /// <returns></returns>
        private async Task<string> LongRunningOperation()
        {
            //For example, webservice call

            await Task.Delay(2000);

            return "result";
        }

        /// <summary>
        /// Sample long running operation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> LongRunningOperation(string input)
        {
            //For example, webservice call

            await Task.Delay(2000);

            return "Input: " + input;
        }

        /// <summary>
        /// Sample long running operation
        /// </summary>
        /// <returns></returns>
        private async Task<string> LongRunningOperationWithException()
        {
            //For example, webservice call

            await Task.Delay(2000);

            throw new Exception("Error");
        }

    }
}
