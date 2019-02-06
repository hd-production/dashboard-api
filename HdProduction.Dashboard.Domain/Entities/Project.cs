using System.Collections.Generic;

namespace HdProduction.Dashboard.Domain.Entities
{
  public class Project : EntityBase<long>
  {
    public Project(string name)
    {
      Name = name;
      //UserRights = UserRights ?? new List<UserProjectRights>();
    }

    public string Name { get; }
    public ICollection<UserProjectRights> UserRights { get; }
  }
}