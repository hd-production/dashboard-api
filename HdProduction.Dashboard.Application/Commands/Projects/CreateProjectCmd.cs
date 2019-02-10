using HdProduction.Dashboard.Domain.Entities.Projects;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class CreateProjectCmd : BaseProjectCmd, IRequest<long>
  {
    public CreateProjectCmd(string name, SelfHostSettings selfHostSettings, long userId)
    : base(name, selfHostSettings, userId)
    {
    }
  }
}