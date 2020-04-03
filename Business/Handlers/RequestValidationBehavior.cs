using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Handlers
{
  public class RequestValidationBehavior<TReq, TRes> : IPipelineBehavior<TReq, TRes> where TReq : IRequest<TRes>
  {
    private readonly IEnumerable<IValidator<TReq>> _requestValidators;

    public RequestValidationBehavior(IEnumerable<IValidator<TReq>> preValidators)
    {
      _requestValidators = preValidators;
    }

    public async Task<TRes> Handle(TReq request, CancellationToken cancellationToken, RequestHandlerDelegate<TRes> next)
    {
      var requestContext = new ValidationContext(request);
      var failures = _requestValidators
          .Select(x => x.Validate(requestContext))
          .SelectMany(x => x.Errors)
          .Where(x => x != null)
          .ToList();

      return failures.Any()
          ? (TRes)Activator.CreateInstance(typeof(TRes), failures)
          : (await next());
    }
  }
}
