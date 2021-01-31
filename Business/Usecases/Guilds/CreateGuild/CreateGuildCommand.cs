using Business.Commands;
using Business.Dtos;
using Domain.Models;
using System;

namespace Business.Usecases.Guilds.CreateGuild
{
    public class CreateGuildCommand : CreationCommand<Guild, GuildDto>
    {
        public string Name { get; set; }
        public Guid MasterId { get; set; }
    }
}