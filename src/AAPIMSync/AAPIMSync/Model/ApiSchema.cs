using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class ApiSchema
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("$ref-documentValue")]
        public string RefDocumentValue { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is ApiSchema && this == (ApiSchema)obj;
        }
        public static bool operator ==(ApiSchema x, ApiSchema y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            
            return x.Id == y.Id &&
                x.ContentType == y.ContentType &&
                x.RefDocumentValue == y.RefDocumentValue;
        }
        public static bool operator !=(ApiSchema x, ApiSchema y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, ContentType, RefDocumentValue).GetHashCode();
        }
    }
}
