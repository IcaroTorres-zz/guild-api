using Domain.Hateoas;
using HateoasNet;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Application.Hateoas
{
    [ExcludeFromCodeCoverage]
    public class ApiHateoasFactory : AbstractHateoas<IDictionary<string, Link>>, IApiHateoasFactory
    {
        public ApiHateoasFactory(IHateoas hateoas) : base(hateoas)
        {
        }

        protected override IDictionary<string, Link> GenerateCustom(IEnumerable<HateoasLink> links)
        {
            return links.ToDictionary(x => x.Rel, x => new Link { Href = x.Href, Method = x.Method });
        }

        public IDictionary<string, Link> Create(object data)
        {
            return Generate(data);
        }
    }
}
