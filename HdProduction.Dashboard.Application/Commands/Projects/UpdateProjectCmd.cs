using HdProduction.Dashboard.Domain.Entities.Projects;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class UpdateProjectCmd : BaseProjectCmd, IRequest
  {
    public UpdateProjectCmd(long id, string name, SelfHostSettings selfHostSettings, long userId)
    : base(name, selfHostSettings, userId)
    {
      Id = id;
    }

    public long Id { get; }
  }
}