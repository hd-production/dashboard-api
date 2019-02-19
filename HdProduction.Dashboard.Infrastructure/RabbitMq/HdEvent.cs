namespace HdProduction.Dashboard.Infrastructure.RabbitMq
{
  public abstract class HdEvent
  {
    public override string ToString()
    {
      return GetType().Name;
    }
  }
}