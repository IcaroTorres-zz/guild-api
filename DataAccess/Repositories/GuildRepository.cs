using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class GuildRepository : IGuildRepository
    {
        private readonly IRepository<Guild> BaseRepository;
        public GuildRepository(IRepository<Guild> baseRepository)
        {
            BaseRepository = baseRepository;
        }
        public virtual Guild Get(Guid id, bool readOnly)
        {
            var query = Query(x => x.Id == id)
                .Include(x => x.Members).ThenInclude(x => x.Memberships)
                .Include(x => x.Invites).ThenInclude(x => x.Member);
                
            return (readOnly ? query.AsNoTracking() : query).SingleOrDefault();
        }
        public virtual IQueryable<Guild> GetAll(bool readOnly = false) => Query(e => true, readOnly);
        public virtual IQueryable<Guild> Query(Expression<Func<Guild, bool>>? predicate = null, bool readOnly = false)
        {
            return BaseRepository.Query(predicate, readOnly);
        }
        public virtual Guild Insert(GuildModel domainModel) => BaseRepository.Insert(domainModel);
        public virtual Guild Update(GuildModel domainModel) => BaseRepository.Update(domainModel);
        public virtual Guild Remove(GuildModel domainModel) => BaseRepository.Remove(domainModel);
    }
}