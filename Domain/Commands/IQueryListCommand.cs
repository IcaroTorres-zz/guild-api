namespace Domain.Commands
{
    public interface IQueryListCommand : IQueryCommand
	{
		int Page { get; }
		int PageSize { get; }
	}
}