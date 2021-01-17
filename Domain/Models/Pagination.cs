using Domain.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Models
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class Pagination<T> where T : class
    {
        public Pagination(List<T> items, int count, int pageSize, int page = 1)
        {
            Items = items;
            Count = count;
            PageSize = Math.Min(pageSize, Count);
            Page = page;
        }

        public List<T> Items { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public int Pages => Count == 0 ? 1 : (Count + PageSize - 1) / PageSize;

        protected IQueryListCommand AppliedCommand { get; set; }

        public TQuery GetCommandAs<TQuery>() where TQuery : class, IQueryListCommand
        {
            return AppliedCommand as TQuery;
        }

        public Pagination<T> SetAppliedCommand(IQueryListCommand command)
        {
            AppliedCommand = command;
            return this;
        }
    }
}
