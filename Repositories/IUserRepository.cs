using System.Linq;
using api.Models;

namespace api.Repositories
{
  public interface IUserRepository : IRepository<User> { 
    IQueryable<User> GetNthUsers(int count = 20);
  }
}