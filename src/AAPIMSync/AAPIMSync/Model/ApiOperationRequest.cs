using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class ApiOperationRequest
    {
        [JsonPropertyName("queryParameters")]
        public List<QueryParameter> QueryParameters { get; set; }
        [JsonPropertyName("headers")]
        public List<Headers> Headers { get; set; }
        [JsonPropertyName("representations")]
        public List<Representation> Representations { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is ApiOperationRequest && this == (ApiOperationRequest)obj;
        }
        public static bool operator ==(ApiOperationRequest x, ApiOperationRequest y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return ApimUtils.ListsAreEqual(x.QueryParameters, y.QueryParameters) &&
                ApimUtils.ListsAreEqual(x.Headers, y.Headers) &&
                ApimUtils.ListsAreEqual(x.Representations, y.Representations);
        }
        public static bool operator !=(ApiOperationRequest x, ApiOperationRequest y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return (QueryParameters, Headers, Representations).GetHashCode();
        }
    }
}
