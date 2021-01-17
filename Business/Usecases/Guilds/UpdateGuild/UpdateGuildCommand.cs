using Domain.Commands;
using Domain.Responses;
using MediatR;
using System;

namespace Business.Usecases.Guilds.UpdateGuild
{
    public class UpdateGuildCommand : IRequest<IApiResult>, ITransactionalCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid MasterId { get; set; }
    }
}