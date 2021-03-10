using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class ApiOperationResponse
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }
        [JsonPropertyName("representations")]
        public List<Representation> Representations { get; set; }
        [JsonPropertyName("headers")]
        public List<object> Headers { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("$ref-description")]
        public string RefDescription { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is ApiOperationResponse && this == (ApiOperationResponse)obj;
        }
        public static bool operator ==(ApiOperationResponse x, ApiOperationResponse y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.StatusCode == y.StatusCode &&
                ApimUtils.ListsAreEqual(x.Representations, y.Representations) &&
                ApimUtils.ListsAreEqual(x.Headers, y.Headers) &&
                x.RefDescription == y.RefDescription;
        }
        public static bool operator !=(ApiOperationResponse x, ApiOperationResponse y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return (StatusCode, Representations, Headers, RefDescription).GetHashCode();
        }
    }
}
