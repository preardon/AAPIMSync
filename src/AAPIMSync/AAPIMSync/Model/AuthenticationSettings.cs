using System;
using System.Text.Json.Serialization;

namespace PReardon.AAPIMSync.Model
{
    public class AuthenticationSettings
    {
        [JsonPropertyName("oAuth2")]
        public string OAuth2 { get; set; }
        [JsonPropertyName("openid")]
        public AuthenticationSettingsOpenId Openid { get; set; }

        public override bool Equals(Object obj)
        {
            return obj is AuthenticationSettings && this == (AuthenticationSettings)obj;
        }
        public static bool operator ==(AuthenticationSettings x, AuthenticationSettings y)
        {
            if (x is null && y is null) return true;
            if (x is null || y is null) return false;
            return x.OAuth2 == y.OAuth2 && x.Openid == y.Openid;
        }
        public static bool operator !=(AuthenticationSettings x, AuthenticationSettings y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(OAuth2, Openid).GetHashCode();
        }
    }
}
