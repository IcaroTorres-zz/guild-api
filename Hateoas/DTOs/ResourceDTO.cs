using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hateoas.DTOs
{
    public abstract class ResourceDTO
    {
        public ResourceDTO(object data)
        {
            Data = data;
        }
        [JsonProperty("data", Order = 2)] public virtual object Data { get; }
        [JsonProperty("links", Order = -2)] public virtual List<LinkDTO> Links { get; } = new List<LinkDTO>();
    }
}
