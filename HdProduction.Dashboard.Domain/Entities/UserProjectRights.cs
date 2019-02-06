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

    public User User { get; set; }
    public Project Project { get; set; }
  }
}