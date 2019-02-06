using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Users
{
  public class RefreshSessionCmd : IRequest<(string jwtToken, string refreshToken)>
  {
    public RefreshSessionCmd(string refreshToken, long userId)
    {
      RefreshToken = refreshToken;
      UserId = userId;
    }

    public long UserId { get; }
    public string RefreshToken { get; }
  }
}