using Application.Common.Abstractions;
using Application.Common.Responses;
using AutoMapper;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class NullObjectProxyRepository<TDomainEntity, TDataEntity> : IRepository<TDomainEntity>
        where TDomainEntity : EntityModel<TDomainEntity>
        where TDataEntity : TDomainEntity
    {
        private readonly ApiContext _context;
        private readonly IRepository<TDomainEntity> _subject;
        private readonly IMapper _mapper;

        public NullObjectProxyRepository(ApiContext context, IMapper mapper)
        {
            _context = context;
            _subject = new Repository<TDomainEntity>(context);
            _mapper = mapper;
        }

        public async Task<bool> ExistsAsync(Expression<Func<TDomainEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _subject.ExistsAsync(predicate, cancellationToken);
        }

        public IQueryable<TDomainEntity> Query(Expression<Func<TDomainEntity, bool>> predicate = null, bool readOnly = false)
        {
            return _subject.Query(predicate, readOnly);
        }

        public async Task<TDomainEntity> InsertAsync(TDomainEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity is INullObject) return entity;

            var (mergedEntity, hasLocalTracking) = MergeEntry(entity, EntityState.Added);

            return hasLocalTracking ? mergedEntity : await _subject.InsertAsync(mergedEntity, cancellationToken);
        }

        public TDomainEntity Update(TDomainEntity entity)
        {
            if (entity is INullObject) return entity;

            var (mergedEntity, hasLocalTracking) = MergeEntry(entity, EntityState.Modified);

            return hasLocalTracking ? mergedEntity : _subject.Update(mergedEntity);
        }

        public async Task<PagedResponse<TDomainEntity>> PaginateAsync(IQueryable<TDomainEntity> query = null, int top = 20, int page = 1, CancellationToken cancellationToken = default)
        {
            return await _subject.PaginateAsync(query, top, page, cancellationToken);
        }

        private (TDomainEntity, bool) MergeEntry(TDomainEntity entity, EntityState state)
        {
            var filteredEntity = _mapper.Map<TDomainEntity, TDataEntity>(entity);
            var entry = _context.Entry<TDomainEntity>(filteredEntity);
            var hasLocalTracking = false;

            if (_context.Set<TDomainEntity>().Local.SingleOrDefault(x => x.Id.Equals(entity.Id)) is { } localEntity)
            {
                entry = _context.Entry(localEntity);
                hasLocalTracking = true;
            }

            entry.CurrentValues.SetValues(filteredEntity);
            entry.State = state;
            return (entry.Entity, hasLocalTracking);
        }
    }
}