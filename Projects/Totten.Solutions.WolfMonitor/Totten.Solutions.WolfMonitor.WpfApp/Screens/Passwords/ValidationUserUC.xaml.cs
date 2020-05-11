using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Helpers;
using Totten.Solutions.WolfMonitor.WpfApp.Applications;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Passwords;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Passwords
{
    /// <summary>
    /// Interação lógica para TokenSolicitationControl.xam
    /// </summary>
    public partial class ValidationUserUC : UserControl, IRecoverPassword
    {
        private IUserService _userService;
        private bool _enableNext;

        public EventHandler OnChangeEvent { get; set; }
        public bool EnableNext => _enableNext;
        public bool EnablePrev => false;
        public StepRecover StepRecover => StepRecover.validateUser;

        public string BtnNextName => "Próximo";

        public ValidationUserUC(IUserService userService)
        {
            _userService = userService;
            InitializeComponent();
        }


        public async Task<object> Validation(object param)
        {
            _enableNext = true;

            if (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtEmail.Text))
            {
                _enableNext = false;
                MessageBox.Show("Por favor preencha todos os campos.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            if (!ValidatorHelper.IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email com formato inválido ou o servidor do email informado não esta respondendo", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            var callback = await _userService.RecoverPassword(txtLogin.Text, txtEmail.Text);

            if (callback.IsFailure)
            {
                _enableNext = false;
                MessageBox.Show("Os dados informados não são correspondentes a uma conta.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return new TokenSolicitationVO
            {
                Login = txtLogin.Text,
                Email = txtEmail.Text,
                RecoverSolicitationCode = callback.Success
            };
        }

        private void textChanged(object sender, TextChangedEventArgs e)
        {
            _enableNext = !string.IsNullOrEmpty(txtLogin.Text) && !string.IsNullOrEmpty(txtEmail.Text);
            OnChangeEvent?.Invoke(sender, e);
        }
    }
}