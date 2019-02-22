using System.Threading;
using System.Threading.Tasks;
using HdProduction.MessageQueue.RabbitMq;
using MediatR;

namespace HdProduction.Dashboard.Application.Events
{
  public class MessageQueueEventsPublisher : INotificationHandler<MqEventNotification>
  {
    private readonly IRabbitMqPublisher _rabbitMqPublisher;

    public MessageQueueEventsPublisher(IRabbitMqPublisher rabbitMqPublisher)
    {
      _rabbitMqPublisher = rabbitMqPublisher;
    }

    public async Task Handle(MqEventNotification notification, CancellationToken cancellationToken)
    {
      await _rabbitMqPublisher.PublishAsync(notification.Event);
    }
  }
}