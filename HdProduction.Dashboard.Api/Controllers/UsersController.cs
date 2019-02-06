using System.Threading.Tasks;
using HdProduction.Dashboard.Api.Models;
using HdProduction.Dashboard.Application.Commands.Users;
using HdProduction.Dashboard.Application.Models;
using HdProduction.Dashboard.Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HdProduction.Dashboard.Api.Controllers
{
  [ApiController, ApiVersion("0")]
  [Route("v{version:apiVersion}/users")]
  public class UsersController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IUserQuery _query;

    public UsersController(IMediator mediator, IUserQuery query)
    {
      _mediator = mediator;
      _query = query;
    }

    [HttpPost("")]
    public async Task<UserReadModel> Register([FromBody] UserSignUpRequestModel requestModel)
    {
      var id = await _mediator.Send(new CreateUserCmd(requestModel.Email, requestModel.PwdHash));
      return await _query.FindAsync(id);
    }
  }
}