using Newtonsoft.Json;

namespace Hateoas.DTOs
{
    public class ArrayResourceDTO<T> : ResourceDTO where T : ResourceDTO
    {
        public ArrayResourceDTO(T[] valores) : base(valores) { }
        private T[] DataArray => base.Data as T[];
        [JsonProperty("items", Order = 2)] public override object Data => DataArray;
        [JsonProperty("count", Order = -1)] public long Count => DataArray.Length;
    }
}
