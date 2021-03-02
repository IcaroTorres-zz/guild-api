using Application.Common.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.PipelineBehaviors
{
    public class UnitOfWorkBehavior<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
        where TCommand : IRequest<TResult>
        where TResult : IApiResult
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            if (!(command is ITransactionalCommand)) return await next();

            _unitOfWork.BeginTransaction();
            var result = await next();
            if (result.Success) result = (TResult)await _unitOfWork.CommitAsync(result, cancellationToken);

            return result;
        }
    }
}