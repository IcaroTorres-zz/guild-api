using System;

namespace Domain.Commands
{
    public interface IQueryItemCommand : IQueryCommand
	{
        Guid Id { get; }
    }
}