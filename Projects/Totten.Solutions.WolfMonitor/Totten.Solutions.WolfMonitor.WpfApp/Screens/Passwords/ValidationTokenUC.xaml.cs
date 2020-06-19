using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Totten.Solutions.WolfMonitor.WpfApp.Applications;
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
        private object _validationResponse;

        public ValidationTokenUC(IUserService userService, object validationResponse)
        {
            _userService = userService;
            _validationResponse = validationResponse;
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
                if (!Guid.TryParse(txtToken.Text, out Guid token))
                {
                    MessageBox.Show("O token informado não corresponde a um formato válido.", "Falha", MessageBoxButton.OK, MessageBoxImage.Error);
                    return param;
                }
                var callback = await _userService.TokenConfimation(validationResponse.Company, validationResponse.Login, validationResponse.Email, validationResponse.RecoverSolicitationCode, token);

                if (callback.IsSuccess)
                {
                    return new ValidationFullVO
                    {
                        Username = validationResponse.Login,
                        Company = validationResponse.Company,
                        Email = validationResponse.Email,
                        RecoverSolicitationCode = validationResponse.RecoverSolicitationCode,
                        TokenSolicitationCode = callback.Success
                    };
                }

                MessageBox.Show("Token incorreto.", "Falha", MessageBoxButton.OK, MessageBoxImage.Error);
                return param;
            }

            MessageBox.Show("Erro interno, contate o administrador.", "Falha", MessageBoxButton.OK, MessageBoxImage.Error);
            return param;
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_validationResponse is TokenSolicitationVO vali)
            {
                MessageBox.Show("Foi enviado o token novamente no email fornecido!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                
                _userService.ReSendToken(vali.Company, vali.Login, vali.Email).ContinueWith(task => {});
            }
        }

        private void txtToken_TextChanged(object sender, TextChangedEventArgs e)
        {
            _enableNext = (sender is TextBox textBox && !string.IsNullOrEmpty(textBox.Text));

            OnChangeEvent?.Invoke(sender, e);
        }
    }
}
