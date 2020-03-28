using Domain.Entities;
using Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IInviteRepository
    {
        Invite Get(Guid id, bool readOnly = false);
        IQueryable<Invite> Query(Expression<Func<Invite, bool>> predicate = null, bool readOnly = false);
        Invite Insert(InviteModel domainModel);
        Invite Remove(InviteModel domainModel);
    }
}