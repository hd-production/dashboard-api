using HdProduction.Dashboard.Domain.Entities.Projects;

namespace HdProduction.Dashboard.Domain.Entities.Builds
{
  public class ProjectBuild : IEntity<long>
  {
    public ProjectBuild(long projectId, int? selfHostConfiguration)
    {
      ProjectId = projectId;
      Status = BuildStatus.InProgress;
      SelfHostConfiguration = selfHostConfiguration;
    }

    public long ProjectId { get; }
    public BuildStatus Status { get; private set; }
    public int? SelfHostConfiguration { get; }
    public string LinkToDownload { get; private set; }
    public string Error { get; private set; }
    
    public Project Project { get; } // ef

    public void MarkBuilt(string linkToDownload)
    {
      Status = BuildStatus.Built;
      LinkToDownload = linkToDownload;
    }

    public void MarkFailed(string error)
    {
      Status = BuildStatus.Failed;
      Error = error;
    }
  }

  public class ProjectBuildMetadata : EntityMetadata<ProjectBuildMetadata>
  {
    public static readonly string Table = "project_build";
    public static readonly string ProjectId = InQuotes(nameof(ProjectBuild.ProjectId));
    public static readonly string Status = InQuotes(nameof(ProjectBuild.Status));
    public static readonly string SelfHostConfiguration = InQuotes(nameof(ProjectBuild.SelfHostConfiguration));
    public static readonly string LinkToDownload = InQuotes(nameof(ProjectBuild.LinkToDownload));
    public static readonly string Error = InQuotes(nameof(ProjectBuild.Error));
  }
}