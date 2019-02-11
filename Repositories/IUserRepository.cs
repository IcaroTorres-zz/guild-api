using System.Collections.Generic;
using lumen.api.Models;

namespace lumen.api.Repositories
{
  public interface IUserRepository : IRepository<User> {
    new User Get(string id);
  }
}