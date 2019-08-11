using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guild.Entities
{
    public abstract class Entity<TKey>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }

        #region Audit Fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        #endregion

        // logic control flag
        public bool Disabled { get; set; } = false;
    }
}