using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class InviteRepository : IInviteRepository
    {
        private readonly IRepository<Invite> BaseRepository;

        public InviteRepository(IRepository<Invite> baseRepository)
        {
            BaseRepository = baseRepository;
        }

        public async Task<bool> ExistsWithIdAsync(Guid id)
        {
            return await BaseRepository.ExistsWithIdAsync(id);
        }

        public virtual async Task<Invite> GetByIdAsync(Guid id, bool readOnly = false)
        {
            var query = Query(x => x.Id == id && !x.Disabled);

            return await (readOnly ? query.AsNoTracking() : query).SingleOrDefaultAsync() ?? new NullInvite();
        }

        public virtual async Task<Invite> GetForAcceptOperation(Guid id)
        {
            return (await Query().Include(x => x.Guild).ThenInclude(g => g.Members)
                .Include(x => x.Member).ThenInclude(m => m.Memberships)
                .SingleOrDefaultAsync(x => x.Id.Equals(id) && !x.Disabled)) ?? new NullInvite();
        }

        public virtual IQueryable<Invite> Query(Expression<Func<Invite, bool>>? predicate = null, bool readOnly = false)
        {
            return BaseRepository.Query(predicate, readOnly);
        }

        public virtual async Task<Invite> InsertAsync(Invite entity)
        {
            return await BaseRepository.InsertAsync(entity);
        }

        public virtual Invite Remove(Invite entity)
        {
            return BaseRepository.Remove(entity);
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<Invite, bool>> predicate)
        {
            return await BaseRepository.ExistsAsync(predicate);
        }


    }
}