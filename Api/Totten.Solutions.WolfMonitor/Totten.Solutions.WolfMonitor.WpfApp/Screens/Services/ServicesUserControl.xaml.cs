﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Totten.Solutions.WolfMonitor.ServiceAgent.Infra.RabbitMQService;
using Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings;
using Totten.Solutions.WolfMonitor.WpfApp.Screens.Items;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices;
using Timer = System.Timers.Timer;
namespace Totten.Solutions.WolfMonitor.WpfApp.Screens.Services
{
    public partial class ServicesUserControl : UserControl, IUserControl
    {
        private Timer _autoRefresh;
        private IRabbitMQ _rabbitMQ;
        private ItemsMonitoringService _itemsMonitoringService;
        private Dictionary<Guid, ServiceUC> _indexes;
        private Guid _agentId;
        private TaskScheduler currentTaskScheduler;
        public ServicesUserControl(Guid agentId,
                                   ItemsMonitoringService itemsMonitoringService)
        {
            currentTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            InitializeComponent();
            _agentId = agentId;
            _itemsMonitoringService = itemsMonitoringService;
            _indexes = new Dictionary<Guid, ServiceUC>();
            Populate();

            LoadCombobox();
        }

        ~ServicesUserControl()
        {
            _indexes.Clear();
            _indexes = null;
            _itemsMonitoringService = null;
        }

        public void PopulateByDictionary()
        {
            this.wrapPanel.Children.Clear();

            foreach (var itemViewModel in _indexes)
            {
                this.wrapPanel.Children.Add(_indexes[itemViewModel.Key]);
            }
            OnApplyTemplate();
        }

        private void LoadCombobox() => cbTimeRefesh.ItemsSource = new List<string>
            {
                new string("segundos"),
                new string("minutos")
            };

        public void Populate()
        {
            _indexes.Clear();
            this.wrapPanel.Children.Clear();
            _itemsMonitoringService.GetSystemServices(_agentId).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    foreach (SystemServiceViewModel serviceViewModel in task.Result.Success.Items)
                    {
                        _indexes.Add(serviceViewModel.Id, new ServiceUC(OnRemove, OnEdit, OnRestart, serviceViewModel));
                    }
                    PopulateByDictionary();
                }
                else
                    MessageBox.Show("Falha na busca dos serviços do agent", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        private void OnRestart(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente modificar o status do serviço?", "Atênção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _rabbitMQ = new Rabbit(_agentId.ToString());
                _rabbitMQ.RouteMessage(_agentId.ToString(), (SystemServiceViewModel)sender);
                MessageBox.Show("Foi enviada uma solicitação para o agent.", "Atênção", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnEdit(object sender, EventArgs e)
        {
            ServiceDetailWindow serviceDetail = new ServiceDetailWindow((SystemServiceViewModel)sender, _itemsMonitoringService);
            serviceDetail.ShowDialog();
        }
        private void OnRemove(object sender, EventArgs e)
        {
            SystemServiceViewModel serviceViewModel = sender as SystemServiceViewModel;

            if (MessageBox.Show($"Deseja realmente remover o serviço: {serviceViewModel.DisplayName} do monitoramento?", "Atênção", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _itemsMonitoringService.Delete(_agentId, serviceViewModel.Id).ContinueWith(task =>
                {
                    if (task.Result.IsSuccess)
                    {
                        MessageBox.Show($"Removido com sucesso", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                        _indexes.Remove(serviceViewModel.Id);
                        PopulateByDictionary();
                    }
                    else
                    {
                        MessageBox.Show($"Falha na tentativa de remoção.", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrmItemsAdd frmItemsAdd = new FrmItemsAdd(new ServicesCreateUC(_agentId));

            var dialogResult = frmItemsAdd.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                await _itemsMonitoringService.Post(frmItemsAdd.Item);
            }
        }

        private void btnRefrash_Click(object sender, RoutedEventArgs e)
           => Populate();

        private void txtValueRefresh_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnApplyTimer_Click(object sender, RoutedEventArgs e)
        {
            if (_autoRefresh == null)
            {
                if (string.IsNullOrEmpty(txtValueRefresh.Text))
                {
                    MessageBox.Show($"Preencha o valor do tempo para o atualização automática", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (cbTimeRefesh.SelectedIndex < 0)
                {
                    MessageBox.Show($"Selecione como será o intervalo de tempo em segundos/minutos para atualização", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (int.TryParse(txtValueRefresh.Text, out int value))
                {
                    var mili = cbTimeRefesh.SelectedIndex == 0 ? value * 1000 : value * 60000;
                    _autoRefresh = new Timer(mili);
                    _autoRefresh.Enabled = true;
                    _autoRefresh.Elapsed += AutoRefresh;
                    kindTimer.Kind = MaterialDesignThemes.Wpf.PackIconKind.Stop;
                    txtValueRefresh.IsEnabled = false;
                    cbTimeRefesh.IsEnabled = false;
                    return;
                }
                MessageBox.Show($"Valor informado não é um numero inteiro válido", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                _autoRefresh.Enabled = false;
                _autoRefresh.Elapsed -= AutoRefresh;
                _autoRefresh.Dispose();
                _autoRefresh = null;
                txtValueRefresh.IsEnabled = true;
                cbTimeRefesh.IsEnabled = true;
                kindTimer.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            }
        }

        private void AutoRefresh(object sender, ElapsedEventArgs e)
        {
            _itemsMonitoringService.GetSystemServices(_agentId).ContinueWith(task =>
            {
                if (task.Result.IsSuccess)
                {
                    foreach (SystemServiceViewModel serviceViewModel in task.Result.Success.Items)
                    {
                        if(_indexes.TryGetValue(serviceViewModel.Id, out ServiceUC value))
                        {
                            value.SetServiceValues(serviceViewModel);
                        }
                    }
                }
                else
                    MessageBox.Show("Falha na busca dos serviços do agent", "Falha", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, currentTaskScheduler);
        }
    }
}
