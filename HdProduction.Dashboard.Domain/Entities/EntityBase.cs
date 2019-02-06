namespace HdProduction.Dashboard.Domain.Entities
{
  public abstract class EntityBase<T> where T : struct
  {
    public T Id { get; private set; }
    public bool IsNew() => Id.Equals(default(T));
  }
}