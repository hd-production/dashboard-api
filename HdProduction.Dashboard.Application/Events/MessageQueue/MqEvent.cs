using HdProduction.Dashboard.Infrastructure.RabbitMq;
using MediatR;

namespace HdProduction.Dashboard.Application.Events.MessageQueue
{
  public class MqEventNotification : INotification
  {
    public MqEventNotification(HdEvent @event)
    {
      Event = @event;
    }

    public HdEvent Event { get; }
  }
}