using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Entities;

namespace HdProduction.Dashboard.Domain.Contracts
{
  public interface IProjectRepository
  {
    Task<Project> FindAsync(long id, bool withTracking = true);
    void Add(Project project);
  }
}