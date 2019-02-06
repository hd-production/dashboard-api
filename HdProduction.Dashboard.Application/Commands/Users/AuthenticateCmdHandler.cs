using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Exceptions;
using HdProduction.Dashboard.Domain.Services;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Users
{
  public class AuthenticateCmdHandler : IRequestHandler<AuthenticateCmd, (string jwtToken, string refreshToken)>
  {
    private readonly IUserRepository _userRepository;
    private readonly ISessionTokenService _sessionTokenService;

    public AuthenticateCmdHandler(IUserRepository userRepository, ISessionTokenService sessionTokenService)
    {
      _userRepository = userRepository;
      _sessionTokenService = sessionTokenService;
    }

    public async Task<(string jwtToken, string refreshToken)> Handle(AuthenticateCmd request, CancellationToken cancellationToken)
    {
      var user = await _userRepository.FindByEmailAsync(request.Email);
      var pwhHelper = new SecurityHelper(user?.PwdSalt ?? throw InvalidCredentials());

      if (!pwhHelper.CheckPassword(request.PwdHash, user.SaltedPwd))
      {
        throw InvalidCredentials();
      }

      var refreshToken = pwhHelper.CreateRefreshToken();
      user.SetRefreshToken(refreshToken);

      await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

      return (_sessionTokenService.CreateToken(user), refreshToken);
    }

    private static BusinessLogicException InvalidCredentials()
    {
      return new BusinessLogicException("Email or password is not valid");
    }
  }
}