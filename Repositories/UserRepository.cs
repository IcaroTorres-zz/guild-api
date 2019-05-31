
using System;
using System.Collections.Generic;
using System.Linq;
using api.Context;
using api.Models;

namespace api.Repositories
{
  public class UserRepository : Repository<User>, IUserRepository
  {
    public UserRepository(ApiContext context) : base(context) { }
    public ApiContext ApiContext => Context as ApiContext;
    public IQueryable<User> GetNthUsers(int count = 20) => GetAll().Take(count);
  }
}