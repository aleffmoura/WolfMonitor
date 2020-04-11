﻿using System;
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
            if (param is ValidationFullVO validationFull)
            {
                if (!txtPass.Password.Equals(txtRepass.Password))
                {
                    MessageBox.Show("As senhas não são iguais.", "Atênção", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                var callback = await _userService.ChangePassword(validationFull.Username, validationFull.Email, validationFull.TokenSolicitationCode, txtPass.Password, txtRepass.Password);

                if (callback.IsSuccess)
                    return validationFull;
                else
                {
                    MessageBox.Show(callback.Failure.Message, "Atênção", MessageBoxButton.OK, MessageBoxImage.Information);
                    return null;
                }
            }


            MessageBox.Show("Falha interna, por favor contate um administrador.", "Falha", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }

        private void textChanged(object sender, RoutedEventArgs e)
        {
            _enableNext = !string.IsNullOrEmpty(txtPass.Password) && !string.IsNullOrEmpty(txtRepass.Password);

            OnChangeEvent?.Invoke(sender, e);
        }
    }
}
