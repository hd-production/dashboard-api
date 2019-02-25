using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.Dashboard.Domain.Exceptions;

namespace HdProduction.Dashboard.Application.Queries.Projects
{
  public interface IAppBuildQuery
  {
    Task<string> FindDownloadLinkAsync(long projectId);
  }

  public class AppBuildQuery : IAppBuildQuery
  {
    private readonly IProjectBuildsRepository _buildsRepository;

    public AppBuildQuery(IProjectBuildsRepository buildsRepository)
    {
      _buildsRepository = buildsRepository;
    }

    public async Task<string> FindDownloadLinkAsync(long projectId)
    {
      var build = await _buildsRepository.FindAsync(projectId) ?? throw new EntityNotFoundException("Project build not found");
      if (build.Status != BuildStatus.Built)
      {
        throw new BusinessLogicException("Project build is not ready for downloading");
      }

      return build.LinkToDownload;
    }
  }
}