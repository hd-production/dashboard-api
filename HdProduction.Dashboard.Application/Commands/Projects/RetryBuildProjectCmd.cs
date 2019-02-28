using HdProduction.Dashboard.Domain.Entities.Builds;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class RetryBuildProjectCmd : BuildProjectCmd
  {
    public long BuildId { get; }
    public RetryBuildProjectCmd(long buildId, long projectId, BuildType type) : base(projectId, type)
    {
      BuildId = buildId;
    }
  }
}