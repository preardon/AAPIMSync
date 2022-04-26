using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class TemplateParameter
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is TemplateParameter && this == (TemplateParameter)obj;
        }
        public static bool operator ==(TemplateParameter x, TemplateParameter y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Name == y.Name &&
                x.Title == y.Title &&
                x.Description == y.Description;
        }
        public static bool operator !=(TemplateParameter x, TemplateParameter y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Name, Title, Description).GetHashCode();
        }
    }
}
