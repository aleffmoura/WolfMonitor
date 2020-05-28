using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Domain.Features.UsersAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.Monitoring.Handlers.Items
{
    public class SolicitationHistoricCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<ItemSolicitationHistoric>>>
        {
            public Guid ItemId { get; set; }

            public Query(Guid itemId)
            {
                ItemId = itemId;
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<ItemSolicitationHistoric>>>
        {
            private readonly IItemRepository _repository;
            private readonly IUserRepository _userRepository;

            public QueryHandler(IItemRepository repository, IUserRepository userRepository)
            {
                _repository = repository;
                _userRepository = userRepository;
            }

            protected override Result<Exception, IQueryable<ItemSolicitationHistoric>> Handle(Query request)
            {
                var item = _repository.GetByIdAsync(request.ItemId).ConfigureAwait(false).GetAwaiter().GetResult();

                if (item.IsFailure)
                    return new NotFoundException("Não foi encontrado um item valido com id informado.");

                Result<Exception, IQueryable<ItemSolicitationHistoric>> historic = _repository.GetAllSolicitation(request.ItemId);

                if (historic.IsFailure)
                    return historic.Failure;

                var solicitations = historic.Success.ToList();

                foreach (var solicitation in solicitations)
                {
                    var user = _userRepository.GetByIdAsync(solicitation.UserId).ConfigureAwait(false).GetAwaiter().GetResult();

                    if (user.IsSuccess)
                        solicitation.User = user.Success;
                }

                return Result<Exception, IQueryable<ItemSolicitationHistoric>>.Of(solicitations.OrderByDescending(x => x.CreatedIn).AsQueryable());
            }
        }
    }
}
