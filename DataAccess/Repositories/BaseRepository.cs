using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Entities;
using Domain.Models;

namespace DataAccess.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        private readonly ApiContext Context;
        public BaseRepository(ApiContext context)
        {
            Context = context;
        }

        public virtual T GetByKeys<T>(object[] keys) where T : EntityModel<T> => Context.Set<T>().Find(keys);
        public virtual IQueryable<T> GetAll<T>(string included = "", bool readOnly = false) where T : EntityModel<T> => Query<T>(e => true, readOnly);
        public virtual IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate = null, bool readOnly = false) where T : EntityModel<T>
        {
            var query = Context.Set<T>().Where(predicate ?? (e => true));
            return readOnly ? query.AsNoTracking() : query;
        }
        public virtual T Insert<T>(DomainModel<T> domainModel) where T : EntityModel<T> => (domainModel.IsValid)
            ? Context.Set<T>().Add(domainModel.Entity).Entity
            : domainModel.Entity;
        public virtual T Update<T>(DomainModel<T> domainModel) where T : EntityModel<T> => (domainModel.IsValid)
            ? Context.Set<T>().Update(domainModel.Entity).Entity
            : domainModel.Entity;
        public virtual T Remove<T>(DomainModel<T> domainModel) where T : EntityModel<T> => (domainModel.IsValid)
            ? Context.Set<T>().Remove(domainModel.Entity).Entity
            : domainModel.Entity;
        public virtual T Remove<T>(params object[] keys) where T : EntityModel<T> => Context.Set<T>().Remove(GetByKeys<T>(keys)).Entity;
        protected virtual Member GetMember(Guid memberId, bool readOnly = false)
        {
            var query = Query<Member>(m => m.Id == memberId)
                .Include(m => m.Memberships)
                .Include(m => m.Guild.Members)
                .Include(m => m.Guild.Invites);
            return (readOnly ? query.AsNoTracking() : query).SingleOrDefault();
        }
        protected Guild GetGuild(Guid id, bool readOnly = false)
        {
            var query = Query<Guild>(g => g.Id == id)
                .Include(g => g.Members).ThenInclude(m => m.Memberships)
                .Include(g => g.Invites).ThenInclude(i => i.Member);
            return (readOnly ? query.AsNoTracking() : query).SingleOrDefault();
        }
    }
}