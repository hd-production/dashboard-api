using System;
using HdProduction.Dashboard.Domain.Entities.Projects;

namespace HdProduction.Dashboard.Domain.Entities.Builds
{
  public class ProjectBuild : EntityBase<long>
  {
    public ProjectBuild(long projectId, BuildType type, int? selfHostConfiguration)
    {
      ProjectId = projectId;
      Type = type;
      Status = BuildStatus.InProgress;
      SelfHostConfiguration = selfHostConfiguration;
      SetLastUpdate();
    }

    public long ProjectId { get; }
    public BuildType Type { get; }
    public int? SelfHostConfiguration { get; }
    public BuildStatus Status { get; private set; }
    public string LinkToDownload { get; private set; }
    public string Error { get; private set; }
    public DateTime LastUpdate { get; private set; }
    
    public Project Project { get; set; } // ef

    public void MarkBuilt(string linkToDownload)
    {
      Status = BuildStatus.Built;
      LinkToDownload = linkToDownload;
      SetLastUpdate();
    }

    public void MarkFailed(string error)
    {
      Status = BuildStatus.Failed;
      Error = error;
      SetLastUpdate();
    }

    public void SetLastUpdate()
    {
      LastUpdate = DateTime.UtcNow;
    }
  }

  public class ProjectBuildMetadata : EntityMetadata<ProjectBuildMetadata>
  {
    public static readonly string Table = "project_build";
    public static readonly string Id = InQuotes(nameof(ProjectBuild.Id));
    public static readonly string ProjectId = InQuotes(nameof(ProjectBuild.ProjectId));
    public static readonly string Type = InQuotes(nameof(ProjectBuild.Type));
    public static readonly string Status = InQuotes(nameof(ProjectBuild.Status));
    public static readonly string SelfHostConfiguration = InQuotes(nameof(ProjectBuild.SelfHostConfiguration));
    public static readonly string LinkToDownload = InQuotes(nameof(ProjectBuild.LinkToDownload));
    public static readonly string Error = InQuotes(nameof(ProjectBuild.Error));
    public static readonly string LastUpdate = InQuotes(nameof(ProjectBuild.LastUpdate));
  }
}