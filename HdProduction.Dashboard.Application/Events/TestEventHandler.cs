using System;
using System.Threading.Tasks;
using HdProduction.MessageQueue.RabbitMq;

namespace HdProduction.Dashboard.Application.Events
{
  public class TestEventHandler : IEventHandler<TestEvent>
  {
    public Task HandleAsync(TestEvent ev)
    {
      Console.WriteLine($"Handled {ev.Name}");
      return Task.CompletedTask;
    }
  }
}