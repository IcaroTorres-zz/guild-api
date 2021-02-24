using Application.Common.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.PipelineBehaviors
{
    public class IncludeHateoasBehavior<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
        where TCommand : IRequest<TResult>
        where TResult : IApiResult
    {
        private readonly IApiHateoasFactory _hateoasFactory;

        public IncludeHateoasBehavior(IApiHateoasFactory hateoasFactory)
        {
            _hateoasFactory = hateoasFactory;
        }

        public async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            var result = await next();

            if (result.Success) result.IncludeHateoas(_hateoasFactory);

            return result;
        }
    }
}
