using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Users
{
  public class CreateUserCmd : IRequest<long>
  {
    public CreateUserCmd(string email, string firstName, string lastName, string passwordHash)
    {
      Email = email;
      PasswordHash = passwordHash;
      FirstName = firstName;
      LastName = lastName;
    }

    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string PasswordHash { get; }
  }
}