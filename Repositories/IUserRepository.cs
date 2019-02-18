using System.Collections.Generic;
using System.Linq;
using lumen.api.Models;

namespace lumen.api.Repositories
{
  public interface IUserRepository : IRepository<User> { 
    IQueryable<User> GetNthUsers(int count = 20);
  }
}