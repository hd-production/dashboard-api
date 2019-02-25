using HdProduction.Dashboard.Domain.Entities.Builds;

namespace HdProduction.Dashboard.Domain.Contracts
{
  public interface IProjectBuildsRepository : IRepository<ProjectBuild, long>
  {
  }
}