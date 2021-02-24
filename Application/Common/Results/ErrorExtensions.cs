using System;
using System.Collections.Generic;

namespace Application.Common.Results
{
    public static class ErrorExtensions
    {
        public static List<ApiError> ToApiError(this Exception ex)
        {
            var messages = new List<ApiError>();

            if (ex is null) return messages;

            messages.Add(new ApiError(ex.GetType().Name, ex.Message));
            messages.AddRange(ex.InnerException.ToApiError());

            return messages;
        }
    }
}
