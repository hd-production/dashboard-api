using HdProduction.Dashboard.Domain.Entities.Projects;

namespace HdProduction.Dashboard.Domain.Contracts
{
  public interface IHelpdeskBuildService
  {
    string BuildApp(SelfHostBuildConfiguration buildConfiguration);
  }
}