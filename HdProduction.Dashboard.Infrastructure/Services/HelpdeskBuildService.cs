using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities.Projects;
using HdProduction.Dashboard.Domain.Exceptions;
using HdProduction.Dashboard.Infrastructure.Synchronization;

namespace HdProduction.Dashboard.Infrastructure.Services
{
  public class HelpdeskBuildService : IHelpdeskBuildService
  {
    private const string ProjectPrefix = "ConfigTool.App.";
    private readonly string _sourcesPath;

    public HelpdeskBuildService(string sourcesPath)
    {
      _sourcesPath = sourcesPath;
    }

    public string BuildApp(SelfHostBuildConfiguration buildConfiguration)
    {
      var appBuildName = ProjectPrefix + buildConfiguration;
      var pathToProject = Path.Combine(_sourcesPath, $"{appBuildName}/{appBuildName}.csproj");
      var outputPath = Path.Combine(_sourcesPath, $"AppBuilds/{appBuildName}");
      var zippedBuild = outputPath + ".zip";

      using (new AppBuildLocker(buildConfiguration))
      {
        if (File.Exists(zippedBuild))
        {
          return zippedBuild;
        }

        if (!File.Exists(pathToProject))
        {
          throw new BusinessLogicException("Selected build configuration is not supported");
        }

        Process.Start("dotnet", $"publish {pathToProject} -c Release -o {outputPath}")
          ?.WaitForExit();

        ZipFile.CreateFromDirectory(outputPath, zippedBuild);
        return zippedBuild;
      }
    }
  }
}