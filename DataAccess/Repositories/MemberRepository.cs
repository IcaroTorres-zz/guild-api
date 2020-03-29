using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IRepository<Member> BaseRepository;
        public MemberRepository(IRepository<Member> baseRepository)
        {
            BaseRepository = baseRepository;
        }
        public virtual Member Get(Guid id, bool readOnly)
        {
            var query = Query(x => x.Id == id);
                
            return (readOnly ? query.AsNoTracking() : query).SingleOrDefault();
        }
        public virtual IQueryable<Member> Query(Expression<Func<Member, bool>>? predicate = null, bool readOnly = false)
        {
            return BaseRepository.Query(predicate, readOnly)
                .Include(x => x.Memberships)
                .Include(x => x.Guild.Members).ThenInclude(m => m.Memberships)
                .Include(x => x.Guild.Invites);
        }
        public virtual Member Insert(DomainModel<Member> domainModel) => BaseRepository.Insert(domainModel);
        public virtual Member Update(DomainModel<Member> domainModel) => BaseRepository.Update(domainModel);
        public virtual Member Remove(DomainModel<Member> domainModel) => BaseRepository.Remove(domainModel);
    }
}