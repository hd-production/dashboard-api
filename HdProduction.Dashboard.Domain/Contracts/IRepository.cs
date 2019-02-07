using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Entities;

namespace HdProduction.Dashboard.Domain.Contracts
{
  public interface IRepository<T, in TKey> where T : EntityBase<TKey> where TKey : struct
  {
    Task<T> FindAsync(TKey key, bool withTracking = true);
    IUnitOfWork UnitOfWork { get; }
    void Add(T entity);
    void Remove(T entity);
  }
}