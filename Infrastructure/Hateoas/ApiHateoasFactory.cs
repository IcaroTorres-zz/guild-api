using Application.Common.Abstractions;
using Application.Common.Responses;
using HateoasNet;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Infrastructure.Hateoas
{
    [ExcludeFromCodeCoverage]
    public class ApiHateoasFactory : AbstractHateoas<IDictionary<string, LinkResponse>>, IApiHateoasFactory
    {
        public ApiHateoasFactory(IHateoas hateoas) : base(hateoas)
        {
        }

        protected override IDictionary<string, LinkResponse> GenerateCustom(IEnumerable<HateoasLink> links)
        {
            return links.ToDictionary(x => x.Rel, x => new LinkResponse { Href = x.Href, Method = x.Method });
        }

        public IDictionary<string, LinkResponse> Create(object data)
        {
            return Generate(data);
        }
    }
}
