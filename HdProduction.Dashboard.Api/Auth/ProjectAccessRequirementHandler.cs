using System.Linq;
using System.Threading.Tasks;
using HdProduction.Dashboard.Api.Extensions;
using HdProduction.Dashboard.Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace HdProduction.Dashboard.Api.Auth
{
  public class ProjectAccessRequirementHandler : AuthorizationHandler<ProjectAccessRequirement>
  {
    private const string ParameterKey = "projectId";

    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IProjectRepository _projectRepository;

    public ProjectAccessRequirementHandler(IHttpContextAccessor httpContextAccessor, IProjectRepository projectRepository)
    {
      _contextAccessor = httpContextAccessor;
      _projectRepository = projectRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectAccessRequirement requirement)
    {
      string projectId = _contextAccessor.HttpContext.Request.Query.TryGetValue(ParameterKey, out var projIdParam)
        ? projIdParam.First()
        : (string) _contextAccessor.HttpContext.GetRouteValue(ParameterKey);

      var userId = context.User.GetId();
      if (projectId == null || long.TryParse(projectId, out var projId)
          && (await _projectRepository.FindRightAsync(projId, userId))?.Right >= requirement.MinRight)
      {
        context.Succeed(requirement);
      }
    }
  }
}