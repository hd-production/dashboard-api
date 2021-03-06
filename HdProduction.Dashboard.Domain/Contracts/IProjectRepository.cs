using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Entities.Projects;
using HdProduction.Dashboard.Domain.Entities.Relational;

namespace HdProduction.Dashboard.Domain.Contracts
{
  public interface IProjectRepository : IRepository<Project, long>
  {
    void Add(UserProjectRights rights);
    Task<UserProjectRights> FindRightAsync(long projectId, long userId);
  }
}