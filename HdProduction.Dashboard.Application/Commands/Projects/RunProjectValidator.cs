using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Infrastructure.Validation;
using MediatR.Pipeline;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
    public class RunProjectValidator : Validator<RunProjectCmd>, IRequestPreProcessor<RunProjectCmd>
    {
        protected override Task SetValidations()
        {
            return Task.CompletedTask;
        }

        public Task Process(RunProjectCmd request, CancellationToken cancellationToken)
        {
            return CheckAsync(request, cancellationToken);
        }
    }
}