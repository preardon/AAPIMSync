using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class Api : ConfigurationBase
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("apiRevision")]
        public string ApiRevision { get; set; }

        [JsonPropertyName("$ref-description")]
        public string RefDescription { get; set; }
        [JsonPropertyName("subscriptionRequired")]
        public bool SubscriptionRequired { get; set; }
        [JsonPropertyName("serviceUrl")]
        public string ServiceUrl { get; set; }
        [JsonPropertyName("path")]
        public string Path { get; set; }
        [JsonPropertyName("protocols")]
        public List<string> Protocols { get; set; }
        [JsonPropertyName("authenticationSettings")]
        public AuthenticationSettings AuthenticationSettings { get; set; }
        [JsonPropertyName("subscriptionKeyParameterNames")]
        public SubscriptionKeyParameterNames SubscriptionKeyParameterNames { get; set; }
        [JsonPropertyName("isCurrent")]
        public bool IsCurrent { get; set; }
        [JsonPropertyName("apiRevisionDescription")]
        public string ApiRevisionDescription { get; set; }
        [JsonPropertyName("apiVersion")]
        public string ApiVersion { get; set; }
        [JsonPropertyName("apiVersionSetId")]
        public string ApiVersionSetId { get; set; }
        [JsonPropertyName("operations")]
        public List<ApiOperation> Operations { get; set; }
        [JsonPropertyName("apiSchemas")]
        public List<ApiSchema> ApiSchemas { get; set; }
        [JsonPropertyName("$ref-policy")]
        public string RefPolicy { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is Api && this == (Api)obj;
        }
        public static bool operator ==(Api x, Api y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                x.Name == y.Name &&
                x.ApiRevision == y.ApiRevision &&
                x.RefDescription == y.RefDescription &&
                x.SubscriptionRequired == y.SubscriptionRequired &&
                x.ServiceUrl == y.ServiceUrl &&
                x.Path == y.Path &&
                ApimUtils.ListsAreEqual(x.Protocols, y.Protocols) &&
                x.AuthenticationSettings == y.AuthenticationSettings &&
                x.SubscriptionKeyParameterNames == y.SubscriptionKeyParameterNames &&
                x.IsCurrent == y.IsCurrent &&
                x.ApiRevisionDescription == y.ApiRevisionDescription &&
                x.ApiVersion == y.ApiVersion &&
                x.ApiVersionSetId == y.ApiVersionSetId &&
                ApimUtils.ListsAreEqual(x.Operations, y.Operations) &&
                ApimUtils.ListsAreEqual(x.ApiSchemas, y.ApiSchemas) &&
                x.RefPolicy == y.RefPolicy;
        }
        public static bool operator !=(Api x, Api y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return (Id, Name, ApiRevision, RefDescription, SubscriptionRequired, ServiceUrl, Path, Protocols, AuthenticationSettings, SubscriptionKeyParameterNames, IsCurrent, ApiRevisionDescription, ApiVersion, ApiVersionSetId, Operations, ApiSchemas, RefPolicy).GetHashCode();
        }
    }
}
