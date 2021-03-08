using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class ApiOperation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("method")]
        public string Method { get; set; }
        [JsonPropertyName("urlTemplate")]
        public string UrlTemplate { get; set; }
        [JsonPropertyName("templateParameters")]
        public List<ApiOperationTemplateParameter> TemplateParameters { get; set; }

        [JsonPropertyName("$ref-description")]
        public string RefDescription { get; set; }
        [JsonPropertyName("request")]
        public ApiOperationRequest Request { get; set; }
        [JsonPropertyName("responses")]
        public List<ApiOperationResponse> Responses { get; set; }
        [JsonPropertyName("policies")]
        public object Policies { get; set; }

        [JsonPropertyName("$ref-policy")]
        public string RefPolicy { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is ApiOperation && this == (ApiOperation)obj;
        }
        public static bool operator ==(ApiOperation x, ApiOperation y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                x.Name == y.Name &&
                x.Method == y.Method &&
                x.UrlTemplate == y.UrlTemplate &&
                ApimUtils.ListsAreEqual(x.TemplateParameters, y.TemplateParameters) &&
                x.RefDescription == y.RefDescription &&
                x.Request == y.Request &&
                ApimUtils.ListsAreEqual(x.Responses, y.Responses) &&
                x.Policies == y.Policies &&
                x.RefPolicy == y.RefPolicy;
        }
        public static bool operator !=(ApiOperation x, ApiOperation y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return (Id, Name, Method, UrlTemplate, TemplateParameters, RefDescription, Request, Responses, Policies, RefPolicy).GetHashCode();
        }
    }
}
