using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation.Results;

namespace Business.Responses
{
	public class ApiErrorOutput
	{
		public ApiErrorOutput(IEnumerable<ValidationFailure> validationFailures)
		{
			_errors = ConvertValidationFailures(validationFailures);
		}

		public ApiErrorOutput()
		{
			_errors = new Dictionary<string, string[]>();
		}

		private Dictionary<string, string[]> _errors { get; }

		public string Title { get; set; } = "Request Validation errors.";
		public int Status { get; set; } = (int) HttpStatusCode.BadRequest;
		public IReadOnlyDictionary<string, string[]> Errors => _errors;

		public void AddErrorEntry(string key, params string[] messages)
		{
			if (_errors.TryGetValue(key, out var presentMessages))
				_errors[key] = presentMessages.Concat(messages).ToArray();
			else
				_errors.Add(key, messages);
		}

		private static Dictionary<string, string[]> ConvertValidationFailures(IEnumerable<ValidationFailure> errors)
		{
			return errors
				.GroupBy(x => x.PropertyName)
				.ToDictionary(err => err.Key,
					err => err.Select(e => e.ErrorMessage).ToArray());
		}
	}
}