using System.Collections.Generic;

namespace Domain.Hateoas
{
    public interface IApiHateoasFactory
    {
        IDictionary<string, Link> Create(object data);
    }
}
