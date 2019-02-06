using Microsoft.AspNetCore.Authentication;

namespace HdProduction.Dashboard.Application.Auth
{
  public class JwtOptions : AuthenticationSchemeOptions
  {
    public string PublicKeyPath { get; set; }
  }
}