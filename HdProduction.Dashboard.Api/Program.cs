using System.Threading.Tasks;
using HdProduction.Dashboard.Api.Configuration.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace HdProduction.Dashboard.Api
{
  public class Program
  {
    public static Task Main(string[] args)
    {
      return CreateWebHostBuilder(args).Build().RunAsync();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .ConfigureLogging((hostingContext, logging) => logging.AddLog4Net())
        .CaptureStartupErrors(true);
  }
}