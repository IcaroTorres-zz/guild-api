using DataAccess.Context;
using DataAccess.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public abstract class BaseService : IService
    {
        private readonly ApiContext Context;
        public BaseService(ApiContext context)
        {
            Context = context;
        }

        //public virtual T GetByKeys<T>(params object[] keys) where T : EntityModel<T> => Context.Set<T>().Find(keys);

        public virtual IQueryable<T> GetAll<T>(bool readOnly = false) where T : EntityModel<T> => Query<T>(e => true, readOnly);

        public virtual IQueryable<T> Query<T>(Expression<Func<T, bool>>? predicate = null, bool readOnly = false) where T : EntityModel<T>
        {
            var query = Context.Set<T>().Where(predicate ?? (e => true));
            return readOnly ? query.AsNoTracking() : query;
        }
        public virtual T Insert<T>(DomainModel<T> domainModel) where T : EntityModel<T> => domainModel.IsValid
            ? Context.Set<T>().Add(domainModel.Entity).Entity
            : domainModel.Entity;

        public virtual T Update<T>(DomainModel<T> domainModel) where T : EntityModel<T> => domainModel.IsValid
            ? Context.Set<T>().Update(domainModel.Entity).Entity
            : domainModel.Entity;

        public virtual T Remove<T>(DomainModel<T> domainModel) where T : EntityModel<T> => domainModel.IsValid
            ? Context.Set<T>().Remove(domainModel.Entity).Entity
            : domainModel.Entity;

        public virtual T Remove<T>(params object[] keys) where T : EntityModel<T> => Context.Set<T>().Remove(GetByKeys<T>(keys)).Entity;

        public virtual T GetByKeys<T>(params object[] keys) where T : EntityModel<T>
        {
            var id = Guid.Parse(keys.First().ToString());

            if (typeof(T) == typeof(Member))
            {
                var query = Query<Member>(x => x.Id == id)
                    .Include(x => x.Memberships)
                    .Include(x => x.Guild.Members)
                    .Include(x => x.Guild.Invites);
                return query.SingleOrDefault() as T;
            }

            if (typeof(T) == typeof(Guild))
            {
                var query = Query<Guild>(x => x.Id == id)
                    .Include(x => x.Members).ThenInclude(x => x.Memberships)
                    .Include(x => x.Invites).ThenInclude(x => x.Member);
                return query.SingleOrDefault() as T;
            }

            return Context.Set<T>().Find(keys);
        }

        //protected virtual Member GetMember(Guid memberId, bool readOnly = false)
        //{
        //    var query = Query<Member>(x => x.Id == memberId)
        //        .Include(x => x.Memberships)
        //        .Include(x => x.Guild.Members)
        //        .Include(x => x.Guild.Invites);
        //    return (readOnly ? query.AsNoTracking() : query).SingleOrDefault();
        //}
        //protected Guild GetGuild(Guid id, bool readOnly = false)
        //{
        //    var query = Query<Guild>(x => x.Id == id)
        //        .Include(x => x.Members).ThenInclude(x => x.Memberships)
        //        .Include(x => x.Invites).ThenInclude(x => x.Member);
        //    return (readOnly ? query.AsNoTracking() : query).SingleOrDefault();
        //}
    }
}