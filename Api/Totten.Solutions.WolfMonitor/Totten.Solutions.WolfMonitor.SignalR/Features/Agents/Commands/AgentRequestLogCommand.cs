using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Totten.Solutions.WolfMonitor.Infra.CrossCutting.Validation;

namespace Totten.Solutions.WolfMonitor.SignalR.Features.Agents.Commands
{
    public class AgentRequestLogCommand
    {
        public string AgentToken { get; set; }

        public string ClientToken { get; set; }

        public ValidationResult Validate()
        {
            return new Validator().Validate(this);
        }

        private class Validator : AbstractValidator<AgentRequestLogCommand>
        {
            public Validator()
            {
                RuleFor(s => s.AgentToken).NotEmpty().Matches(RegexTypes.AgentToken);
                RuleFor(s => s.ClientToken).NotEmpty();
            }
        }
    }
}
