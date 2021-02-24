using System;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Results
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ApiError
    {
        public string Title { get; }
        public string Message { get; }

        public ApiError(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
