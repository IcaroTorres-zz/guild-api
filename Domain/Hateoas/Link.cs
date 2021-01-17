using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Hateoas
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class Link
    {
        public string Method { get; set; }
        public string Href { get; set; }
    }
}
