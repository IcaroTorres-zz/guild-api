using System.Collections.Generic;
using FluentValidation.Results;

namespace Business.Responses
{
	public interface IValidationResponse
	{
		IList<ValidationFailure> Failures { get; }
		ApiErrorOutput GenerateFailuresOutput();
	}
}