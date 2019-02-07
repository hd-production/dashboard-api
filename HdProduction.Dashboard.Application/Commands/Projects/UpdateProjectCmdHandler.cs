using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Exceptions;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class UpdateProjectCmdHandler : IRequestHandler<UpdateProjectCmd>
  {
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectCmdHandler(IProjectRepository projectRepository)
    {
      _projectRepository = projectRepository;
    }

    public async Task<Unit> Handle(UpdateProjectCmd request, CancellationToken cancellationToken)
    {
      var project = await _projectRepository.FindAsync(request.Id);
      if (project == null)
      {
        throw new EntityNotFoundException("Project not found");
      }
      project.Update(request.Name);
      await _projectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }
}