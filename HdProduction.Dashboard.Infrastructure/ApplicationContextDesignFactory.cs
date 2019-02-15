using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace HdProduction.Dashboard.Infrastructure
{
  public class ApplicationContextDesignFactory : IDesignTimeDbContextFactory<ApplicationContext>
  {
    public ApplicationContext CreateDbContext(string[] args)
    {
      var env = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), ".env"));
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "/HdProduction.Dashboard.Api")
        .AddJsonFile($"appsettings.{env}.json")
        .Build();

      var msb = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("Db"))
      {
        Database = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("Db")).Database
      };
      var options = new DbContextOptionsBuilder<ApplicationContext>()
        .UseNpgsql(msb.ToString(), b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
      Console.WriteLine(msb.ToString());
      return new ApplicationContext(options.Options);
    }
  }
}