using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class Template
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("$ref-body")]
        public string RefBody { get; set; }
        [JsonPropertyName("itle")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }
        [JsonPropertyName("parameters")]
        public List<TemplateParameter> Parameters { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is Template && this == (Template)obj;
        }
        public static bool operator ==(Template x, Template y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Name == y.Name &&
                x.RefBody == y.RefBody &&
                x.Title == y.Title &&
                x.Description == y.Description &&
                x.IsDefault == y.IsDefault &&
                ApimUtils.ListsAreEqual(x.Parameters, y.Parameters);
        }
        public static bool operator !=(Template x, Template y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Name, RefBody, Title, Description, IsDefault, Parameters).GetHashCode();
        }
    }
}
