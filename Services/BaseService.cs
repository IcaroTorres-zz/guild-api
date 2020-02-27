using Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class BaseService : IBaseService
    {
        private readonly ApiContext Context;
        public BaseService(ApiContext context)
        {
            Context = context;
        }

        public virtual T GetWithKeys<T>(params object[] keys) where T : class
        {
            return GetWithKeys<T>(keys, null, null);
        }

        public virtual T GetWithKeys<T>(object[] keys, IEnumerable<string> navications = null, IEnumerable<string> collections = null) where T : class
        {
            var entity = Context.Set<T>().Find(keys);

            if (entity != null)
            {
                (navications ?? new string[] { }).Aggregate(entity, (e, navegation) =>
                {
                    Context.Entry<T>(e).Reference(navegation).Load();
                    return e;
                });

                (collections ?? new string[] { }).Aggregate(entity, (e, collection) =>
                {
                    Context.Entry<T>(e).Collection(collection).Load();
                    return e;
                });

            }
            return entity;
        }

        public virtual IQueryable<T> GetAll<T>(string included = "", bool readOnly = false) where T : class
        {
            return Query<T>(e => true, readOnly, included);
        }

        public virtual IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate = null, bool readOnly = false, string included = "") where T : class
        {
            var query = included
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(Context.Set<T>() as IQueryable<T>, (set, navigation) => set.Include<T>(navigation))
                .Where(predicate ?? (e => true));

            return readOnly ? query.AsNoTracking() : query;
        }

        public virtual T Insert<T>(T entity) where T : class
        {
            return Context.Set<T>().Add(entity).Entity;
        }

        public virtual IEnumerable<T> InsertMany<T>(IEnumerable<T> entities) where T : class
        {
            Context.Set<T>().AddRange(entities);

            return entities;
        }

        public virtual T Remove<T>(T entity) where T : class
        {
            return Context.Set<T>().Remove(entity).Entity;
        }

        public virtual T Remove<T>(params object[] keys) where T : class
        {
            return Context.Set<T>().Remove(GetWithKeys<T>(keys)).Entity;
        }

        public virtual IEnumerable<T> RemoveMany<T>(IEnumerable<T> entities) where T : class
        {
            Context.Set<T>().RemoveRange(entities);

            return entities;
        }

        public virtual IEnumerable<T> RemoveMany<T>(IEnumerable<object[]> keysList) where T : class
        {
            foreach (var keys in keysList)
            {
                yield return Remove<T>(keys);
            }
        }
    }
}