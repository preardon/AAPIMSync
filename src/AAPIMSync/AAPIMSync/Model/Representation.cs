using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class Representation
    {
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("$ref-sample")]
        public string RefSample { get; set; }
        [JsonPropertyName("schemaId")]
        public string SchemaId { get; set; }
        [JsonPropertyName("typeName")]
        public string TypeName { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is Representation && this == (Representation)obj;
        }
        public static bool operator ==(Representation x, Representation y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.ContentType == y.ContentType &&
                x.RefSample == y.RefSample &&
                x.SchemaId == y.SchemaId &&
                x.TypeName == y.TypeName;
        }
        public static bool operator !=(Representation x, Representation y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return (ContentType, RefSample, SchemaId, TypeName).GetHashCode();
        }
    }
}
