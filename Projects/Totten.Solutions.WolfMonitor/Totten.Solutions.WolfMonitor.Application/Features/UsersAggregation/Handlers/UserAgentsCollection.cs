using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.ViewModels;
using Totten.Solutions.WolfMonitor.Domain.Exceptions;
using Totten.Solutions.WolfMonitor.Domain.Features.Agents;
using Totten.Solutions.WolfMonitor.Domain.Features.ItemAggregation;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Structs;

namespace Totten.Solutions.WolfMonitor.Application.Features.UsersAggregation.Handlers
{
    public class UserAgentsCollection
    {
        public class Query : IRequest<Result<Exception, IQueryable<AgentForUserViewModel>>>
        {
            public Guid UserId { get; set; }

            public Query(Guid userId)
            {
                UserId = userId;
            }
        }

        public class QueryHandler : RequestHandler<Query, Result<Exception, IQueryable<AgentForUserViewModel>>>
        {
            private readonly IAgentRepository _repository;
            private readonly IItemRepository _itemRepository;

            public QueryHandler(IAgentRepository repository, IItemRepository itemRepository)
            {
                _repository = repository;
                _itemRepository = itemRepository;
            }

            protected override Result<Exception, IQueryable<AgentForUserViewModel>> Handle(Query request)
            {
                var returned = new List<AgentForUserViewModel>();

                var agentsCallback = _repository.GetAllByUserId(request.UserId);

                if (agentsCallback.IsFailure)
                    return new BusinessException(Domain.Enums.ErrorCodes.NotFound, "Não foi possivel localizar os agents deste usuário, contate o administrador do sistema");


                foreach (Agent agent in agentsCallback.Success)
                {
                    var itemsCallback = _itemRepository.GetAll(agent.Id);

                    int qtdServices = 0;
                    int qtdConfigurations = 0;

                    if (itemsCallback.IsSuccess)
                    {
                        qtdServices = itemsCallback.Success.Where(item => item.Type == Domain.Enums.ETypeItem.SystemService).Count();
                        qtdConfigurations = itemsCallback.Success.Where(item => item.Type == Domain.Enums.ETypeItem.SystemArchive).Count();
                    }

                    var viewModel = Mapper.Map<AgentForUserViewModel>(agent);
                    viewModel.QtdServices = $"{qtdServices}";
                    viewModel.QtdConfiguration = $"{qtdConfigurations}";

                    returned.Add(viewModel);
                }


                return Result<Exception, IQueryable<AgentForUserViewModel>>.Of(returned.AsQueryable());
            }
        }
    }
}
