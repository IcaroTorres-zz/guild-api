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
    public class NotFoundResponseBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
        where TCommand : IRequest<TResponse>
        where TResponse : IApiResult
    {
        public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            if (command is IQueryItemCommand queryItemCommand)
            {
                var dataId = response.Data?.GetType().GetProperty(nameof(IQueryItemCommand.Id))?.GetValue(response.Data);
                if (dataId is Guid guid && guid == Guid.Empty)
                {
                    var dataType = response.Data.GetType();
                    var typeName = Regex.Replace(dataType.Name, "dto", "", RegexOptions.IgnoreCase);

                    response.SetExecutionError(
                        HttpStatusCode.NotFound,
                        new DomainMessage(typeName, $"Record not found for {typeName} with given id {queryItemCommand.Id}."));
                }

            }
            return response;
        }
    }
}