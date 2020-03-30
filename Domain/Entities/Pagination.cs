using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    [Serializable]
    public class Pagination<TEntity> where TEntity : class
    {
        public int Page { get; private set; } = 1;
        public int PageSize { get; private set; } = 10;
        public long Count { get; private set; } = 0;
        public long Pages => Count == 0 ? 1 : (Count + PageSize - 1) / PageSize;
        public IEnumerable<TEntity> Items { get; private set; }

        public Pagination(IEnumerable<TEntity> items, long count, int pageSize, int page = 1)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            Count = count;
        }
    }
}
