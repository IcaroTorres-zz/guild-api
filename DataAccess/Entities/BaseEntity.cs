using DataAccess.Validations;
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
        public Guid Id { get; protected set; } = Guid.NewGuid();
        [NotMapped, JsonIgnore] public virtual IValidationResult ValidationResult { get; set; } = new SuccessValidationResult();
        [NotMapped, JsonIgnore] public virtual bool IsValid => Validate().IsValid;
        [JsonIgnore] public virtual DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
        [JsonIgnore] public virtual DateTime ModifiedDate { get; protected set; } = DateTime.UtcNow;
        [JsonIgnore] public bool Disabled { get; protected set; } = false;
        public virtual IValidationResult Validate() => ValidationResult;
        public DateTime RegisterCreation()
        {
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = CreatedDate;
            return CreatedDate;
        }
        public DateTime RegisterModification() => ModifiedDate = DateTime.UtcNow;
    }
}