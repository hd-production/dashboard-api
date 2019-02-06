using HdProduction.Dashboard.Application.Queries.Projects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HdProduction.Dashboard.Api.Controllers
{
  [ApiController, ApiVersion("0")]
  [Route("v{version:apiVersion}/projects")]
  public class ProjectsController : ControllerBase
  {
    private readonly IProjectQuery _query;
    private readonly IMediator _mediator;

    public ProjectsController(IProjectQuery query, IMediator mediator)
    {
      _query = query;
      _mediator = mediator;
    }
  }
}