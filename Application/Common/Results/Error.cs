using System;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Results
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class Error
    {
        public string Title { get; }
        public string Message { get; }

        public Error(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
