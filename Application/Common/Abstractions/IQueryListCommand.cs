namespace Application.Common.Abstractions
{
    public interface IQueryListCommand
    {
        int Page { get; }
        int PageSize { get; }
    }
}