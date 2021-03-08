using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class ApiVersionSet : ConfigurationBase
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("versioningScheme")]
        public string VersioningScheme { get; set; }
        [JsonPropertyName("versionQueryName")]
        public string VersionQueryName { get; set; }
        [JsonPropertyName("versionHeaderName")]
        public string VersionHeaderName { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is ApiVersionSet && this == (ApiVersionSet)obj;
        }
        public static bool operator ==(ApiVersionSet x, ApiVersionSet y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            
            return x.Id == y.Id &&
                x.Name == y.Name &&
                x.Description == y.Description &&
                x.VersioningScheme == y.VersioningScheme &&
                x.VersionQueryName == y.VersionQueryName &&
                x.VersionHeaderName == y.VersionHeaderName;
        }
        public static bool operator !=(ApiVersionSet x, ApiVersionSet y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, Name, Description, VersioningScheme, VersionQueryName, VersionHeaderName).GetHashCode();
        }
    }
}
