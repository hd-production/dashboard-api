using System.Threading.Tasks;
using HdProduction.App.Common.Auth;
using HdProduction.Dashboard.Api.Extensions;
using HdProduction.Dashboard.Api.Models.Sessions;
using HdProduction.Dashboard.Application.Commands.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HdProduction.Dashboard.Api.Controllers
{
  [ApiController, ApiVersion("0")]
  [Route("v{version:apiVersion}/sessions")]
  public class SessionsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public SessionsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost("")]
    public async Task<SessionResponseModel> Authenticate([FromBody] AuthenticateRequestModel requestModel)
    {
      var (token, refreshToken) = await _mediator.Send(new AuthenticateCmd(requestModel.Email, requestModel.PwdHash));
      return new SessionResponseModel
      {
        Token = token,
        RefreshToken = refreshToken
      };
    }

    [HttpPut("refresh"), Authorize(AuthenticationSchemes = JwtDefaults.AuthenticationSchemeIgnoreExpiration)]
    public async Task<SessionResponseModel> Refresh([FromBody] RefreshSessionRequestModel requestModel)
    {
      var (token, refreshToken) = await _mediator.Send(new RefreshSessionCmd(requestModel.RefreshToken, User.GetId()));
      return new SessionResponseModel
      {
        Token = token,
        RefreshToken = refreshToken
      };
    }

    [HttpDelete(""), Authorize]
    public Task SignOut() => _mediator.Send(new SignOutCmd(User.GetId()));
  }
}