using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace HdProduction.Dashboard.Application.Events.MessageQueue
{
  public class MessageQueueEventsPublisher : INotificationHandler<MqEventNotification>
  {
    public Task Handle(MqEventNotification notification, CancellationToken cancellationToken)
    {
      Console.WriteLine(notification.Event.ToString());
      return Unit.Task;
    }
  }
}