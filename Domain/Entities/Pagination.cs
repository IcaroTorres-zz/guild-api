using System;
using System.Collections.Generic;

namespace Domain.Entities
{
	[Serializable]
	public class Pagination<TEntity> where TEntity : class
	{
		public Pagination(IEnumerable<TEntity> data, long count, int pageSize, int page = 1)
		{
			Data = data;
			Page = page;
			PageSize = pageSize;
			Count = count;
		}

		public IEnumerable<TEntity> Data { get; private set; }
		public int Page { get; private set; }
		public int PageSize { get; private set; }
		public long Count { get; private set; }
		public long Pages => Count == 0 ? 1 : (Count + PageSize - 1) / PageSize;
	}
}