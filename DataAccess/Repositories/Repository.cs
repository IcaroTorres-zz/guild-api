using DataAccess.Context;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityModel<T> 
    {
        private readonly ApiContext Context;
        private readonly DbSet<T> DbSet;
        public Repository(ApiContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public virtual T GetByKeys(params object[] keys) => DbSet.Find(keys);
        public virtual IQueryable<T> GetAll(bool readOnly = false) => Query(e => true, readOnly);
        public virtual IQueryable<T> Query(Expression<Func<T, bool>>? predicate = null, bool readOnly = false)
        {
            var query = DbSet.Where(predicate ?? (e => true));
            return readOnly ? query.AsNoTracking() : query;
        }
        public virtual T Insert(DomainModel<T> domainModel)
        {
            return domainModel.IsValid
                ? DbSet.Add(domainModel.Entity).Entity
                : domainModel.Entity;
        }
        public virtual T Update(DomainModel<T> domainModel)
        {
            return domainModel.IsValid
                ? DbSet.Update(domainModel.Entity).Entity
                : domainModel.Entity;
        }
        public virtual T Remove(DomainModel<T> domainModel)
        {
            return domainModel.IsValid
                ? DbSet.Remove(domainModel.Entity).Entity
                : domainModel.Entity;
        }
        public virtual T Remove(params object[] keys) => DbSet.Remove(GetByKeys(keys)).Entity;

        // public virtual T GetByKeys(params object[] keys)
        // {
        //     var id = Guid.Parse(keys.First().ToString());

        //     if (typeof(T) == typeof(Member))
        //     {
        //         var query = Query<Member>(x => x.Id == id)
        //             .Include(x => x.Memberships)
        //             .Include(x => x.Guild.Members).ThenInclude(m => m.Memberships)
        //             .Include(x => x.Guild.Invites);
        //         return query.SingleOrDefault() as T;
        //     }

        //     if (typeof(T) == typeof(Guild))
        //     {
        //         var query = Query<Guild>(x => x.Id == id)
        //             .Include(x => x.Members).ThenInclude(x => x.Memberships)
        //             .Include(x => x.Invites).ThenInclude(x => x.Member);
        //         return query.SingleOrDefault() as T;
        //     }

        //     return DbSet.Find(keys);
        // }
    }
}