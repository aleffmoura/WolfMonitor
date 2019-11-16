using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Totten.Solutions.WolfMonitor.Infra.CrossCutting.Extensions
{
    public static class MongoIndexExtensions
    {
        public static void CreateOrUpdateIndex<T>(this IMongoIndexManager<T> mongoIndexManager, CreateIndexModel<T> indexModel)
        {
            try
            {
                mongoIndexManager.CreateOne(indexModel.Keys, indexModel.Options);
            }
            catch (MongoCommandException mongoCommandExpection)
            {
                // Código de erro referente ao conflito de indexes
                var indexOptionsConflictCode = 85;

                if (mongoCommandExpection.Code.Equals(indexOptionsConflictCode))
                {
                    var indexName = mongoCommandExpection.Message.Split(' ')[6];

                    mongoIndexManager.DropOne(indexName.ToString());

                    mongoIndexManager.CreateOrUpdateIndex(indexModel);
                }
            }
        }

        public static void CreateOrUpdateManyIndexes<T>(this IMongoIndexManager<T> mongoIndexManager, List<CreateIndexModel<T>> indexModels)
        {
            indexModels.ForEach(indexModel => mongoIndexManager.CreateOrUpdateIndex(indexModel));
        }
    }
}
