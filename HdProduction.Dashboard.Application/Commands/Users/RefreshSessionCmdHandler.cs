using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Exceptions;
using HdProduction.Dashboard.Domain.Services;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Users
{
  public class RefreshSessionCmdHandler : IRequestHandler<RefreshSessionCmd, (string jwtToken, string refreshToken)>
  {
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshSessionCmdHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
      _userRepository = userRepository;
      _jwtTokenService = jwtTokenService;
    }

    public async Task<(string jwtToken, string refreshToken)> Handle(RefreshSessionCmd request, CancellationToken cancellationToken)
    {
      var user = await _userRepository.FindAsync(request.UserId);
      if (user == null || user.RefreshToken != request.RefreshToken)
      {
        throw new BusinessLogicException("Wrong refresh token");
      }

      var securityHelper = new SecurityHelper(user.PwdSalt);
      var refreshToken = securityHelper.CreateRefreshToken();
      user.SetRefreshToken(refreshToken);

      await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

      return (_jwtTokenService.CreateToken(user), refreshToken);
    }
  }
}