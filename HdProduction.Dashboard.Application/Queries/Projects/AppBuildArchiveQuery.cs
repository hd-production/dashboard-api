using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities;

namespace HdProduction.Dashboard.Application.Queries.Projects
{
  public interface IAppBuildQuery
  {
    Task DownloadArchiveAsync();
  }

  public class AppBuildQuery : IAppBuildQuery
  {
    private readonly IHelpdeskBuildService _helpdeskBuildService;

    public AppBuildQuery(IHelpdeskBuildService helpdeskBuildService)
    {
      _helpdeskBuildService = helpdeskBuildService;
    }

    public Task DownloadArchiveAsync()
    {
      return _helpdeskBuildService.LoadBuildArchiveAsync(SelfHostBuildConfiguration.MySql);
    }
  }
}