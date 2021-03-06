using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HdProduction.Dashboard.Api.Auth;
using HdProduction.Dashboard.Api.Extensions;
using HdProduction.Dashboard.Api.Models.Projects;
using HdProduction.Dashboard.Application.Commands.Projects;
using HdProduction.Dashboard.Application.Models;
using HdProduction.Dashboard.Application.Queries.Projects;
using HdProduction.Dashboard.Domain.Entities.Builds;
using HdProduction.Dashboard.Domain.Entities.Projects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HdProduction.Dashboard.Api.Controllers
{
  [ApiController, ApiVersion("0")]
  [Route("v{version:apiVersion}/projects"), Authorize]
  public class ProjectsController : ControllerBase
  {
    private readonly IAppBuildQuery _appBuildQuery;
    private readonly IProjectQuery _query;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProjectsController(IProjectQuery query, IMediator mediator, 
      IAppBuildQuery appBuildQuery, IMapper mapper)
    {
      _query = query;
      _mediator = mediator;
      _appBuildQuery = appBuildQuery;
      _mapper = mapper;
    }

    [HttpGet("")]
    public Task<IReadOnlyCollection<ProjectGridReadModel>> Get() => _query.GetAllAsync(User.GetId());

    [HttpGet("{projectId}"), Authorize(Policy = Policy.ProjectAdminAccess)]
    public Task<ProjectReadModel> Get(long projectId) => _query.GetAsync(projectId);

    [HttpPost("")]
    public async Task<ProjectReadModel> Create([FromBody] ProjectCreateRequestModel requestModel)
    {
      var id = await _mediator.Send(new CreateProjectCmd(
        requestModel.Name, User.GetId()));
      return await Get(id);
    }

    [HttpPut("{projectId}"), Authorize(Policy = Policy.ProjectAdminAccess)]
    public async Task<ProjectReadModel> Update(long projectId, [FromBody] ProjectUpdateRequestModel requestModel)
    {
      await _mediator.Send(new UpdateProjectCmd(
        projectId, requestModel.Name, _mapper.Map<SelfHostSettings>(requestModel.SelfHostSettings),
        _mapper.Map<DefaultAdminSettings>(requestModel.DefaultAdminSettings), User.GetId()));
      return await Get(projectId);
    }

    [HttpDelete("{projectId}"), Authorize(Policy = Policy.ProjectCreatorAccess)]
    public Task Delete(long projectId)
    {
      return _mediator.Send(new DeleteProjectCmd(projectId, User.GetId()));
    }

    [HttpPut("{projectId}/run"), Authorize(Policy = Policy.ProjectAdminAccess)]
    public async Task<ProjectReadModel> Run(long projectId)
    {
      await _mediator.Send(new RunProjectCmd(projectId, User.GetId()));
      return await Get(projectId);
    }
    
    [HttpGet("{projectId}/app_build")]
    public async Task<IActionResult> DownloadAppBuild(long projectId)
    {
      return Redirect(await _appBuildQuery.FindDownloadLinkAsync(projectId));
    }

    [HttpPut("{projectId}/build/retry")]
    public Task RetryBuild(long projectId, [FromQuery] BuildType type, [FromQuery] long buildId)
    {
      return _mediator.Send(new RetryBuildProjectCmd(buildId, projectId, type));
    }

    [HttpPost("{projectId}/build")]
    public Task TestBuild(long projectId)
    {
      return _mediator.Send(new BuildProjectCmd(projectId, BuildType.SelfHost, SelfHostBuildConfiguration.MySql));
    }
  }
}