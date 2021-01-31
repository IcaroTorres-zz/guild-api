using System;

namespace Domain.Commands
{
    public interface IQueryItemCommand
	{
        Guid Id { get; }
    }
}