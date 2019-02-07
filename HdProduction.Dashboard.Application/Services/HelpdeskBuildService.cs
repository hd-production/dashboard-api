using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities;
using HdProduction.Dashboard.Domain.Exceptions;

namespace HdProduction.Dashboard.Application.Services
{
  public class HelpdeskBuildService : IHelpdeskBuildService
  {
    private static readonly Dictionary<SelfHostBuildConfiguration, string> AppBuildNames = new Dictionary<SelfHostBuildConfiguration, string>
    {
      [SelfHostBuildConfiguration.MySql] = "ConfigTool.App.MySql",
      [SelfHostBuildConfiguration.Sqlite] = "ConfigTool.App.Sqlite",
      [SelfHostBuildConfiguration.SqlServer] = "ConfigTool.App.SqlServer",
      [SelfHostBuildConfiguration.PostgresSql] = "ConfigTool.App.PostgreSql"
    };

    private readonly string _sourcesPath;

    public HelpdeskBuildService(string sourcesPath)
    {
      _sourcesPath = sourcesPath;
    }

    public async Task LoadBuildArchiveAsync(SelfHostBuildConfiguration buildConfiguration)
    {
      if (!AppBuildNames.ContainsKey(buildConfiguration))
      {
        throw new BusinessLogicException("Selected build configuration is not supported");
      }

      var appBuildName = AppBuildNames[buildConfiguration];
      var pathToProject = Path.Combine(_sourcesPath, $"{appBuildName}/{appBuildName}.csproj");
      var outputPath = Path.Combine(_sourcesPath, $"AppBuilds/{appBuildName}");
      Process.Start("dotnet", $"publish {pathToProject} -c Release -o {outputPath}");
    }
  }
}