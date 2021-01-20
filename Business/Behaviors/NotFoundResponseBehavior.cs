using Domain.Commands;
using Domain.Messages;
using Domain.Responses;
using MediatR;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Behaviors
{
    public class NotFoundResponseBehavior<TCommand, TResult> : IPipelineBehavior<TCommand, TResult>
        where TCommand : IRequest<TResult>
        where TResult : IApiResult
    {
        public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            var result = await next();
            if (command is IQueryItemCommand queryItemCommand)
            {
                var dataId = result.Data?.GetType().GetProperty(nameof(IQueryItemCommand.Id))?.GetValue(result.Data);
                if (dataId is Guid guid && guid == Guid.Empty)
                {
                    var dataType = result.Data.GetType();
                    var typeName = Regex.Replace(dataType.Name, "dto", "", RegexOptions.IgnoreCase);

                    result.SetExecutionError(
                        HttpStatusCode.NotFound,
                        new DomainMessage(typeName, $"Record not found for {typeName} with given id {queryItemCommand.Id}."));
                }
            }
            return result;
        }
    }
}