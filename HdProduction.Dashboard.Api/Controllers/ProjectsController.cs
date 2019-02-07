using System.Collections.Generic;
using System.Threading.Tasks;
using HdProduction.Dashboard.Api.Auth;
using HdProduction.Dashboard.Api.Extensions;
using HdProduction.Dashboard.Api.Models;
using HdProduction.Dashboard.Application.Commands.Projects;
using HdProduction.Dashboard.Application.Models;
using HdProduction.Dashboard.Application.Queries.Projects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HdProduction.Dashboard.Api.Controllers
{
  [ApiController, ApiVersion("0")]
  [Route("v{version:apiVersion}/projects"), Authorize]
  public class ProjectsController : ControllerBase
  {
    private readonly IProjectQuery _query;
    private readonly IMediator _mediator;

    public ProjectsController(IProjectQuery query, IMediator mediator)
    {
      _query = query;
      _mediator = mediator;
    }

    [HttpGet("")]
    public Task<IReadOnlyCollection<ProjectGridReadModel>> Get()
    {
      return _query.GetAllAsync(User.GetId());
    }

    [HttpGet("{projectId}"), Authorize(Policy = Policy.ProjectAdminAccess)]
    public Task<ProjectReadModel> Get(long projectId)
    {
      return _query.GetAsync(projectId);
    }

    [HttpPost("")]
    public async Task<ProjectReadModel> Create([FromBody] ProjectCreateRequestModel requestModel)
    {
      var id = await _mediator.Send(new CreateProjectCmd(requestModel.Name, User.GetId()));
      return await Get(id);
    }

    [HttpPut("{projectId}"), Authorize(Policy = Policy.ProjectAdminAccess)]
    public async Task<ProjectReadModel> Update(long projectId, [FromBody] ProjectCreateRequestModel requestModel)
    {
      await _mediator.Send(new UpdateProjectCmd(projectId, requestModel.Name, User.GetId()));
      return await Get(projectId);
    }

    [HttpDelete("{projectId}"), Authorize(Policy = Policy.ProjectCreatorAccess)]
    public Task Delete(long projectId)
    {
      return _mediator.Send(new DeleteProjectCmd(projectId, User.GetId()));
    }
  }
}