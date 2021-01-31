using Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Business.Commands
{
    [ExcludeFromCodeCoverage]
    public abstract class QueryListCommand<TIn, TOut> : MappedResultCommand<TIn, TOut>, IQueryListCommand
    {
        [FromQuery(Name = "pageSize")] public virtual int PageSize { get; set; } = 20;
        [FromQuery(Name = "page")] public virtual int Page { get; set; } = 1;
    }
}
