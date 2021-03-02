using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Application.Common.Results
{
    [ExcludeFromCodeCoverage]
    public static class ErrorExtensions
    {
        public static List<Error> ToApiError(this Exception ex)
        {
            var messages = new List<Error>();

            if (ex is null) return messages;

            messages.Add(new Error(ex.GetType().Name, ex.Message));
            messages.AddRange(ex.InnerException.ToApiError());

            return messages;
        }

        public static IEnumerable<Error> ToApiError(this ValidationFailure[] validationFailures)
        {
            return (validationFailures ?? new ValidationFailure[] { }).Select(x => new Error(x.PropertyName, x.ErrorMessage));
        }
    }
}
