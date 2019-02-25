using System.Collections.Generic;
using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.Dashboard.Domain.Entities.Relational;

namespace HdProduction.Dashboard.Domain.Entities.Projects
{
  public class Project : EntityBase<long>
  {
    public Project(string name, SelfHostSettings selfHostSettings)
    {
      Name = name;
      SelfHostSettings = selfHostSettings;
      UserRights = UserRights ?? new List<UserProjectRights>();
      Builds = Builds ?? new List<ProjectBuild>();
    }

    public string Name { get; private set; }
    
    public SelfHostSettings SelfHostSettings { get; }
    
    public ICollection<UserProjectRights> UserRights { get; } // ef
    public ICollection<ProjectBuild> Builds { get; } // ef

    public void Update(string name)
    {
      Name = name;
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