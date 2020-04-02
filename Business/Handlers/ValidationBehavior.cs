using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _preResponseValidators;
        private readonly IEnumerable<IValidator<TResponse>> _posResponseValidators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> preValidators, IEnumerable<IValidator<TResponse>> posValidators)
        {
            _preResponseValidators = preValidators;
            _posResponseValidators = posValidators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);

            var preResponseContext = new ValidationContext(request);
            var failures = _preResponseValidators
                .Select(x => x.Validate(preResponseContext))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (!failures.Any())
            {
                response = await next();
                var posResponseContext = new ValidationContext(response);
                failures.AddRange(_posResponseValidators
                    .Select(x => x.Validate(preResponseContext))
                    .SelectMany(x => x.Errors)
                    .Where(x => x != null)
                    .ToList());
            }

            return response;
        }
    }
}
