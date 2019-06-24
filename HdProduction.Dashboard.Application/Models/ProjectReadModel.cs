using System;
using System.Collections.Generic;
using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.Dashboard.Domain.Entities.Projects;

namespace HdProduction.Dashboard.Application.Models
{
  public class ProjectReadModel
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public SelfHostSettingsReadModel SelfHostSettings { get; set; }
    public List<ProjectBuildReadModel> Builds { get; set; }  
  }

  public class ProjectBuildReadModel
  {
    public long ProjectId { get; set; }
    public BuildType Type { get; set; }
    public int? SelfHostConfiguration { get; set; }
    public BuildStatus Status { get; set; }
    public string LinkToDownload { get; set; }
    public string Error { get; set; }
    public DateTime LastUpdate { get; set; }
  }

  public class SelfHostSettingsReadModel
  {
    public SelfHostBuildConfiguration BuildConfiguration { get; set; }
  }

  public class DefaultAdminSettingsReadModel
  {
    
  }
}