using FluentValidation.Results;
using System.Collections.Generic;

namespace Business.ResponseOutputs
{
  public class ApiResponse<T>
  {
    public T Value { get; }
    public IList<ValidationFailure> Errors { get; }

    public ApiErrorOutput AsErrorOutput()
    {
      return new ApiErrorOutput(Errors);
    }

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
  }
}
