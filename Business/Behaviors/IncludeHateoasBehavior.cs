using Domain.Hateoas;
using Domain.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Behaviors
{
    public class IncludeHateoasBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
        where TCommand : IRequest<TResponse>
        where TResponse : IApiResult
    {
        private readonly IApiHateoasFactory _hateoasFactory;

        public IncludeHateoasBehavior(IApiHateoasFactory hateoasFactory)
        {
            _hateoasFactory = hateoasFactory;
        }

        public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            if (response.Success) response.IncludeHateoas(_hateoasFactory);

            return response;
        }
    }
}
