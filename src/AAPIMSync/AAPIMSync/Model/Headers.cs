using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class Headers
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("required")]
        public bool Required { get; set; }
        [JsonPropertyName("values")]
        public List<string> Values { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is Headers && this == (Headers)obj;
        }
        public static bool operator ==(Headers x, Headers y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Name == y.Name &&
                x.Type == y.Type &&
                x.Required == y.Required &&
                ApimUtils.ListsAreEqual(x.Values, y.Values);
        }
        public static bool operator !=(Headers x, Headers y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Name, Type, Required, Values).GetHashCode();
        }
    }
}
