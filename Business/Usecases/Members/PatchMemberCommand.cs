using Domain.Commands;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Usecases.Members
{
    public class PatchMemberCommand : IRequest<IApiResult>, ITransactionalCommand
    {
        [FromRoute] public Guid Id { get; set; }
    }
}