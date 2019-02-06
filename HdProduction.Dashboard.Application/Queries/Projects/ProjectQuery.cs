using System.Collections.Generic;
using System.Threading.Tasks;
using HdProduction.Dashboard.Application.Models;

namespace HdProduction.Dashboard.Application.Queries.Projects
{
  public interface IProjectQuery
  {
    Task<IReadOnlyCollection<ProjectGridReadModel>> GetAsync(long userId);
  }

  public class ProjectQuery
  {
  }
}