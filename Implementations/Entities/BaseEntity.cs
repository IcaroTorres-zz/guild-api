using Newtonsoft.Json;
using System;

namespace Implementations.Entities
{
    [Serializable]
    public abstract class BaseEntity
    {
        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        // logic control flag
        [JsonIgnore]
        public bool Disabled { get; set; } = false;

        public abstract bool Validate();
    }
}