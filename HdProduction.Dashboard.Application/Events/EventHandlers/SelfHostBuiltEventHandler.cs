using System.Reflection;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.MessageQueue.RabbitMq;
using HdProduction.MessageQueue.RabbitMq.Events.AppBuilds;
using log4net;

namespace HdProduction.Dashboard.Application.Events.EventHandlers
{
  public class SelfHostBuiltMessageHandler : IMessageHandler<SelfHostBuiltMessage>, IMessageHandler<SelfHostBuildingFailedMessage>
  {
    private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private readonly IProjectBuildsRepository _buildsRepository;

    public SelfHostBuiltMessageHandler(IProjectBuildsRepository buildsRepository)
    {
      _buildsRepository = buildsRepository;
    }

    public async Task HandleAsync(SelfHostBuiltMessage ev)
    {
      var build = await _buildsRepository.FindAsync(ev.BuildId);
      if (build == null)
      {
        Logger.Warn($"Build {ev.BuildId} is not found");
        return;
      }
      build.MarkBuilt(ev.DownloadLink);
      await _buildsRepository.UnitOfWork.SaveChangesAsync();
    }

    public async Task HandleAsync(SelfHostBuildingFailedMessage ev)
    {
      var build = await _buildsRepository.FindAsync(ev.BuildId);
      if (build == null)
      {
        Logger.Warn($"Build {ev.BuildId} is not found");
        return;
      }
      build.MarkFailed(ev.Exception);
      await _buildsRepository.UnitOfWork.SaveChangesAsync();
    }
  }
}