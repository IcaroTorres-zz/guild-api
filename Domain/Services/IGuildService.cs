using Domain.Entities;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using Domain.Validations;

namespace Domain.Services
{
    public interface IGuildService
    {
        IGuild GetGuild(Guid id);
        IValidationResult Create(GuildDto payload);
        IValidationResult Update(GuildDto payload, Guid id);
        IValidationResult Delete(Guid id);
        IReadOnlyList<IGuild> List(int count = 20);
    }
}