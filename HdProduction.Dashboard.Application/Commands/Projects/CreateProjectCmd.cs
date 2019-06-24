using HdProduction.Dashboard.Domain.Entities.Projects;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class CreateProjectCmd : BaseProjectCmd, IRequest<long>
  {
    public CreateProjectCmd(string name, long userId)
    : base(name, null, null, userId)
    {
    }
  }
}