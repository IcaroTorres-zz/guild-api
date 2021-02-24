using Application.Common.Commands;
using Application.Common.Responses;
using Domain.Models;
using System;

namespace Application.Guilds.Commands.CreateGuild
{
    public class CreateGuildCommand : CreationCommand<Guild, GuildResponse>
    {
        public string Name { get; set; }
        public Guid LeaderId { get; set; }
    }
}