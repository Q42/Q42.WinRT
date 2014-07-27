using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Q42.WinRT.UniversalSampleApp.ViewModel
{
    public class ConvertersExampleViewModel : ViewModelBase
    {
        private object _objectProp;

        public object ObjectProp
        {
            get { return _objectProp; }
            set { _objectProp = value;
            RaisePropertyChanged(() => ObjectProp);
            }
        }

        private string _stringProp;

        public string StringProp
        {
            get { return _stringProp; }
            set
            {
                _stringProp = value;
            RaisePropertyChanged(() => StringProp);
            }
        }

        private int _intProp;

        public int IntProp
        {
            get { return _intProp; }
            set { _intProp = value;
            RaisePropertyChanged(() => IntProp);
            }
        }

        private bool _boolProp;

        public bool BoolProp
        {
            get { return _boolProp; }
            set { _boolProp = value;
            RaisePropertyChanged(() => BoolProp);
            }
        }

        private IEnumerable<string> _enumProp = new List<string>();

        public IEnumerable<string> EnumProp
        {
          get { return _enumProp; }
          set
          {
            _enumProp = value;
            RaisePropertyChanged(() => EnumProp);
          }
        }

        public string CapitalText
        {
            get
            {
                return "FULL CAPS";
            }
        }


        public RelayCommand ToggleCommand { get; set; }


        public ConvertersExampleViewModel()
        {
            ToggleCommand = new RelayCommand(() => ToggleCommandAction());

            ToggleCommandAction();
        }

        private void ToggleCommandAction()
        {
            if (BoolProp)
            {
                BoolProp = false;
                ObjectProp = null;
                StringProp = string.Empty;
                IntProp = 0;
                EnumProp = new List<string>();
            }
            else
            {
                BoolProp = true;
                ObjectProp = new object();
                StringProp = "test";
                IntProp = 5;
                EnumProp = new List<string>() { "test" };

            }
        }

    }
}
