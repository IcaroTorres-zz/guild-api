using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
  public class MemberRepository : IMemberRepository
  {
    private readonly IRepository<Member> BaseRepository;

    public MemberRepository(IRepository<Member> baseRepository)
    {
      BaseRepository = baseRepository;
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<Member, bool>> predicate)
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

    public virtual async Task<Member> GetByIdAsync(Guid id, bool readOnly = false)
    {
      return (await BaseRepository.GetByKeysAsync(id)) ?? new NullMember();
    }

    public virtual async Task<Member> GetByNameAsync(string name, bool readOnly = false)
    {
      var query = Query(x => x.Name.Equals(name) && !x.Disabled);

      return await (readOnly ? query.AsNoTracking() : query).SingleOrDefaultAsync() ?? new NullMember();
    }

    public virtual async Task<Member> GetForGuildOperationsAsync(Guid id)
    {
      return (await BaseRepository.Query()
          .Include(x => x.Memberships)
          .Include(x => x.Guild)
          .ThenInclude(x => x.Members)
          .SingleOrDefaultAsync(x => x.Id.Equals(id))) ?? new NullMember();
    }

    public virtual IQueryable<Member> Query(Expression<Func<Member, bool>>? predicate = null, bool readOnly = false)
    {
      return BaseRepository.Query(predicate, readOnly);
    }

    public virtual async Task<Member> InsertAsync(Member entity)
    {
      return await BaseRepository.InsertAsync(entity);
    }

    public virtual Member Update(Member entity)
    {
      return BaseRepository.Update(entity);
    }

    public virtual Member Remove(Member entity)
    {
      return BaseRepository.Remove(entity);
    }
  }
}