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
            var query = Query(x => x.Id == id)
                .Include(x => x.Memberships)
                .Include(x => x.Guild.Members).ThenInclude(m => m.Memberships)
                .Include(x => x.Guild.Invites);
                
            return (readOnly ? query.AsNoTracking() : query).SingleOrDefault();
        }
        public virtual IQueryable<Member> Query(Expression<Func<Member, bool>>? predicate = null, bool readOnly = false)
        {
            return BaseRepository.Query(predicate, readOnly);
        }
        public virtual Member Insert(MemberModel domainModel) => BaseRepository.Insert(domainModel);
        public virtual Member Update(MemberModel domainModel) => BaseRepository.Update(domainModel);
        public virtual Member Remove(MemberModel domainModel) => BaseRepository.Remove(domainModel);
    }
}