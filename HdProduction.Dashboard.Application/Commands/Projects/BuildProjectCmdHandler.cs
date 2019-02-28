using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Application.Events;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.Dashboard.Domain.Exceptions;
using HdProduction.MessageQueue.RabbitMq.Events.AppBuilds;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
    public class BuildProjectCmdHandler : IRequestHandler<BuildProjectCmd>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectBuildsRepository _buildsRepository;
        private readonly IMediator _mediator;

        public BuildProjectCmdHandler(IProjectBuildsRepository buildsRepository, IMediator mediator, IProjectRepository projectRepository)
        {
            _buildsRepository = buildsRepository;
            _mediator = mediator;
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(BuildProjectCmd request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.FindAsync(request.ProjectId) ?? throw new EntityNotFoundException("Project not found");
            if (project.SelfHostSettings?.BuildConfiguration == null)
            {
                throw new BusinessLogicException("No self host settings");
            }
            var build = new ProjectBuild(request.ProjectId, BuildType.SelfHost, (int?) project.SelfHostSettings.BuildConfiguration);

            _buildsRepository.Add(build);
            await _buildsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            if (request.Type == BuildType.SelfHost)
            {
                await _mediator.Publish(new MqEventNotification(new RequiresSelfHostBuildingMessage
                {
                    BuildId = build.Id,
                    SelfHostConfiguration = build.SelfHostConfiguration.Value
                }), cancellationToken);
            }

            return Unit.Value;
        }
    }
}