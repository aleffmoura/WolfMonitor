using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Base;

namespace Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation
{
    public class Items : Entity
    {

        [BsonIgnoreIfDefault]
        public BsonObjectId _id { get; set; }

        public Guid AgentId { get; set; }

        public BsonDateTime ReadingDateUTC { get; set; }
        public BsonDocument RawData { get; private set; }

        public Items(Guid agentId, Dictionary<string, object> rawData)
        {
            AgentId = agentId;
            RawData = new BsonDocument(rawData);
        }
        public Items()
        {

        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
