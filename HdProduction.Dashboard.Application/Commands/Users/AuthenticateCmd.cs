using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Users
{
  public class AuthenticateCmd : IRequest<(string jwtToken, string refreshToken)>
  {
    public AuthenticateCmd(string email, string pwdHash)
    {
      Email = email;
      PwdHash = pwdHash;
    }

    public string Email { get; }
    public string PwdHash { get; }
  }
}