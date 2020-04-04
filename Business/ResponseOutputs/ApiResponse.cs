using System.Collections.Generic;
using FluentValidation.Results;

namespace Business.ResponseOutputs
{
	public class ApiResponse<T>
	{
		public ApiResponse(T value, IList<ValidationFailure> validationFailures)
		{
			Value = value;
			Errors = validationFailures;
		}

		public ApiResponse(IList<ValidationFailure> validationFailures)
		{
			Errors = validationFailures;
		}

		public ApiResponse(T value)
		{
			Value = value;
			Errors = new List<ValidationFailure>();
		}

		public T Value { get; }
		public IList<ValidationFailure> Errors { get; }

		public ApiErrorOutput AsErrorOutput()
		{
			return new ApiErrorOutput(Errors);
		}
	}
}