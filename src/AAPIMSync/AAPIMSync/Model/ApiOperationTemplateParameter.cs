using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class ApiOperationTemplateParameter
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("required")]
        public bool Required { get; set; }

        //Must Confirm Type
        [JsonPropertyName("values")]
        public List<string> Values { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is ApiOperationTemplateParameter && this == (ApiOperationTemplateParameter)obj;
        }
        public static bool operator ==(ApiOperationTemplateParameter x, ApiOperationTemplateParameter y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            
            return x.Name == y.Name && 
                x.Description == y.Description &&
                x.Type == y.Type &&
                x.Required == y.Required &&
                ApimUtils.ListsAreEqual(x.Values, y.Values);
        }
        public static bool operator !=(ApiOperationTemplateParameter x, ApiOperationTemplateParameter y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Name, Description, Type, Required, Values).GetHashCode();
        }
    }
}
