using Domain.Hateoas;
using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Responses
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class Result
    {
        public object Data { get; set; }
        public bool Success { get; set; } = true;
        public IEnumerable<DomainMessage> Errors { get; set; } = new List<DomainMessage>();
        public IDictionary<string, Link> Links { get; set; } = new Dictionary<string, Link>(StringComparer.InvariantCultureIgnoreCase);
    }

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class Result<T>
    {
        public T Data { get; }
        public bool Success { get; }
        public IEnumerable<DomainMessage> Errors { get; }
        public IDictionary<string, Link> Links { get; }
    }
}
