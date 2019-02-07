using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Exceptions;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class DeleteProjectCmdHandler : IRequestHandler<DeleteProjectCmd>
  {
    private readonly IProjectRepository _repository;

    public DeleteProjectCmdHandler(IProjectRepository repository)
    {
      _repository = repository;
    }

    public async Task<Unit> Handle(DeleteProjectCmd request, CancellationToken cancellationToken)
    {
      var project = await _repository.FindAsync(request.Id);
      if (project == null)
      {
        throw new EntityNotFoundException("Project not found");
      }
      _repository.Remove(project);
      await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }
}