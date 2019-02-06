using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace HdProduction.Dashboard.Api.Configuration
{
  public static class CorsExtensions
  {
    public static IApplicationBuilder UseCors(this IApplicationBuilder app, IConfiguration configuration)
    {
      return app.UseCors(pb => InitCorsOptions(configuration, pb));
    }

    private static void InitCorsOptions(IConfiguration configuration, CorsPolicyBuilder policyBuilder)
    {
      string origins = configuration.GetValue<string>("Cors:origins");
      string methods = configuration.GetValue<string>("Cors:methods");
      string headers = configuration.GetValue<string>("Cors:headers");
      if (origins == CorsConstants.AnyOrigin)
        policyBuilder.AllowAnyOrigin();
      else
        policyBuilder.WithOrigins(origins.Split(','));
      if (methods == "*")
        policyBuilder.AllowAnyMethod();
      else
        policyBuilder.WithMethods(methods.Split(','));
      if (headers == "*")
        policyBuilder.AllowAnyHeader();
      else
        policyBuilder.WithHeaders(headers.Split(','));
    }
  }
}