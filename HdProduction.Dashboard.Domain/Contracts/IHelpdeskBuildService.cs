using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Entities;

namespace HdProduction.Dashboard.Domain.Contracts
{
  public interface IHelpdeskBuildService
  {
    Task LoadBuildArchiveAsync(SelfHostBuildConfiguration buildConfiguration);
  }
}