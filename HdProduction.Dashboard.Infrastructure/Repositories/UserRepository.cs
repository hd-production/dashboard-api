using System.Threading.Tasks;
using HdProduction.Dashboard.Domain.Contracts;
using HdProduction.Dashboard.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HdProduction.Dashboard.Infrastructure.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
      _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Add(User user)
    {
      _context.Users.Add(user);
    }

    public Task<User> FindAsync(long id, bool withTracking = true)
    {
      return withTracking
        ? _context.Users.FindAsync(id)
        : _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public Task<User> FindByEmailAsync(string email)
    {
      return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
  }
}