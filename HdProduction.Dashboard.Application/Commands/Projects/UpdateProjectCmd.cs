using HdProduction.Dashboard.Domain.Entities.Projects;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class UpdateProjectCmd : BaseProjectCmd, IRequest
  {
    public UpdateProjectCmd(long id, string name, SelfHostSettings selfHostSettings, DefaultAdminSettings defaultAdminSettings, long userId)
    : base(name, selfHostSettings, defaultAdminSettings, userId)
    {
      Id = id;
    }

    public long Id { get; }
  }
}