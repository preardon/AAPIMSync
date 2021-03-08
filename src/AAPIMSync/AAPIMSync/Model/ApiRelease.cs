using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class ApiRelease : ConfigurationBase
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("apiId")]
        public string ApiId { get; set; }
        [JsonPropertyName("createdDateTime")]
        public DateTime CreatedDateTime { get; set; }
        [JsonPropertyName("updatedDateTime")]
        public DateTime UpdatedDateTime { get; set; }
        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is ApiRelease && this == (ApiRelease)obj;
        }
        public static bool operator ==(ApiRelease x, ApiRelease y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                x.ApiId == y.ApiId &&
                x.CreatedDateTime == y.CreatedDateTime &&
                x.UpdatedDateTime == y.UpdatedDateTime &&
                x.Notes == y.Notes;
        }
        public static bool operator !=(ApiRelease x, ApiRelease y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, ApiId, CreatedDateTime, UpdatedDateTime, Notes).GetHashCode();
        }

    }
}
