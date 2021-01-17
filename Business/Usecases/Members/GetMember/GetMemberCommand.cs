using Domain.Commands;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Usecases.Members.GetMember
{
    public class GetMemberCommand : IRequest<IApiResult>, IQueryItemCommand
    {
        [FromRoute] public Guid Id { get; set; }
    }
}