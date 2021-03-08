using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class AuthenticationSettingsOpenId
    {
        [JsonPropertyName("openidProviderId")]
        public string OpenidProviderId { get; set; }

        //This Type is a guess
        [JsonPropertyName("bearerTokenSendingMethods")]
        public string BearerTokenSendingMethods { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is AuthenticationSettingsOpenId && this == (AuthenticationSettingsOpenId)obj;
        }
        public static bool operator ==(AuthenticationSettingsOpenId x, AuthenticationSettingsOpenId y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            return x.OpenidProviderId == y.OpenidProviderId && x.BearerTokenSendingMethods == y.BearerTokenSendingMethods;
        }
        public static bool operator !=(AuthenticationSettingsOpenId x, AuthenticationSettingsOpenId y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(OpenidProviderId, BearerTokenSendingMethods).GetHashCode();
        }
    }
}
