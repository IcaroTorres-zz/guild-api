using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class InviteRepository : IInviteRepository
    {
        private readonly IRepository<Invite> BaseRepository;
        public InviteRepository(IRepository<Invite> baseRepository)
        {
            BaseRepository = baseRepository;
        }
        public virtual Invite Get(Guid id, bool readOnly = false)
        {
            var query = Query(x => x.Id == id);

            return (readOnly ? query.AsNoTracking() : query).SingleOrDefault();
        }
        public virtual IQueryable<Invite> Query(Expression<Func<Invite, bool>>? predicate = null, bool readOnly = false)
        {
            return BaseRepository.Query(predicate, readOnly)
                .Include(x => x.Member.Memberships)
                .Include(x => x.Guild.Members).ThenInclude(m => m.Memberships);
        }
        public virtual Invite Insert(DomainModel<Invite> domainModel) => BaseRepository.Insert(domainModel);
        public virtual Invite Remove(DomainModel<Invite> domainModel) => BaseRepository.Remove(domainModel);
        public virtual bool Exists(Expression<Func<Invite, bool>> predicate) => BaseRepository.Exists(predicate);
    }
}