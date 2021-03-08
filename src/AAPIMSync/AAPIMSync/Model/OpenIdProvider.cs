using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class OpenIdProvider : ConfigurationBase
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("metadataEndpoint")]
        public string MetadataEndpoint { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is OpenIdProvider && this == (OpenIdProvider)obj;
        }
        public static bool operator ==(OpenIdProvider x, OpenIdProvider y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                x.Name == y.Name &&
                x.Description == y.Description &&
                x.MetadataEndpoint == y.MetadataEndpoint;
        }
        public static bool operator !=(OpenIdProvider x, OpenIdProvider y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, Name, Description, MetadataEndpoint).GetHashCode();
        }
    }
}
