using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Identity.API.Domain
{
    public class Role : IEquatable<Role>
    {
        public string Name { get; }

        public static readonly Role User = new Role(nameof(User));
        public static readonly Role Admin = new Role(nameof(Admin));
        public static readonly Role Owner = new Role(nameof(Owner));

        private static IDictionary<string, Role> _rolesCache;

        protected Role(string name)
        {
            Name = ToCamelCase(name);
        }

        private Role()
        {
        }

        public static Role Parse(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return null;
            }

            EnsureRolesCachePopulated();

            _rolesCache.TryGetValue(role.ToLowerInvariant(), out var matchingRole);
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
                .ToDictionary(x => x.Name.ToLowerInvariant(), x => x);
        }

        private static string ToCamelCase(string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 0)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }

            return str;
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
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }
            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(Role obj1, Role obj2)
        {
            return !(obj1 == obj2);
        }
    }
}
