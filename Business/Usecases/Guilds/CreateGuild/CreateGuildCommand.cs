using Domain.Commands;
using System;

namespace Business.Usecases.Guilds.CreateGuild
{
    public class CreateGuildCommand : CreationCommand
    {
        public string Name { get; set; }
        public Guid MasterId { get; set; }
    }
}