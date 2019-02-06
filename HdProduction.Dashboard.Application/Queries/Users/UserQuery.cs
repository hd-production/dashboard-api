using System.Threading.Tasks;
using AutoMapper;
using HdProduction.Dashboard.Application.Models;
using HdProduction.Dashboard.Domain.Contracts;

namespace HdProduction.Dashboard.Application.Queries.Users
{
  public interface IUserQuery
  {
    Task<UserReadModel> FindAsync(long id);
  }

  public class UserQuery : IUserQuery
  {
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserQuery(IUserRepository userRepository, IMapper mapper)
    {
      _userRepository = userRepository;
      _mapper = mapper;
    }

    public async Task<UserReadModel> FindAsync(long id)
    {
      return _mapper.Map<UserReadModel>(await _userRepository.FindAsync(id, false));
    }
  }
}