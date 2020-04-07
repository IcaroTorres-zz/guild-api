using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Business.Responses
{
	public class ApiResponse<T> : IApiResponse<T>
	{
		public T Data { get; }
		public IList<ValidationFailure> Failures { get; } = new List<ValidationFailure>();
		public ApiResponse(T value, IList<ValidationFailure> validationFailures)
		{
			Data = value;
			Failures = validationFailures;
		}

		public ApiResponse(IList<ValidationFailure> validationFailures)
		{
			Failures = validationFailures;
		}

		public ApiResponse(T value)
		{
			Data = value;
		}

		public ApiErrorOutput GenerateFailuresOutput() => new ApiErrorOutput(Failures);
	}
}