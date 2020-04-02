using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Business.Commands.Members
{
    public class UpdateMemberCommand : IRequest<ApiResponse<Member>>
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public Guid GuildId { get; set; }

        public UpdateMemberCommand(string name, Guid guildId, [FromRoute(Name = "id")] Guid id)
        {
            Id = id;
            Name = name;
            GuildId = guildId;
        }
    }
}
