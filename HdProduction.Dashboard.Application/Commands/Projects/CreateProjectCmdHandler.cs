using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities.Projects;
using HdProduction.Dashboard.Domain.Entities.Relational;
using HdProduction.Dashboard.Infrastructure;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Projects
{
  public class CreateProjectCmdHandler : IRequestHandler<CreateProjectCmd, long>
  {
    private readonly ApplicationContext _dbContext;
    private readonly IProjectRepository _projectRepository;

    public CreateProjectCmdHandler(IProjectRepository projectRepository, ApplicationContext dbContext)
    {
      _projectRepository = projectRepository;
      _dbContext = dbContext;
    }

    public async Task<long> Handle(CreateProjectCmd request, CancellationToken cancellationToken)
    {
      using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
      {
        var project = new Project(request.Name, request.SelfHostSettings);
        _projectRepository.Add(project);
        await _projectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        _projectRepository.Add(new UserProjectRights(request.UserId, project.Id, ProjectRight.Creator));
        await _projectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        transaction.Commit();
        return project.Id;
      }
    }
  }
}