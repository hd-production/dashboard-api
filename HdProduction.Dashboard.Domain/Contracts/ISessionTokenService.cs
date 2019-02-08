using HdProduction.Dashboard.Domain.Entities.Users;

namespace HdProduction.Dashboard.Domain.Contracts
{
  public interface ISessionTokenService
  {
    string CreateToken(User user);
  }
}