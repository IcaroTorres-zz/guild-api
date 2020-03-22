using System;
using Newtonsoft.Json;

namespace DataAccess.Entities
{
    [Serializable]
    public abstract class EntityModel<T> where T : EntityModel<T>
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        [JsonIgnore] public virtual DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
        [JsonIgnore] public virtual DateTime ModifiedDate { get; protected set; } = DateTime.UtcNow;
        [JsonIgnore] public bool Disabled { get; protected set; } = false;

        public override bool Equals(object obj)
        {
            var compareTo = obj as EntityModel<T>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }
        public static bool operator ==(EntityModel<T> a, EntityModel<T> b) 
        => (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        || !(ReferenceEquals(a, null) || ReferenceEquals(b, null))
        || a.Equals(b);
        public static bool operator !=(EntityModel<T> a, EntityModel<T> b) => !(a == b);
        public override int GetHashCode() => Id.GetHashCode();
    }
}