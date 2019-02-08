using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities.Projects;
using HdProduction.Dashboard.Domain.Entities.Relational;
using Microsoft.EntityFrameworkCore;

namespace HdProduction.Dashboard.Infrastructure.Repositories
{
  public class ProjectRepository : IProjectRepository
  {
    private readonly ApplicationContext _context;

    public ProjectRepository(ApplicationContext context)
    {
      _context = context;
    }

    public Task<Project> FindAsync(long id, bool withTracking = true)
    {
      return withTracking
        ? _context.Projects.FindAsync(id)
        : _context.Projects.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Add(Project project)
    {
      _context.Projects.Add(project);
    }

    public void Remove(Project entity)
    {
      _context.Projects.Remove(entity);
    }

    public void Add(UserProjectRights userRights)
    {
      _context.UserProjectRights.Add(userRights);
    }

    public Task<UserProjectRights> FindRightAsync(long projectId, long userId)
    {
      return _context.UserProjectRights.FirstOrDefaultAsync(r => r.UserId == userId && r.ProjectId == projectId);
    }
  }
}