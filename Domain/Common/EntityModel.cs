using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Common
{
    /// <summary>
    /// Represents a generic base entity model abstraction holding <see cref="Id"/>, <see cref="CreatedDate"/>,
    /// <see cref="ModifiedDate"/> and <see cref="Disabled"/> properties.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public abstract class EntityModel<T> where T : EntityModel<T>
    {
        public virtual Guid Id { get; protected internal set; }
        public virtual DateTime CreatedDate { get; protected internal set; } = DateTime.UtcNow;
        public virtual DateTime? ModifiedDate { get; protected internal set; }
        public bool Disabled { get; protected internal set; } = false;

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}