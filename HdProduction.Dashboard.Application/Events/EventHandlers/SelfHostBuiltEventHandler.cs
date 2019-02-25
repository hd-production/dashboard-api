using System.Reflection;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.MessageQueue.RabbitMq;
using HdProduction.MessageQueue.RabbitMq.Events.AppBuilds;
using log4net;

namespace HdProduction.Dashboard.Application.Events.EventHandlers
{
  public class SelfHostBuiltEventHandler : IEventHandler<SelfHostBuiltEvent>, IEventHandler<SelfHostBuildingFailedEvent>
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private readonly IProjectBuildsRepository _buildsRepository;

    public SelfHostBuiltEventHandler(IProjectBuildsRepository buildsRepository)
    {
      _buildsRepository = buildsRepository;
    }

    public async Task HandleAsync(SelfHostBuiltEvent ev)
    {
      var build = await _buildsRepository.FindAsync(ev.ProjectId);
      if (build == null)
      {
        Logger.Warn($"Build for project {ev.ProjectId} is not found");
        return;
      }
      build.MarkBuilt(ev.DownloadLink);
      await _buildsRepository.UnitOfWork.SaveChangesAsync();
    }

    public async Task HandleAsync(SelfHostBuildingFailedEvent ev)
    {
      var build = await _buildsRepository.FindAsync(ev.ProjectId);
      if (build == null)
      {
        Logger.Warn($"Build for project {ev.ProjectId} is not found");
        return;
      }
      build.MarkFailed(ev.Exception);
      await _buildsRepository.UnitOfWork.SaveChangesAsync();
    }
  }
}