using System;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Responses
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class LinkResponse
    {
        public string Method { get; set; }
        public string Href { get; set; }
    }
}
