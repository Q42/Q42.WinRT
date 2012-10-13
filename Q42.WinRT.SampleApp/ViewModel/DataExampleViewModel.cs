using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Q42.WinRT.Data;

namespace Q42.WinRT.SampleApp.ViewModel
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


        public RelayCommand StartLongRunningCommand { get; set; }
        public RelayCommand StartLongRunningWithExceptionCommand { get; set; }
        public RelayCommand CacheCommand { get; set; }
        public RelayCommand CacheWithExceptionCommand { get; set; }
        public RelayCommand SourceABCommand { get; set; }
        public RelayCommand CacheRefreshCommand { get; set; }
        public RelayCommand ClearCacheCommand { get; set; }

        public RelayCommand ClearWebDataCacheCommand { get; set; }
        public RelayCommand GetUriCommand { get; set; }

        public DataExampleViewModel()
        {
            StartLongRunningDataLoader = new DataLoader();
            StartLongRunningWithExceptionDataLoader = new DataLoader(true); //swallow exceptions
            CacheDataLoader = new DataLoader();
            CacheWithExceptionDataLoader = new DataLoader(true); //swallow exceptions
            SourceABDataLoader = new DataLoader(true); //swallow exceptions
            CacheRefreshDataLoader = new DataLoader(true); //swallow exceptions


            StartLongRunningCommand = new RelayCommand(() => StartLongRunningAction());
            StartLongRunningWithExceptionCommand = new RelayCommand(() => StartLongRunningWithExceptionAction());
            CacheCommand = new RelayCommand(() => CacheAction());
            CacheWithExceptionCommand = new RelayCommand(() => CacheWithExceptionAction());
            SourceABCommand = new RelayCommand(() => SourceABAction());
            CacheRefreshCommand = new RelayCommand(() => CacheRefreshAction());
            ClearCacheCommand = new RelayCommand(() => ClearCacheAction());

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
            string result = await CacheDataLoader.LoadAsync(() => JsonCache.GetAsync("samplekey", () => LongRunningOperation()));
        }


        /// <summary>
        /// Get from cache with Exception result
        /// </summary>
        private async void CacheWithExceptionAction()
        {
            //Fire task
            //Will show loader in the UI
            string result = await CacheWithExceptionDataLoader.LoadAsync(() => JsonCache.GetAsync("samplekey_exception", () => LongRunningOperationWithException(), expireDate: DateTime.Now.AddDays(1)));


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

        /// <summary>
        /// First return cached result, then refresh this result with live value
        /// </summary>
        private void CacheRefreshAction()
        {
            CacheRefreshResult = string.Empty;

            //Fire task
            //Will show loader in the UI
            CacheRefreshDataLoader.LoadCacheThenRefreshAsync(() => JsonCache.GetFromCache<string>("key6"), () => JsonCache.GetAsync("key6", () => LongRunningOperation(DateTime.Now.Second.ToString()), expireDate: DateTime.Now.AddDays(1), forceRefresh: true), x =>
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
        /// Clears the JsonCache
        /// </summary>
        private void ClearCacheAction()
        {
            JsonCache.ClearAll();
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
