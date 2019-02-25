using HdProduction.MessageQueue.RabbitMq.Events;
using MediatR;

namespace HdProduction.Dashboard.Application.Events
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