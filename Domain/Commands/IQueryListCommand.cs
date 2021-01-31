namespace Domain.Commands
{
    public interface IQueryListCommand
	{
		int Page { get; }
		int PageSize { get; }
	}
}