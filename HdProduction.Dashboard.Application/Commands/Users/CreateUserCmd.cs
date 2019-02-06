using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Users
{
  public class CreateUserCmd : IRequest<long>
  {
    public CreateUserCmd(string email, string passwordHash)
    {
      Email = email;
      PasswordHash = passwordHash;
    }

    public string Email { get; }
    public string PasswordHash { get; }
  }
}