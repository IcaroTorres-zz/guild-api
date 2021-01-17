using Domain.Commands;
using Domain.Responses;
using Domain.Unities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Behaviors
{
    public class UnitOfWorkBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
        where TCommand : IRequest<TResponse>
        where TResponse : IApiResult
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (command is ITransactionalCommand)
            {
                _unitOfWork.BeginTransaction();

                var response = await next();

                if (!response.Errors.Any()) await _unitOfWork.CommitAsync(cancellationToken);
                else await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                return response;
            }

            return await next();
        }
    }
}