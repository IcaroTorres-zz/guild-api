using Domain.Entities;
using Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IGuildRepository
    {
        Guild Get(Guid id, bool readOnly = false);
        IQueryable<Guild> GetAll(bool readOnly = false);
        IQueryable<Guild> Query(Expression<Func<Guild, bool>> predicate = null, bool readOnly = false);
        Guild Insert(DomainModel<Guild> domainModel);
        Guild Update(DomainModel<Guild> domainModel);
        Guild Remove(DomainModel<Guild> domainModel);
        bool Exists(Expression<Func<Guild, bool>> predicate);
    }
}