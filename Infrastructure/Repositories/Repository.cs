using Infrastructure.Context;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
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
        public bool Exists(Expression<Func<T, bool>> predicate) => Query(predicate, true).Any();
    }
}