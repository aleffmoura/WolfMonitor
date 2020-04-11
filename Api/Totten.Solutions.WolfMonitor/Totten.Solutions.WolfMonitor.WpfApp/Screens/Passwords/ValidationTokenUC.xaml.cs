using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Totten.Solutions.WolfMonitor.WpfApp.Applications;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Users;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Passwords;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Passwords
{
    /// <summary>
    /// Interação lógica para ValidationTokenUC.xam
    /// </summary>
    public partial class ValidationTokenUC : UserControl, IRecoverPassword
    {
        private bool _enableNext;
        private IUserService _userService;

        public ValidationTokenUC(IUserService userService)
        {
            _userService = userService;
            InitializeComponent();
        }

        public StepRecover StepRecover => StepRecover.tokenConfirm;

        public EventHandler OnChangeEvent { get; set; }

        public bool EnablePrev => true;

        public bool EnableNext => _enableNext;

        public string BtnNextName => "Próximo";

        public async Task<object> Validation(object param)
        {
            if (param is TokenSolicitationVO validationResponse)
            {
                var callback = await _userService.TokenConfimation(validationResponse.Username, validationResponse.Email, validationResponse.RecoverSolicitationCode, txtToken.Text);

                if (callback.IsSuccess)
                {
                    return new ValidationFullVO
                    {
                        Username = validationResponse.Username,
                        Email = validationResponse.Email,
                        RecoverSolicitationCode = validationResponse.RecoverSolicitationCode,
                        TokenSolicitationCode = callback.Success
                    };
                }

                MessageBox.Show("Token incorreto.", "Falha", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            MessageBox.Show("Erro interno, contate o administrador.", "Falha", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Foi enviado o token novamente no email fornecido!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void txtToken_TextChanged(object sender, TextChangedEventArgs e)
        {
            _enableNext = (sender is TextBox textBox && !string.IsNullOrEmpty(textBox.Text));

            OnChangeEvent?.Invoke(sender, e);
        }
    }
}
