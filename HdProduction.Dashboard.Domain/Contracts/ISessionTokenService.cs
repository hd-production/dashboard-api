using HdProduction.Dashboard.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace HdProduction.Dashboard.Domain.Contracts
{
  public interface ISessionTokenService
  {
    string CreateToken(User user);
  }
}