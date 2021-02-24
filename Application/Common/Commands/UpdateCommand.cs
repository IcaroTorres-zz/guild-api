using Application.Common.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.Common.Commands
{
    public abstract class UpdateCommand<TIn, TOut> : MappedResultCommand<TIn, TOut>, ITransactionalCommand
    {
        [FromRoute(Name = "id")] public Guid Id { get; set; }
    }
}