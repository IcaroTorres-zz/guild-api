using Domain.Entities;
using Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IMemberRepository
    {
        Member Get(Guid id, bool readOnly = false);
        IQueryable<Member> Query(Expression<Func<Member, bool>> predicate = null, bool readOnly = false);
        Member Insert(DomainModel<Member> domainModel);
        Member Update(DomainModel<Member> domainModel);
        Member Remove(DomainModel<Member> domainModel);
    }
}