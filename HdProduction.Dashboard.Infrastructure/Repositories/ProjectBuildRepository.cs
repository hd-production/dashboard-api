using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities.Builds;
using Microsoft.EntityFrameworkCore;

namespace HdProduction.Dashboard.Infrastructure.Repositories
{
  public class ProjectBuildRepository : IProjectBuildsRepository
  {
    private readonly ApplicationContext _context;

    public ProjectBuildRepository(ApplicationContext context)
    {
      _context = context;
    }

    public Task<ProjectBuild> FindAsync(long key, bool withTracking = true)
    {
      return withTracking
        ? _context.ProjectBuilds.FindAsync(key)
        : _context.ProjectBuilds.AsNoTracking().SingleAsync(pb => pb.ProjectId == key);
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Add(ProjectBuild entity)
    {
      _context.Add(entity);
    }

    public void Remove(ProjectBuild entity)
    {
      _context.Remove(entity);
    }
  }
}