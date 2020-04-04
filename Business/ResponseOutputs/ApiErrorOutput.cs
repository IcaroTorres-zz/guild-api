using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation.Results;

namespace Business.ResponseOutputs
{
	public class ApiErrorOutput
	{
		public ApiErrorOutput(IList<ValidationFailure> validationFailures)
		{
			errors = ConvertValidationFailures(validationFailures);
		}

		public ApiErrorOutput()
		{
			errors = new Dictionary<string, string[]>();
		}

		private Dictionary<string, string[]> errors { get; }

		public string Title { get; set; } = "Request Validation errors.";
		public int Status { get; set; } = (int) HttpStatusCode.BadRequest;
		public IReadOnlyDictionary<string, string[]> Errors => errors;

		public void AddErrorEntry(string key, params string[] messages)
		{
			if (errors.TryGetValue(key, out var presentMessages))
				errors[key] = presentMessages.Concat(messages).ToArray();
			else
				errors.Add(key, messages);
		}

		private Dictionary<string, string[]> ConvertValidationFailures(IList<ValidationFailure> errors)
		{
			return errors
				.GroupBy(x => x.PropertyName)
				.ToDictionary(err => err.Key,
					err => err.Select(e => e.ErrorMessage).ToArray());
		}
	}
}