using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Q42.WinRT.Storage;

namespace Q42.WinRT.SampleApp.ViewModel
{
    public class StorageExampleViewModel : ViewModelBase
    {

        private StorageHelper<List<MyModel>> storageHelper = new StorageHelper<List<MyModel>>(StorageType.Local);

        private string _result;

        public string Result
        {
            get { return _result; }
            set { _result = value;
            RaisePropertyChanged(() => Result);
            }
        }

        private List<MyModel> _myList = new List<MyModel>();

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }



        public StorageExampleViewModel()
        {
            _myList.Add(new MyModel() { Name = "Michiel", Age = 29 });
            _myList.Add(new MyModel() { Name = "000", Age = 1 });
            _myList.Add(new MyModel() { Name = "Aa", Age = 3 });
            _myList.Add(new MyModel() { Name = "Bb", Age = 4 });
            _myList.Add(new MyModel() { Name = "CC", Age = 6 });
            _myList.Add(new MyModel() { Name = "dd", Age = 88 });
            _myList.Add(new MyModel() { Name = "ee", Age = 50 });
            _myList.Add(new MyModel() { Name = "ff", Age = 15 });

            SaveCommand = new RelayCommand(() => SaveAction());
            LoadCommand = new RelayCommand(() => LoadAction());
            DeleteCommand = new RelayCommand(() => DeleteAction());

        }

        private async void SaveAction()
        {
            await storageHelper.SaveAsync(_myList, "mylist");

            Result = "List saved";
        }

        private async void LoadAction()
        {
            var result = await storageHelper.LoadAsync("mylist");

            if(result == null)
                Result = string.Format("List is null / file not available");
            else
                Result = string.Format("List loaded and contains {0} records", result.Count);

        }

        private async void DeleteAction()
        {
            await storageHelper.DeleteAsync("mylist");

            Result = "List deleted";
        }



        public class MyModel
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

    }
}
