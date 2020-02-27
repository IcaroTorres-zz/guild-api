using System;

namespace Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        // logic control flag
        public bool Disabled { get; set; } = false;
    }
}