using Domain.Entities;
using Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IRepository<T> where T : EntityModel<T>
    {
        T GetByKeys(params object[] keys);
        IQueryable<T> GetAll(bool readOnly = false);
        IQueryable<T> Query(Expression<Func<T, bool>> predicate = null, bool readOnly = false);
        T Insert(DomainModel<T> domainModel);
        T Update(DomainModel<T> domainModel);
        T Remove(DomainModel<T> domainModel);
        bool Exists(Expression<Func<T, bool>> predicate);
    }
}