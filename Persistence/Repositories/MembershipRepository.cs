using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly IRepository<Membership> _baseRepository;

        public MembershipRepository(IRepository<Membership> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<Membership> InsertAsync(Membership model, CancellationToken cancellationToken = default)
        {
            return await _baseRepository.InsertAsync(model, cancellationToken);
        }

        public Membership Update(Membership model)
        {
            return _baseRepository.Update(model);
        }

        public async Task<Pagination<Membership>> PaginateAsync(Expression<Func<Membership, bool>> predicate = null,
            int top = 20, int page = 1, CancellationToken cancellationToken = default)
        {
            var itemsQuery = _baseRepository.Query(predicate, readOnly: true).Include(x => x.Member).Include(x => x.Guild);

            return await _baseRepository.PaginateAsync(itemsQuery, top, page, cancellationToken);
        }
    }
}