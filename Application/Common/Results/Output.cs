using Application.Common.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Results
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class Output
    {
        public object Data { get; set; }
        public bool Success { get; set; } = true;
        public IEnumerable<Error> Errors { get; set; } = new List<Error>();
        public IDictionary<string, LinkResponse> Links { get; set; } = new Dictionary<string, LinkResponse>(StringComparer.InvariantCultureIgnoreCase);
    }

    [Serializable]
    [ExcludeFromCodeCoverage]
    public class Output<T>
    {
        public T Data { get; }
        public bool Success { get; }
        public IEnumerable<Error> Errors { get; }
        public IDictionary<string, LinkResponse> Links { get; }
    }
}
