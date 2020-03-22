using DataAccess.Entities;
using Domain.Validations;
using Newtonsoft.Json;
using System;

namespace Domain.Models
{
    public abstract class DomainModel<T> where T : EntityModel<T>
    {
        protected DomainModel() { }
        protected DomainModel(T entity)
        {
            Entity = entity;
        }
        [JsonIgnore] public virtual T Entity { get; set; }
        [JsonIgnore] public virtual IValidationResult ValidationResult { get; set; } = new SuccessValidationResult();
        [JsonIgnore] public virtual bool IsValid => Validate().IsValid;
        public virtual IValidationResult Validate() => ValidationResult;
        public override bool Equals(object obj)
        {
            var compareTo = obj as DomainModel<T>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Entity.Id.Equals(compareTo.Entity.Id);
        }
        public static bool operator ==(DomainModel<T> a, DomainModel<T> b) 
        => (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        || !(ReferenceEquals(a, null) || ReferenceEquals(b, null))
        || a.Equals(b);
        public static bool operator !=(DomainModel<T> a, DomainModel<T> b) => !(a == b);
        public override int GetHashCode() => Entity.Id.GetHashCode();
    }
}