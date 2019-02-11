using System.Threading.Tasks;
using HdProduction.Dashboard.Api.Extensions;
using HdProduction.Dashboard.Api.Models.Users;
using HdProduction.Dashboard.Application.Commands.Users;
using HdProduction.Dashboard.Application.Models;
using HdProduction.Dashboard.Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HdProduction.Dashboard.Api.Controllers
{
  [ApiController, ApiVersion("0")]
  [Route("v{version:apiVersion}/users"), Authorize]
  public class UsersController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IUserQuery _query;

    public UsersController(IMediator mediator, IUserQuery query)
    {
      _mediator = mediator;
      _query = query;
    }

    [HttpGet("{id}")]
    public Task<UserReadModel> Get(long id) => _query.FindAsync(id);

    [HttpGet("")]
    public Task<UserReadModel> Get() => _query.FindAsync(User.GetId());

    [HttpPost(""), AllowAnonymous]
    public async Task<UserReadModel> Register([FromBody] UserSignUpRequestModel requestModel)
    {
      var id = await _mediator.Send(new CreateUserCmd(requestModel.Email, requestModel.FirstName, requestModel.LastName,requestModel.PwdHash));
      return await _query.FindAsync(id);
    }
  }
}