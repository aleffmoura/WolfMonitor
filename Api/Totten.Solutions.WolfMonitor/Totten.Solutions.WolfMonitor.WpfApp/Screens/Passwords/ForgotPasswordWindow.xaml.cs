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
        private object validationObject;
        private IRecoverPassword _actualControl;
        private ValidationUserUC _validationUser;
        private ValidationTokenUC _validationToken;
        private ChangePasswordUC _changePassword;

        public ForgotPasswordWindow(IUserService userService)
        {
            InitializeComponent();

            _validationUser = new ValidationUserUC(userService);
            _validationToken = new ValidationTokenUC(userService);
            _changePassword = new ChangePasswordUC(userService);

            SwitchPanels(_validationUser);
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
                    return _validationToken;
            else if (_actualControl.StepRecover == StepRecover.tokenConfirm)
            {
                if (isNext)
                    return _changePassword;
                return _validationUser;
            }
            else if (!isNext)
                    return _validationToken;

            return null;
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
           => SwitchPanels(GetControl(false));

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            btnNext.IsEnabled = false;
            btnPrev.IsEnabled = false;

            _actualControl.Validation(validationObject).ContinueWith(task => {
                validationObject = task.Result;
                if (validationObject != null)
                {
                    if (_actualControl.StepRecover != StepRecover.passwordChange)
                        SwitchPanels(GetControl(true));
                    else
                        this.Close();
                }
            });
        }
    }
}
