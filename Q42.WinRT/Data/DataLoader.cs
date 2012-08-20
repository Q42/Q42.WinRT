using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Q42.WinRT.Data
{
    public enum LoadingState
    {
        None,
        Loading,
        Finished,
        Error
    }

    /// <summary>
    /// DataLoader that enables easy binding to Loading / Finished / Error properties
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataLoader : INotifyPropertyChanged
    {
        private LoadingState _loadingState;
        private bool _catchExceptions = false;

        /// <summary>
        /// Current loading state
        /// </summary>
        public LoadingState LoadingState
        {
            get { return _loadingState; }
            set
            {
                _loadingState = value;

                RaisePropertyChanged();
                RaisePropertyChanged(() => IsBusy);
                RaisePropertyChanged(() => IsError);
                RaisePropertyChanged(() => IsFinished);
            }
        }

        /// <summary>
        /// Indicates LoadingState == LoadingState.Error
        /// </summary>
        public bool IsError
        {
            get
            {
#if DEBUG
                //Always return true for the designer, for easy blend support
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    return true;
#endif

                if (LoadingState == LoadingState.Error)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Indicates LoadingState == LoadingState.Loading
        /// </summary>
        public bool IsBusy
        {
            get
            {
#if DEBUG
                //Always return true for the designer, for easy blend support
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    return true;
#endif

                if (LoadingState == LoadingState.Loading)
                    return true;

                return false;
            }

        }

        /// <summary>
        /// Indicates LoadingState == LoadingState.Finished
        /// </summary>
        public bool IsFinished
        {
            get
            {
#if DEBUG
                //Always return true for the designer, for easy blend support
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    return true;
#endif
                if (LoadingState == LoadingState.Finished)
                    return true;

                return false;
            }

        }
     

        /// <summary>
        /// DataLoader constructors
        /// </summary>
        /// <param name="catchErrors">Swallows exceptions</param>
        public DataLoader(bool? catchExceptions = null)
        {
            if(catchExceptions.HasValue)
                _catchExceptions = catchExceptions.Value;
        }

       /// <summary>
        ///  Load data. Errors will be in errorcallback
       /// </summary>
       /// <param name="loadingMethod">The task is hot and already running</param>
       /// <param name="errorCallback">optional error callback. Fires when exceptino is thrown in loadingMethod</param>
       /// <returns></returns>
        public async Task<T> LoadAsync<T>(Func<Task<T>> loadingMethod, Action<T> resultCallback = null, Action<Exception> errorCallback = null)
        {
            //Set loading state
            LoadingState = Data.LoadingState.Loading;

            T result = default(T);

            try
            {
                result = await loadingMethod();
               
                //Set finished state
                LoadingState = Data.LoadingState.Finished;

                if (resultCallback != null)
                    resultCallback(result);

            }
            catch (Exception e)
            {
                //Set error state
                LoadingState = Data.LoadingState.Error;

                if (errorCallback != null)
                    errorCallback(e);
                else if(!_catchExceptions) //swallow exception if catchexception is true
                    throw; //throw error if no callback is defined

            }

            return result;
        }


        /// <summary>
        /// First returns result callback with result from cache, then from refresh method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheLoadingMethod"></param>
        /// <param name="refreshLoadingMethod"></param>
        /// <param name="resultCallback"></param>
        /// <param name="errorCallback"></param>
        /// <returns></returns>
        public async Task LoadCacheThenRefreshAsync<T>(Func<Task<T>> cacheLoadingMethod, Func<Task<T>> refreshLoadingMethod, Action<T> resultCallback = null, Action<Exception> errorCallback = null)
        {
            //Set loading state
            LoadingState = Data.LoadingState.Loading;

            T cacheResult = default(T);
            T refreshResult = default(T);

            try
            {
                cacheResult = await cacheLoadingMethod();

                if (resultCallback != null)
                    resultCallback(cacheResult);

                refreshResult = await refreshLoadingMethod();

                if (resultCallback != null)
                    resultCallback(refreshResult);

                //Set finished state
                LoadingState = Data.LoadingState.Finished;

            }
            catch (Exception e)
            {
                //Set error state
                LoadingState = Data.LoadingState.Error;

                if (errorCallback != null)
                    errorCallback(e);
                else if (!_catchExceptions) //swallow exception if catchexception is true
                    throw; //throw error if no callback is defined

            }

        }




        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        protected void RaisePropertyChanged(Expression<Func<object>> expression)
        {
            RaisePropertyChanged(Util.GetPropertyName(expression));
        }

    }
}
