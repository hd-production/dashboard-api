using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.Dashboard.Domain.Entities.Projects;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
    public class BuildProjectCmd : IRequest
    {
        public BuildProjectCmd(long projectId, BuildType type, SelfHostBuildConfiguration cfg)
        {
            ProjectId = projectId;
            Type = type;
            Cfg = cfg;
        }

        public long ProjectId { get; }
        public BuildType Type { get; }
        public SelfHostBuildConfiguration Cfg { get; }
    }
}