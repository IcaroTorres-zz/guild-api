using Domain.Entities;
using Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Services
{
    public interface IService
    {
        T GetByKeys<T>(params object[] keys) where T : EntityModel<T>;
        IQueryable<T> GetAll<T>(bool readOnly = false) where T : EntityModel<T>;
        IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate = null, bool readOnly = false) where T : EntityModel<T>;
        T Insert<T>(DomainModel<T> domainModel) where T : EntityModel<T>;
        T Update<T>(DomainModel<T> domainModel) where T : EntityModel<T>;
        T Remove<T>(DomainModel<T> domainModel) where T : EntityModel<T>;
        T Remove<T>(params object[] keys) where T : EntityModel<T>;
    }
}