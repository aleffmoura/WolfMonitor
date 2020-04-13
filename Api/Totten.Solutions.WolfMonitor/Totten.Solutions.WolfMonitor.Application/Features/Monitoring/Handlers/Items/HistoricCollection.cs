using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items
{

    public class HistoricCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<ItemHistoric>>>
        {
            public Guid ItemId { get; set; }

            public Query(Guid itemId)
            {
                ItemId = itemId;
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<ItemHistoric>>>
        {
            private readonly IItemRepository _repository;
            public QueryHandler(IItemRepository repository)
            {
                _repository = repository;
            }

            protected override Result<Exception, IQueryable<ItemHistoric>> Handle(Query request)
            {

                var item = _repository.GetByIdAsync(request.ItemId).ConfigureAwait(false).GetAwaiter().GetResult();

                if (item.IsFailure)
                    return new NotFoundException("Não foi encontrado um item valido com id informado.");

                Result<Exception, IQueryable<ItemHistoric>> historic = _repository.GetAllHistoric(request.ItemId);

                return historic;
            }
        }
    }
}
