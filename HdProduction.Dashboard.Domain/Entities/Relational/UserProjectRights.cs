using HdProduction.Dashboard.Domain.Entities.Projects;
using HdProduction.Dashboard.Domain.Entities.Users;

namespace HdProduction.Dashboard.Domain.Entities.Relational
{
  public class UserProjectRights
  {
    public UserProjectRights(long userId, long projectId, ProjectRight right)
    {
      UserId = userId;
      ProjectId = projectId;
      Right = right;
    }

    public long UserId { get; private set; }
    public long ProjectId { get; private set; }
    public ProjectRight Right { get; private set; }

    public User User { get; set; } // ef
    public Project Project { get; set; } // ef
  }

  public class UserProjectRightsMetadata : EntityMetadata<UserProjectRightsMetadata>
  {
    public static readonly string Table = "user_project";
    public static readonly string UserId = InQuotes(nameof(UserProjectRights.UserId));
    public static readonly string ProjectId = InQuotes(nameof(UserProjectRights.ProjectId));
    public static readonly string Right = InQuotes(nameof(UserProjectRights.Right));
  }
}