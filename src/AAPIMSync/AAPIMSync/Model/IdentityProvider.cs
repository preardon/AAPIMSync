using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class IdentityProvider : ConfigurationBase
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("authority")]
        public string Authority { get; set; }
        [JsonPropertyName("allowedTenants")]
        public List<string> AllowedTenants { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is IdentityProvider && this == (IdentityProvider)obj;
        }
        public static bool operator ==(IdentityProvider x, IdentityProvider y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Type == y.Type &&
                x.Authority == y.Authority &&
                ApimUtils.ListsAreEqual(x.AllowedTenants, y.AllowedTenants) ;
        }
        public static bool operator !=(IdentityProvider x, IdentityProvider y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Type, Authority, AllowedTenants).GetHashCode();
        }
    }
}
