using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class SubscriptionKeyParameterNames
    {
        [JsonPropertyName("header")]
        public string Header { get; set; }
        [JsonPropertyName("query")]
        public string Query { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is SubscriptionKeyParameterNames && this == (SubscriptionKeyParameterNames)obj;
        }
        public static bool operator ==(SubscriptionKeyParameterNames x, SubscriptionKeyParameterNames y)
        {
            return x.Header == y.Header && x.Query == y.Query;
        }
        public static bool operator !=(SubscriptionKeyParameterNames x, SubscriptionKeyParameterNames y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Header, Query).GetHashCode();
        }
    }
}
