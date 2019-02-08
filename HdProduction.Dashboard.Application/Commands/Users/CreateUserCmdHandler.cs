using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities.Users;
using HdProduction.Dashboard.Domain.Services;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Users
{
  public class CreateUserCmdHandler : IRequestHandler<CreateUserCmd, long>
  {
    private readonly IUserRepository _userRepository;

    public CreateUserCmdHandler(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<long> Handle(CreateUserCmd request, CancellationToken cancellationToken)
    {
      var pwdHelper = SecurityHelper.Create();
      var user = new User(request.Email, pwdHelper.CreateSaltedPassword(request.PasswordHash), pwdHelper.Salt);
      _userRepository.Add(user);

      await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
      return user.Id;
    }
  }
}