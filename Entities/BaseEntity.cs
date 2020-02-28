using Newtonsoft.Json;
using System;

namespace Entities
{
    public abstract class BaseEntity
    {
        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public string ModifiedBy { get; set; }

        // logic control flag
        [JsonIgnore]
        public bool Disabled { get; set; } = false;
    }
}