namespace HdProduction.Dashboard.Domain.Entities
{
  public class UserProjectRights
  {
    public UserProjectRights(long userId, long projectId, ProjectRight right)
    {
      UserId = userId;
      ProjectId = projectId;
      Right = right;
    }

    public long UserId { get; }
    public long ProjectId { get; }
    public ProjectRight Right { get; }

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