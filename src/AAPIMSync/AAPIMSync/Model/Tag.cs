using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class Tag : ConfigurationBase
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("apiIds")]
        public List<string> ApiIds { get; set; }
        [JsonPropertyName("productIds")]
        public List<string> ProductIds { get; set; }
        [JsonPropertyName("descriptions")]
        public List<object> Descriptions { get; set; }
        [JsonPropertyName("operationIds")]
        public List<string> OperationIds { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is Tag && this == (Tag)obj;
        }
        public static bool operator ==(Tag x, Tag y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Id == y.Id &&
                x.Name == y.Name &&
                x.ApiIds == y.ApiIds &&
                x.ProductIds == y.ProductIds &&
                ApimUtils.ListsAreEqual(x.Descriptions, y.Descriptions) &&
                x.OperationIds == y.OperationIds;
        }
        public static bool operator !=(Tag x, Tag y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Id, Name, ApiIds, ProductIds, Descriptions, OperationIds).GetHashCode();
        }
    }
}
