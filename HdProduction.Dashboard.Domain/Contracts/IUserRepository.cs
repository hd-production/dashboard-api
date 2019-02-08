using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Entities.Users;

namespace HdProduction.Dashboard.Domain.Contracts
{
  public interface IUserRepository : IRepository<User, long>
  {
    Task<User> FindByEmailAsync(string email);
  }
}