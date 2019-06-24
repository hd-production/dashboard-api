using System.Collections.Generic;
using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.Dashboard.Domain.Entities.Relational;

namespace HdProduction.Dashboard.Domain.Entities.Projects
{
  public class Project : EntityBase<long>
  {
    public Project(string name, SelfHostSettings selfHostSettings, DefaultAdminSettings defaultAdminSettings)
    {
      Name = name;
      SelfHostSettings = selfHostSettings;
      DefaultAdminSettings = defaultAdminSettings;
      UserRights = UserRights ?? new List<UserProjectRights>();
      Builds = Builds ?? new List<ProjectBuild>();
    }

    public string Name { get; private set; }
    public ProjectStatus Status { get; private set; }
    public SelfHostSettings SelfHostSettings { get; private set; }
    public DefaultAdminSettings DefaultAdminSettings { get; private set; }
    
    public ICollection<UserProjectRights> UserRights { get; set; } // ef
    public ICollection<ProjectBuild> Builds { get; set; } // ef

    public void Update(string name, SelfHostSettings selfHostSettings, DefaultAdminSettings defaultAdminSettings)
    {
      Name = name;
      SelfHostSettings = selfHostSettings;
      DefaultAdminSettings = defaultAdminSettings;
    }

    public void Run()
    {
      Status = ProjectStatus.Running;
    }
  }

  public class ProjectMetadata : EntityMetadata<ProjectMetadata>
  {
    public static readonly string Table = "project";
    public static readonly string Id = InQuotes(nameof(Project.Id));
    public static readonly string Name = InQuotes(nameof(Project.Name));
    public static readonly string SelfHostSettings = InQuotes(nameof(Project.SelfHostSettings));
  }
}