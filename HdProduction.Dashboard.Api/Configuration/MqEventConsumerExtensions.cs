using HdProduction.MessageQueue.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HdProduction.Dashboard.Api.Configuration
{
  public static class MqEventConsumerExtensions
  {
    public static IRabbitMqConsumer SetEventConsumer(this IApplicationBuilder app)
    {
      using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
      {
        return serviceScope.ServiceProvider.GetService<IRabbitMqConsumer>();
      }
    }
  }
}