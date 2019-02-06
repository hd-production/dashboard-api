using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace HdProduction.Dashboard.Api.Extensions
{
  public static class UserIdentityExtensions
  {
    public static int GetId(this ClaimsPrincipal user)
    {
      var id = user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)
        ?? throw new UnauthorizedAccessException();
      return int.Parse(id.Value);
    }
  }
}