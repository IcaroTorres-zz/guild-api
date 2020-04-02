using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace Business.Commands
{
    public class ApiResponse<T>
    {
        public T Value { get; }
        public IList<DomainFailure> Errors { get; }

        private IList<DomainFailure> ConvertValidationFailures(IList<ValidationFailure> errors)
        {
            return errors.Select(err => new DomainFailure
            {
                Property = err.PropertyName,
                Message = err.ErrorMessage
            }).ToList();
        }

        public ApiResponse(T value, IList<ValidationFailure> errors)
        {
            Value = value;
            Errors = ConvertValidationFailures(errors);
        }

        public ApiResponse(IList<ValidationFailure> errors)
        {
            Errors = ConvertValidationFailures(errors);
        }

        public ApiResponse(T value)
        {
            Value = value;
            Errors = new List<DomainFailure>();
        }

    }
    public class DomainFailure
    {
        public string Property { get; set; }
        public string Message { get; set; }
    }
}
