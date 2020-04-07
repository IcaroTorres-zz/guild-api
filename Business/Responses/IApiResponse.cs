namespace Business.Responses
{
	public interface IApiResponse<out T> : IDataResponse<T>, IValidationResponse
	{
	}
}