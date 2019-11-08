using System;

namespace Totten.Solutions.WolfMonitor.Domain.Base
{
    public abstract class Entity
    {
        public virtual Guid Id { get; set; }
        public bool Removed { get; set; }
        public DateTime CreatedIn { get; set; }
        public DateTime UpdatedIn { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id.Equals(Guid.Empty) || other.Id.Equals(Guid.Empty))
                return false;

            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public virtual TEntity Clone<TEntity>()
        {
            return (TEntity)MemberwiseClone();
        }
        public abstract void Validate();
    }
}
