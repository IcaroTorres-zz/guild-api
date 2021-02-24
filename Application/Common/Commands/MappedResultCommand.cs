using Application.Common.Abstractions;
using AutoMapper;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Commands
{
    [ExcludeFromCodeCoverage]
    public abstract class MappedResultCommand<TIn, TOut> : IMappedResultCommand, IRequest<IApiResult>
    {
        public virtual object MapResult(object result, IMapper mapper) => mapper.Map<TIn, TOut>((TIn)result);
    }
}
