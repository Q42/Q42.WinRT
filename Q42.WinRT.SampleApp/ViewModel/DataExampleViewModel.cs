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

        private string _result6;

        public string Result6
        {
            get { return _result6; }
            set
            {
                _result6 = value;
                RaisePropertyChanged("Result6");
            }
        }
        

        public DataLoader MyDataLoader { get; set; }
        public DataLoader MyDataLoader2 { get; set; }
        public DataLoader MyDataLoader3 { get; set; }
        public DataLoader MyDataLoader4 { get; set; }
        public DataLoader MyDataLoader5 { get; set; }
        public DataLoader MyDataLoader6 { get; set; }


        public RelayCommand Button1Command { get; set; }
        public RelayCommand Button2Command { get; set; }
        public RelayCommand Button3Command { get; set; }
        public RelayCommand Button4Command { get; set; }
        public RelayCommand Button5Command { get; set; }
        public RelayCommand Button6Command { get; set; }
        public RelayCommand ClearCacheCommand { get; set; }

        public RelayCommand ClearWebDataCacheCommand { get; set; }
        public RelayCommand GetUriCommand { get; set; }

        public DataExampleViewModel()
        {
            MyDataLoader = new DataLoader();
            MyDataLoader2 = new DataLoader(true); //swallow exceptions
            MyDataLoader3 = new DataLoader();
            MyDataLoader4 = new DataLoader(true); //swallow exceptions
            MyDataLoader5 = new DataLoader(true); //swallow exceptions
            MyDataLoader6 = new DataLoader(true); //swallow exceptions


            Button1Command = new RelayCommand(() => Button1Action());
            Button2Command = new RelayCommand(() => Button2Action());
            Button3Command = new RelayCommand(() => Button3Action());
            Button4Command = new RelayCommand(() => Button4Action());
            Button5Command = new RelayCommand(() => Button5Action());
            Button6Command = new RelayCommand(() => Button6Action());
            ClearCacheCommand = new RelayCommand(() => ClearCacheAction());

            ClearWebDataCacheCommand = new RelayCommand(() => ClearWebDataCacheCommandAction());
            GetUriCommand = new RelayCommand(() => GetUriCommandAction());

        }

      
        private async void Button1Action()
        {
            //Fire task
            //Will show loader in the UI
            string result = await MyDataLoader.LoadAsync(() => LongRunningOperation());
        }

        private async void Button2Action()
        {
            //Fire task
            //Will show loader in the UI
            string result = await MyDataLoader2.LoadAsync(() => LongRunningOperationWithException());
        }

        private async void Button3Action()
        {
            //Fire task
            //Will show loader in the UI
            string result = await MyDataLoader3.LoadAsync(() => JsonCache.GetAsync("samplekey", () => LongRunningOperation()));
        }

        private async void Button4Action()
        {
            //Fire task
            //Will show loader in the UI
            //string result = await MySecondDataLoader.Load(() => LongRunningOperationWithException());

            string result = await MyDataLoader4.LoadAsync(() => JsonCache.GetAsync("samplekey_exception", () => LongRunningOperationWithException(), expireDate: DateTime.Now.AddDays(1)));


        }

        private void Button5Action()
        {
            Result = string.Empty;

            //Fire task
            //Will show loader in the UI
            MyDataLoader5.LoadCacheThenRefreshAsync(() => LongRunningOperation("first result"), () => LongRunningOperation("refresh result"), x =>
                {
                    Result = x;
                });
        }

        private void Button6Action()
        {
            Result6 = string.Empty;

            //Fire task
            //Will show loader in the UI
            MyDataLoader6.LoadCacheThenRefreshAsync(() => JsonCache.GetFromCache<string>("key6"), () => JsonCache.GetAsync("key6", () => LongRunningOperation(DateTime.Now.Second.ToString()), expireDate: DateTime.Now.AddDays(1), forceRefresh: true), x =>
            {
                Result6 = x;
            });
        }

        private async void GetUriCommandAction()
        {
            var result = await WebDataCache.GetAsync(new Uri("http://microsoft.com"), forceGet: false);

            if (result == null)
            {
            }
        }


        private void ClearWebDataCacheCommandAction()
        {
            WebDataCache.ClearAll();
        }

        private void ClearCacheAction()
        {
            JsonCache.ClearAll();
        }



        private async Task<string> LongRunningOperation()
        {
            //For example, webservice call

            await Task.Delay(2000);

            return "result";
        }

        private async Task<string> LongRunningOperation(string input)
        {
            //For example, webservice call

            await Task.Delay(2000);

            return "Input: " + input;
        }

        private async Task<string> LongRunningOperationWithException()
        {
            //For example, webservice call

            await Task.Delay(2000);

            throw new Exception("Error");
        }
    }
}
