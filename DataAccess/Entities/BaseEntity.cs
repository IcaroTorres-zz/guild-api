using Domain.Entities;
using Domain.Validations;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Serializable]
    public abstract class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            ValidationResult = new CreatedValidationResult(this);
        }
        public Guid Id { get; protected set; }
        [JsonIgnore] public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
        [JsonIgnore] public DateTime ModifiedDate { get; protected set; } = DateTime.UtcNow;
        [JsonIgnore] public bool Disabled { get; protected set; } = false;
        public DateTime RegisterCreation()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = CreatedDate;
            return CreatedDate;
        }
        public DateTime RegisterModification() => ModifiedDate = DateTime.UtcNow;
        public virtual IValidationResult Validate() => ValidationResult;
        [NotMapped, JsonIgnore] public virtual IValidationResult ValidationResult { get; set; }
        [NotMapped, JsonIgnore] public virtual bool IsValid => Validate().IsValid;
    }
}