﻿using System;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Base;
using Totten.Solutions.WolfMonitor.Client.Infra.Data.Https.Features.Monitorings;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.Items;
using Totten.Solutions.WolfMonitor.WpfApp.ValueObjects.SystemServices;

namespace Totten.Solutions.WolfMonitor.WpfApp.Applications.Monitorings
{
    public class ItemsMonitoringService
    {
        private ItemsEndPoint _endPoint;

        public ItemsMonitoringService(ItemsEndPoint endPoint)
        {
            _endPoint = endPoint;
        }

        public async Task<Result<Exception, Guid>> Post(Item Item)
        {
            return await _endPoint.Post<Item>(Item);
        }

        public async Task<Result<Exception, PageResult<SystemServiceViewModel>>> GetSystemServices(Guid id)
        {
            return await _endPoint.GetServicesByAgentId<SystemServiceViewModel>(id);
        }

        public async Task<Result<Exception, Unit>> Delete(Guid agentId,Guid id)
        {
            return await _endPoint.Delete(agentId,id);
        }

        public Task<Result<Exception, PageResult<ItemHistoricViewModel>>> GetItemHistoric(Guid id)
        {
            return _endPoint.GetItemHistoric<ItemHistoricViewModel>(id);
        }
    }
}