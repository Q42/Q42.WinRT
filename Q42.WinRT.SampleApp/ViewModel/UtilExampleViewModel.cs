using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Q42.WinRT.SampleApp.ViewModel
{
    public class UtilExampleViewModel : ViewModelBase
    {
        public string AppVersion
        {
            get
            {
                return Util.GetAppVersion();
            }
        }

        private string _osVersion;

        public string OsVersion
        {
            get { return _osVersion; }
            set { _osVersion = value;
            RaisePropertyChanged(() => OsVersion);
            }
        }

        public UtilExampleViewModel()
        {
            //Dont load when viewing in Visual Studio or Blend
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                LoadOsVersion();
            }
        }

        private async void LoadOsVersion()
        {
            OsVersion = await Util.GetOsVersionAsync();
        }

    }
}
