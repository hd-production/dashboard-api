using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class UpdateProjectCmd : IRequest
  {
    public UpdateProjectCmd(long id, string name, long userId)
    {
      Id = id;
      Name = name;
      UserId = userId;
    }

    public long Id { get; }
    public string Name { get; }
    public long UserId { get; }
  }
}