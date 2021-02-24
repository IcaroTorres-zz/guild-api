using Application.Common.Responses;
using System.Collections.Generic;

namespace Application.Common.Abstractions
{
    public interface IApiHateoasFactory
    {
        IDictionary<string, LinkResponse> Create(object data);
    }
}
