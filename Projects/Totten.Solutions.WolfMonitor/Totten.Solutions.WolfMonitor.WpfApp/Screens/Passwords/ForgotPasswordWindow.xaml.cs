using System;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.WpfApp.Applications;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Passwords
{
    public enum StepRecover
    {
        validateUser = 0,
        tokenConfirm,
        passwordChange
    }
    /// <summary>
    /// Lógica interna para ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        private object _validationObject;
        private IRecoverPassword _actualControl;
        private IUserService _userService;

        public ForgotPasswordWindow(IUserService userService)
        {
            InitializeComponent();
            
            _userService = userService;

            SwitchPanels(new ValidationUserUC(_userService, null));
        }

        private void SwitchPanels(UserControl userControl)
        {
            if (_actualControl != null)
                _actualControl.OnChangeEvent -= ChangeStateButtons;

            _actualControl = userControl as IRecoverPassword;
            this.pnlRoot.Children.Clear();
            this.pnlRoot.Children.Add(userControl);
            _actualControl.OnChangeEvent += ChangeStateButtons;
            btnNext.Content = _actualControl.BtnNextName;

            ChangeStateButtons(userControl, new EventArgs());
        }

        private void ChangeStateButtons(object sender, EventArgs e)
        {
            btnPrev.IsEnabled = _actualControl.EnablePrev;
            btnNext.IsEnabled = _actualControl.EnableNext;
        }

        private UserControl GetControl(bool isNext)
        {
            if (_actualControl.StepRecover == StepRecover.validateUser && isNext)
                return new ValidationTokenUC(_userService, _validationObject);
            else if (_actualControl.StepRecover == StepRecover.tokenConfirm)
            {
                if (isNext)
                    return new ChangePasswordUC(_userService, _validationObject);
                return new ValidationUserUC(_userService, _validationObject);
            }
            else if (!isNext)
                return new ValidationTokenUC(_userService, _validationObject);

            return null;
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
           => SwitchPanels(GetControl(false));

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            btnNext.IsEnabled = false;
            btnPrev.IsEnabled = false;

            var result = await _actualControl.Validation(_validationObject);

            if (result != null)
            {
                if (_actualControl.StepRecover != StepRecover.passwordChange && result != _validationObject)
                {
                    _validationObject = result;
                    SwitchPanels(GetControl(true));
                }
                else if(_actualControl.StepRecover == StepRecover.passwordChange && bool.TryParse(result.ToString(), out bool resultBool) && resultBool == true)
                    this.Close();
            }

        }
    }
}
