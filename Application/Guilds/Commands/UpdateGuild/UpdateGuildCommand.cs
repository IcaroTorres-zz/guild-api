using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;
using System;

namespace Application.Guilds.Commands.UpdateGuild
{
    public class UpdateGuildCommand : UpdateCommand<Guild, GuildResponse>
    {
        public string Name { get; set; }
        public Guid LeaderId { get; set; }
    }
}