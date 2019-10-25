using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions;

namespace Totten.Solutions.WolfMonitor.Infra.NoSql.Contexts
{
    public class MonitoringContext
    {
        public IMongoDatabase Database { get; private set; }

        #region "Tables de documentos"
        public IMongoCollection<Items> Items
        {
            get { return Database.GetCollection<Items>("Items"); }
        }
        public IMongoCollection<Agent> Agents
        {
            get { return Database.GetCollection<Agent>("Agents"); }
        }
        #endregion

        public MonitoringContext(IMongoDatabase database)
        {
            Database = database;

            CreateHostHardwareIndexes();
        }

        private void CreateHostHardwareIndexes()
        {
            var builderIndex = Builders<Items>.IndexKeys;

            var agentTokenIndexModel = new CreateIndexModel<Items>(builderIndex.Ascending(x => x.AgentId));

            Items.Indexes.CreateOrUpdateIndex(agentTokenIndexModel);
        }
    }
}
