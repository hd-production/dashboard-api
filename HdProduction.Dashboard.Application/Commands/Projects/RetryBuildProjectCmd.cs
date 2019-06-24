using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.Dashboard.Domain.Entities.Projects;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class RetryBuildProjectCmd : BuildProjectCmd
  {
    public long BuildId { get; }
    public RetryBuildProjectCmd(long buildId, long projectId, BuildType type) : base(projectId, type, SelfHostBuildConfiguration.MySql)
    {
      BuildId = buildId;
    }
  }
}