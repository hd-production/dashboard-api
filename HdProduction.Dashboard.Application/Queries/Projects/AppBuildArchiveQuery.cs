using System.IO;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Exceptions;

namespace HdProduction.Dashboard.Application.Queries.Projects
{
  public interface IAppBuildQuery
  {
    Task<(FileStream stream, string name)> DownloadArchiveAsync(long projectId);
  }

  public class AppBuildQuery : IAppBuildQuery
  {
    private readonly IProjectRepository _projectRepository;
    private readonly IHelpdeskBuildService _helpdeskBuildService;

    public AppBuildQuery(IHelpdeskBuildService helpdeskBuildService, IProjectRepository projectRepository)
    {
      _helpdeskBuildService = helpdeskBuildService;
      _projectRepository = projectRepository;
    }

    public async Task<(FileStream stream, string name)> DownloadArchiveAsync(long projectId)
    {
      var project = await _projectRepository.FindAsync(projectId) ?? throw new EntityNotFoundException("Project not found");
      if (project.SelfHostSettings == null)
      {
        throw new BusinessLogicException("Project has no self host settings");
      }
      var archivePath = _helpdeskBuildService.BuildApp(project.SelfHostSettings.BuildConfiguration);
      var stream = File.OpenRead(archivePath);
      return (stream, Path.GetFileName(archivePath));
    }
  }
}