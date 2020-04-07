using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Responses;
using FluentValidation;
using MediatR;

namespace Business.Behaviors
{
	public class ResponseValidationBehavior<TReq, TRes> : IPipelineBehavior<TReq, TRes> where TReq : IRequest<TRes>
	{
		private readonly IEnumerable<IValidator<TRes>> _responseValidators;

		public ResponseValidationBehavior(IEnumerable<IValidator<TRes>> responseValidators)
		{
			_responseValidators = responseValidators;
		}

		public async Task<TRes> Handle(TReq request, CancellationToken cancellationToken, RequestHandlerDelegate<TRes> next)
		{
			var response = await next();
			var responseValue = response.GetType().GetProperty(nameof(ApiResponse<object>.Data))?.GetValue(response);
			
			var responseContext = new ValidationContext(response);
			var failures = _responseValidators
				.Select(async x => await x.ValidateAsync(responseContext, cancellationToken))
				.SelectMany(x => x.Result.Errors)
				.Where(x => x != null)
				.ToList();
			
			return failures.Any()
				? (TRes) Activator.CreateInstance(typeof(TRes), responseValue, failures) : response;
		}
	}
}