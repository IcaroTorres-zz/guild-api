namespace Business.Responses
{
	public interface IDataResponse<out T>
	{
		T Data { get; }
	}
}