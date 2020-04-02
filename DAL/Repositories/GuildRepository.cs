using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
  public class GuildRepository : IGuildRepository
  {
    private readonly IRepository<Guild> BaseRepository;

    public GuildRepository(IRepository<Guild> baseRepository)
    {
      BaseRepository = baseRepository;
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<Guild, bool>> predicate)
    {
      return await BaseRepository.ExistsAsync(predicate);
    }

    public virtual async Task<bool> ExistsWithIdAsync(Guid id)
    {
      return await BaseRepository.ExistsWithIdAsync(id);
    }

    public virtual async Task<bool> ExistsWithNameAsync(string name)
    {
      return await BaseRepository.ExistsAsync(x => x.Name.Equals(name));
    }

    public virtual async Task<Guild> GetByIdAsync(Guid id, bool readOnly = false)
    {
      return (await BaseRepository.GetByKeysAsync(id)) ?? new NullGuild();
    }

    public virtual async Task<Guild> GetForMemberHandlingAsync(Guid id)
    {
      return (await BaseRepository.Query()
          .Include(x => x.Members).ThenInclude(x => x.Memberships)
          .Include(x => x.Invites).ThenInclude(x => x.Member)
          .SingleOrDefaultAsync(x => !x.Disabled && x.Id.Equals(id))) ?? new NullGuild();
    }

    public virtual async Task<IReadOnlyList<Guild>> GetAllAsync(bool readOnly = false)
    {
      return await Query(e => true, readOnly).ToListAsync();
    }

    public virtual IQueryable<Guild> Query(Expression<Func<Guild, bool>>? predicate = null, bool readOnly = false)
    {
      return BaseRepository.Query(predicate, readOnly);
    }

    public virtual async Task<Guild> InsertAsync(Guild entity)
    {
      return await BaseRepository.InsertAsync(entity);
    }

    public virtual Guild Update(Guild entity)
    {
      return BaseRepository.Update(entity);
    }

    public virtual Guild Remove(Guild entity)
    {
      return BaseRepository.Remove(entity);
    }
  }
}