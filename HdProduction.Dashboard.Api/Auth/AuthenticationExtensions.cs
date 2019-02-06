using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace HdProduction.Dashboard.Api.Auth
{
  public static class AuthenticationExtensions
  {
    public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services, string publicKeyPath)
    {
      return services.AddAuthentication(opts =>
        {
          opts.DefaultAuthenticateScheme = JwtDefaults.AuthenticationScheme;
          opts.DefaultChallengeScheme = JwtDefaults.AuthenticationScheme;
        })
        .AddScheme<JwtOptions, JwtAuthBearerHandler>(JwtDefaults.AuthenticationScheme, null, opts =>
        {
          opts.ClaimsIssuer = JwtDefaults.Issuer;
          opts.PublicKeyPath = publicKeyPath;
        });
    }
  }
}