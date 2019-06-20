using AutoMapper;
using HdProduction.Dashboard.Application.Models;
using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.Dashboard.Domain.Entities.Projects;

namespace HdProduction.Dashboard.Api.Configuration
{
  public class AutoMapperConfig
  {
    public static IMapper Configure()
    {
      var config = new MapperConfiguration(cfg =>
      {
        cfg.CreateMissingTypeMaps = true;
        cfg.AllowNullCollections = true;
        cfg.AllowNullDestinationValues = true;
        CreateMapping(cfg);
      });

      return config.CreateMapper();
    }

    private static void CreateMapping(IProfileExpression cfg)
    {
      cfg.CreateMap<ProjectBuild, ProjectBuildReadModel>();
      cfg.CreateMap<Project, ProjectReadModel>();
    }
  }
}