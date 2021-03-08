using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class Group : ConfigurationBase
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("$ref-description")]
        public string RefDescription { get; set; }
        [JsonPropertyName("builtIn")]
        public bool BuiltIn { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("externalId")]
        public object ExternalId { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is Group && this == (Group)obj;
        }
        public static bool operator ==(Group x, Group y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                x.RefDescription == y.RefDescription &&
                x.BuiltIn == y.BuiltIn &&
                x.Type == y.Type &&
                x.RefDescription == y.RefDescription &&
                x.ExternalId == y.ExternalId;
        }
        public static bool operator !=(Group x, Group y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return (Id, RefDescription, BuiltIn, Type, RefDescription, ExternalId).GetHashCode();
        }
    }
}
