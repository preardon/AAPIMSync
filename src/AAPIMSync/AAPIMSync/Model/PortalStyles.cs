using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class PortalStyles : ConfigurationBase
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("$ref-Production")]
        public string RefProduction { get; set; }

        [JsonPropertyName("$ref-Template")]
        public string RefTemplate { get; set; }

        [JsonPropertyName("$ref-Preview")]
        public string RefPreview { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is PortalStyles && this == (PortalStyles)obj;
        }
        public static bool operator ==(PortalStyles x, PortalStyles y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;

            return x.Name == y.Name &&
                x.RefProduction == y.RefProduction &&
                x.RefTemplate == y.RefTemplate &&
                x.RefPreview == y.RefPreview;
        }
        public static bool operator !=(PortalStyles x, PortalStyles y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Name, RefProduction, RefTemplate, RefPreview).GetHashCode();
        }
    }
}
