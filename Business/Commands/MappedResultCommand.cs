using AutoMapper;
using Domain.Commands;
using Domain.Responses;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Business.Commands
{
    [ExcludeFromCodeCoverage]
    public abstract class MappedResultCommand<TIn, TOut> : IMappedResultCommand, IRequest<IApiResult>
    {
        public virtual object MapResult(object result, IMapper mapper) => mapper.Map<TIn, TOut>((TIn)result);
    }
}
