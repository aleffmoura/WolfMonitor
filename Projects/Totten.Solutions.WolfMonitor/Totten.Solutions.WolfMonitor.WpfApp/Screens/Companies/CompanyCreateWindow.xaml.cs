using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.ObjectValues;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Companies;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Companies;

namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Companies
{
    /// <summary>
    /// Lógica interna para CompanyCreateWindow.xaml
    /// </summary>
    public partial class CompanyCreateWindow : Window
    {
        private TaskScheduler _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private CompanyService _companyService;

        public CompanyCreateWindow(CompanyService companyService)
        {
            InitializeComponent();
            _companyService = companyService;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            btnAdd.IsEnabled = false;

            _companyService.Post(new CompanyCreateVO
            {
                Name = txtName.Text,
                FantasyName = txtFantasyName.Text,
                Email = txtEmail.Text,
                Cnae = txtCnae.Text,
                Cnpj = txtCnpj.Text.Replace(".","").Replace("/","").Replace("-",""),
                Address = txtAddress.Text,
                MunicipalRegistration = txtMunicipalRegistration.Text,
                StateRegistration = txtStateRegistration.Text,
                Phone = txtPhone.Text
            }).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    MessageBox.Show("Adicionado com sucesso.", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
                else
                {
                    if (SetErros(msg: task.Result.Failure.Message))
                        MessageBox.Show("Ocorreram falhas", "Falha", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show(task.Result.Failure.Message, "Falha", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                btnAdd.IsEnabled = true;
            }, _taskScheduler);
        }

        private bool SetErros(string msg)
        {
            ValidationErrorVO errors = new ValidationErrorVO(msg);

            lblNameError.Content = errors["Name"];
            lblFantasyNameError.Content = errors["FantasyName"];
            lblCnpjError.Content = errors["Cnpj"];
            lblStateRegistrationError.Content = errors["StateRegistration"];
            lblMunicipalRegistrationError.Content = errors["MunicipalRegistration"];
            lblCnaeError.Content = errors["Cnae"];
            lblEmailError.Content = errors["Email"];
            lblPhoneError.Content = errors["Phone"];
            lblAddressError.Content = errors["Address"];

            return errors.ContainsErros;
        }
    }
}
