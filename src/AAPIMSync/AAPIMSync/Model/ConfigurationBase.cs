using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class ConfigurationBase
    {
        [JsonPropertyName("__GitModuleVersion")]
        public int GitModuleVersion { get; set; }
    }
}
