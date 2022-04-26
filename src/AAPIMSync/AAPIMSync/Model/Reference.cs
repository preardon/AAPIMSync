using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class Reference
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("$ref-value")]
        public string Value { get; set; }


        public override bool Equals(Object obj)
        {
            return obj is Reference && this == (Reference) obj;
        }

        public static bool operator ==(Reference x, Reference y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Value == y.Value;
        }

        public static bool operator !=(Reference x, Reference y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
