using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Business.Commands;
using Business.Responses;
using Domain.Unities;
using MediatR;

namespace Business.Behaviors
{
	public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<IValidationResponse>, IRequest<TResponse>
		where TResponse : IValidationResponse
	{
		private readonly IUnitOfWork _unitOfWork;

		public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			// jump if isn't transactional request 
			if (!request.GetType().GetInterfaces().Contains(typeof(ITransactionalCommand))) return await next();
			
			// wrap database updates resulting from request
			_unitOfWork.BeginTransaction();

			// get response from next pipeline execution 
			var response = await next();

			// finish transaction committing or discarding in case of failures
			if (!response.Failures.Any()) 
				await _unitOfWork.CommitAsync(cancellationToken);
			else 
				await _unitOfWork.RollbackTransactionAsync(cancellationToken);

			return response;
		}
	}
}