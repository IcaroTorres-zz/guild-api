using Business.Responses;
using Domain.Responses;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Behaviors
{
    public class RequestValidationBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
        where TCommand : IRequest<TResponse>
        where TResponse : IApiResult
    {
        private readonly IEnumerable<IValidator<TCommand>> _requestValidators;

        public RequestValidationBehavior(IEnumerable<IValidator<TCommand>> requestValidators)
        {
            _requestValidators = requestValidators;
        }

        public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestContext = new ValidationContext<TCommand>(command);

            var validationTasks = _requestValidators.Select(x => x.ValidateAsync(requestContext, cancellationToken));
            var validations = await Task.WhenAll(validationTasks);
            var failures = validations.SelectMany(x => x.Errors).Where(x => x != null).ToList();

            return failures.Any()
                ? (TResponse)new ApiResult().SetValidationError(failures.ToArray())
                : await next();
        }
    }
}