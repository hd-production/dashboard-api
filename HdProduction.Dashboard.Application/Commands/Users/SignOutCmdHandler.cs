using System.Threading;
using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Exceptions;
using MediatR;

namespace HdProduction.Dashboard.Application.Commands.Users
{
  public class SignOutCmdHandler : IRequestHandler<SignOutCmd>
  {
    private readonly IUserRepository _userRepository;

    public SignOutCmdHandler(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<Unit> Handle(SignOutCmd request, CancellationToken cancellationToken)
    {
      var user = await _userRepository.FindAsync(request.UserId);
      if (user == null)
      {
        throw new EntityNotFoundException("User not found");
      }

      user.SetRefreshToken(null);
      await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }
  }
}