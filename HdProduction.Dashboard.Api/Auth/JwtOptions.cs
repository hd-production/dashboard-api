using Microsoft.AspNetCore.Authentication;

namespace HdProduction.Dashboard.Api.Auth
{
  public class JwtOptions : AuthenticationSchemeOptions
  {
    public string PublicKeyPath { get; set; }
  }
}