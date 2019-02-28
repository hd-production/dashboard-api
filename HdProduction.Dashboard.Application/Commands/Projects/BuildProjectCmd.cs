using HdProduction.Dashboard.Domain.Entities.Builds;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
    public class BuildProjectCmd : IRequest
    {
        public BuildProjectCmd(long projectId, BuildType type)
        {
            ProjectId = projectId;
            Type = type;
        }

        public long ProjectId { get; }
        public BuildType Type { get; }
    }
}