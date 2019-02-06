using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Users
{
  public class SignOutCmd : IRequest
  {
    public SignOutCmd(long userId)
    {
      UserId = userId;
    }

    public long UserId { get; }
  }
}