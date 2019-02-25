using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Application.Events;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.MessageQueue.RabbitMq.Events.AppBuilds;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
    public class RetryBuildProjectCmdHandler : IRequestHandler<RetryBuildProjectCmd>
    {
        private readonly IProjectBuildsRepository _buildsRepository;
        private readonly IMediator _mediator;

        public RetryBuildProjectCmdHandler(IProjectBuildsRepository buildsRepository, IMediator mediator)
        {
            _buildsRepository = buildsRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(RetryBuildProjectCmd request, CancellationToken cancellationToken)
        {
            var build = await _buildsRepository.FindAsync(request.BuildId);
            build.SetLastUpdate();
            await _buildsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            if (request.Type == BuildType.SelfHost)
            {
                await _mediator.Publish(new MqEventNotification(new ProjectRequiresSelfHostBuildingEvent
                {
                    ProjectId = request.ProjectId,
                    SelfHostConfiguration = build.SelfHostConfiguration.Value
                }), cancellationToken);
            }

            return Unit.Value;
        }
    }
}