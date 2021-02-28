using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Identity.API.Domain
{
    public class Role : IEquatable<Role>
    {
        public string Name { get; }

        public static readonly Role User = new(nameof(User));
        public static readonly Role Admin = new(nameof(Admin));
        public static readonly Role Owner = new(nameof(Owner));

        private static IDictionary<string, Role> _rolesCache;

        protected Role(string name)
        {
            Name = name.ToUpperInvariant();
        }

        private Role()
        {
        }

        public static Role Parse(string role)
        {
            if (string.IsNullOrWhiteSpace(role)) return null;

            EnsureRolesCachePopulated();

            _rolesCache.TryGetValue(role.ToUpperInvariant(), out var matchingRole);
            return matchingRole;
        }

        public static bool IsValid(string role)
        {
            return Parse(role) != null;
        }

        private static void EnsureRolesCachePopulated()
        {
            _rolesCache ??= typeof(Role)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<Role>()
                .ToDictionary(x => x.Name.ToUpperInvariant(), x => x);
        }

        public bool Equals(Role other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals((Role)obj);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        public static bool operator ==(Role obj1, Role obj2)
        {
            if (ReferenceEquals(obj1, obj2)) return true;
            if (ReferenceEquals(obj1, null)) return false;
            if (ReferenceEquals(obj2, null)) return false;

            return obj1.Equals(obj2);
        }

        public static bool operator !=(Role obj1, Role obj2)
        {
            return !(obj1 == obj2);
        }
    }
}
