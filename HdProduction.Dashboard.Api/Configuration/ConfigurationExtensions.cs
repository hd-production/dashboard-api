using Microsoft.Extensions.DependencyInjection;

namespace HdProduction.Dashboard.Api.Configuration
{
  public static class ConfigurationExtensions
  {
    public static IMvcCoreBuilder AddWebApi(this IServiceCollection services)
    {
      return services.AddMvcCore().AddAuthorization().AddFormatterMappings().AddJsonFormatters().AddCors().AddApiExplorer();
    }
  }
}