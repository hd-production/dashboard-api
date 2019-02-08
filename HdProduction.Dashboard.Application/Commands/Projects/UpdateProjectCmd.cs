using HdProduction.Dashboard.Domain.Entities.Projects;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class UpdateProjectCmd : IRequest
  {
    public UpdateProjectCmd(long id, string name, SelfHostSettings selfHostSettings, long userId)
    {
      Id = id;
      Name = name;
      UserId = userId;
      SelfHostSettings = selfHostSettings;
    }

    public long Id { get; }
    public string Name { get; }
    public SelfHostSettings SelfHostSettings { get; }
    public long UserId { get; }
  }
}