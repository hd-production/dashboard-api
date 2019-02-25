using System;
using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.Dashboard.Infrastructure.Validation;
using MediatR.Pipeline;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
    public class RetryBuildProjectValidator : Validator<ProjectBuild>, IRequestPreProcessor<RetryBuildProjectCmd>
    {
        private readonly IProjectBuildsRepository _buildsRepository;

        public RetryBuildProjectValidator(IProjectBuildsRepository buildsRepository)
        {
            _buildsRepository = buildsRepository;
        }

        protected override Task SetValidations()
        {
            RuleFor(b => b).NotNull().ThrowsNotFound()
                .WithMessage("Project build not found");

            RuleFor(b => b?.Status).Equal(BuildStatus.Failed)
                .WithMessage("Build retry available only for failed builds");

            RuleFor(b => b?.LastUpdate).LessThan(DateTime.UtcNow.AddMinutes(-5))
                .WithMessage("Retrying may be available only after ");

            RuleFor(b => b?.SelfHostConfiguration).NotNull()
                .When(b => b.Type == BuildType.SelfHost)
                .WithMessage("Self host configuration is not set");
            
            return Task.CompletedTask;
        }

        public async Task Process(RetryBuildProjectCmd request, CancellationToken cancellationToken)
        {
            await CheckAsync(await _buildsRepository.FindAsync(request.BuildId), cancellationToken);
        }
    }
}