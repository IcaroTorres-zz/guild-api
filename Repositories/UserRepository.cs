
using System;
using System.Collections.Generic;
using System.Linq;
using api.Context;
using api.Models;

namespace api.Repositories
{
  public class UserRepository : Repository<User>, IUserRepository
  {
    public UserRepository(LumenContext context) : base(context) { }
    public LumenContext LumenContext => Context as LumenContext;
    public IQueryable<User> GetNthUsers(int count = 20) => GetAll().Take(count);
  }
}