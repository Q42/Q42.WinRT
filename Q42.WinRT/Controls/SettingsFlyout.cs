using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Q42.WinRT.Controls
{
    /// <summary>
    /// Flyout that can host a UserControl
    /// </summary>
    public class SettingsFlyout
    {
        private const int _width = 346;
        private Popup _popup;

        public void ShowFlyout(UserControl control)
        {
            _popup = new Popup();
            _popup.Closed += OnPopupClosed;
            Window.Current.Activated += OnWindowActivated;
            _popup.IsLightDismissEnabled = true;
            _popup.Width = _width;
            _popup.Height = Window.Current.Bounds.Height;

            control.Width = _width;
            control.Height = Window.Current.Bounds.Height;

            _popup.Child = control;
            _popup.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width - _width);
            _popup.SetValue(Canvas.TopProperty, 0);
            _popup.IsOpen = true;
        }

        private void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                _popup.IsOpen = false;
            }
        }

        void OnPopupClosed(object sender, object e)
        {
            Window.Current.Activated -= OnWindowActivated;
        }
    }
}
