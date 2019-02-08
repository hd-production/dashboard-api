using HdProduction.Dashboard.Domain.Entities.Projects;

namespace HdProduction.Dashboard.Application.Models
{
  public class ProjectReadModel
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public SelfHostSettingsReadModel SelfHostSettings { get; set; }
  }

  public class SelfHostSettingsReadModel
  {
    public SelfHostBuildConfiguration BuildConfiguration { get; set; }
  }
}