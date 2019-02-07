using System.Collections.Generic;

namespace HdProduction.Dashboard.Domain.Entities
{
  public class Project : EntityBase<long>
  {
    public Project(string name)
    {
      Name = name;
      UserRights = UserRights ?? new List<UserProjectRights>();
    }

    public string Name { get; private set; }
    public ICollection<UserProjectRights> UserRights { get; } // ef

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
  }
}