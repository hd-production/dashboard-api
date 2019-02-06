using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Entities;

namespace HdProduction.Dashboard.Domain.Contracts
{
  public interface IUserRepository
  {
    Task<User> FindAsync(long id, bool withTracking = true);
    Task<User> FindByEmailAsync(string email);

    IUnitOfWork UnitOfWork { get; }
    void Add(User user);
  }
}