using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace Business.Behaviors
{
	public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _requestValidators;

		public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> requestValidators)
		{
			_requestValidators = requestValidators;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
			RequestHandlerDelegate<TResponse> next)
		{
			var requestContext = new ValidationContext(request);
			var failures = _requestValidators
				.Select(async x => await x.ValidateAsync(requestContext, cancellationToken))
				.SelectMany(x => x.Result.Errors)
				.Where(x => x != null)
				.ToList();

			return failures.Any()
				? (TResponse) Activator.CreateInstance(typeof(TResponse), failures)
				: await next();
		}
	}
}