using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.WpfApp.Applications;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Passwords;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Passwords
{
    /// <summary>
    /// Interação lógica para ChangePasswordUC.xam
    /// </summary>
    public partial class ChangePasswordUC : UserControl, IRecoverPassword
    {
        private IUserService _userService;
        private bool _enableNext;

        public ChangePasswordUC(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        public EventHandler OnChangeEvent { get; set; }

        public StepRecover StepRecover => StepRecover.passwordChange;

        public bool EnablePrev => false;

        public bool EnableNext => _enableNext;

        public string BtnNextName => "Concluir";

        public async Task<object> Validation(object param)
        {
            if (param is ValidationFullVO validation)
            {
                if (!txtPass.Password.Equals(txtRepass.Password))
                {
                    MessageBox.Show("As senhas não são iguais.", "Atênção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return param;
                }

                var callback = await _userService.ChangePassword(validation.Username, validation.Email,
                                                                 validation.TokenSolicitationCode, validation.RecoverSolicitationCode, txtPass.Password);

                if (callback.IsSuccess)
                {
                    MessageBox.Show("A senha foi alterada com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show(callback.Failure.Message, "Atênção", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return param;
                }
            }


            MessageBox.Show("Falha interna, por favor contate um administrador.", "Falha", MessageBoxButton.OK, MessageBoxImage.Error);
            return param;
        }

        private void textChanged(object sender, RoutedEventArgs e)
        {
            _enableNext = !string.IsNullOrEmpty(txtPass.Password) && !string.IsNullOrEmpty(txtRepass.Password);

            OnChangeEvent?.Invoke(sender, e);
        }
    }
}
