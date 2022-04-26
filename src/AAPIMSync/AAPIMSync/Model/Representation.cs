using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class Representation
    {
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("$ref-sample")]
        public string RefSample { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("schemaId")]
        public string SchemaId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("typeName")]
        public string TypeName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("generatedSample")]
        public string GeneratedSample { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("sample")]
        public string Sample { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("examples")]
        public Dictionary<string, Reference> Examples { get; set; }

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
                x.TypeName == y.TypeName &&
                x.GeneratedSample == y.GeneratedSample &&
                x.Sample == y.Sample &&
                x.Examples == y.Examples;
        }
        public static bool operator !=(Representation x, Representation y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return (ContentType, RefSample, SchemaId, TypeName, GeneratedSample, Sample, Examples).GetHashCode();
        }
    }
}
