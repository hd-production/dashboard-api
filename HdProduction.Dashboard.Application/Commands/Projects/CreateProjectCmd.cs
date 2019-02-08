using HdProduction.Dashboard.Domain.Entities.Projects;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class CreateProjectCmd : IRequest<long>
  {
    public CreateProjectCmd(string name, SelfHostSettings selfHostSettings, int userId)
    {
      Name = name;
      UserId = userId;
      SelfHostSettings = selfHostSettings;
    }

    public string Name { get; }
    public SelfHostSettings SelfHostSettings { get; }
    public int UserId { get; }
  }
}