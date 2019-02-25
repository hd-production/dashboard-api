using HdProduction.Dashboard.Domain.Entities.Builds;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
    public class RetryBuildProjectCmd : IRequest
    {
        public RetryBuildProjectCmd(long buildId, long projectId, BuildType type)
        {
            BuildId = buildId;
            ProjectId = projectId;
            Type = type;
        }

        public long BuildId { get; }
        public long ProjectId { get; }
        public BuildType Type { get; }
    }
}