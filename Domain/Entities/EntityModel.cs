using System;
using Newtonsoft.Json;

namespace Domain.Entities
{
	[Serializable]
	public abstract class EntityModel<T> where T : EntityModel<T>
	{
		[JsonRequired] public Guid Id { get; protected internal set; } = Guid.NewGuid();
		[JsonIgnore] public virtual DateTime CreatedDate { get; internal set; } = DateTime.UtcNow;
		[JsonIgnore] public virtual DateTime ModifiedDate { get; internal set; } = DateTime.UtcNow;
		[JsonIgnore] public bool Disabled { get; protected set; } = false;

		public override bool Equals(object obj)
		{
			var compareTo = obj as EntityModel<T>;

			return ReferenceEquals(this, compareTo) || !(compareTo is null) && Id.Equals(compareTo.Id);
		}

		public static bool operator ==(EntityModel<T> a, EntityModel<T> b)
		{
			return a is null && b is null || !(a is null || b is null) || (a?.Equals(b) ?? false);
		}

		public static bool operator !=(EntityModel<T> a, EntityModel<T> b)
		{
			return !(a == b);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}