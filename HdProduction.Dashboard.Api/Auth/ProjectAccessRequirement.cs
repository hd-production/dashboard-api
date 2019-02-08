using HdProduction.Dashboard.Domain.Entities.Projects;
using Microsoft.AspNetCore.Authorization;

namespace HdProduction.Dashboard.Api.Auth
{
  public class ProjectAccessRequirement : IAuthorizationRequirement
  {
    public ProjectRight MinRight { get; }

    public ProjectAccessRequirement(ProjectRight minAccessRight)
    {
      MinRight = minAccessRight;
    }
  }
}