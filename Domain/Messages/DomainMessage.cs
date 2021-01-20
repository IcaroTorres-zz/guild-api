using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Messages
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class DomainMessage
    {
        public string Title { get; }
        public string Message { get; }

        public DomainMessage(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
