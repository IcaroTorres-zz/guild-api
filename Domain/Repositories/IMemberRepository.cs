using Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories
{
  public interface IMemberRepository
  {
    Task<bool> ExistsAsync(Expression<Func<Member, bool>> predicate);
    Task<bool> ExistsWithIdAsync(Guid id);
    Task<bool> ExistsWithNameAsync(string name);
    Task<Member> GetByIdAsync(Guid id, bool readOnly = false);
    Task<Member> GetByNameAsync(string name, bool readOnly = false);
    Task<Member> GetForGuildOperationsAsync(Guid id);
    IQueryable<Member> Query(Expression<Func<Member, bool>> predicate = null, bool readOnly = false);
    Task<Member> InsertAsync(Member entity);
    Member Update(Member entity);
    Member Remove(Member entity);

  }
}