using HdProduction.MessageQueue.RabbitMq.Events;
using HdProduction.MessageQueue.RabbitMq.Events.AppBuilds;
using MediatR;

namespace HdProduction.Dashboard.Application.Events
{
  public class MqEventNotification : INotification
  {
    public MqEventNotification(HdMessage @event)
    {
      Event = @event;
    }

    public HdMessage Event { get; }
  }
}