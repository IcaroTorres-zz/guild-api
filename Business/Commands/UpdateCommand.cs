using Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Commands
{
    public abstract class UpdateCommand<TIn, TOut> : MappedResultCommand<TIn, TOut>, ITransactionalCommand
    {
        [FromRoute(Name = "id")] public Guid Id { get; set; }
    }
}