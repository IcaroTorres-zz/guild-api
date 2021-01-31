using Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Business.Commands
{
    [ExcludeFromCodeCoverage]
    public abstract class QueryItemCommand<TIn, TOut> : MappedResultCommand<TIn, TOut>, IQueryItemCommand
    {
        [FromRoute(Name = "id")] public virtual Guid Id { get; set; }
    }
}
