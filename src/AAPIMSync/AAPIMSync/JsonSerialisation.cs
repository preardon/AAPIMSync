using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace PReardon.AAPIMSync
{
    public static class JsonSerialisation
    {
        public static JsonSerializerOptions Options { get; }

        static JsonSerialisation()
        {
            var encoderSettings = new TextEncoderSettings();
            //encoderSettings.AllowCharacter('u0027');
            //encoderSettings.AllowCharacter('u0022');
            encoderSettings.AllowRange(UnicodeRanges.BasicLatin);

            Options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping //JavaScriptEncoder.Create(encoderSettings)
            };
        }
        public static string Serialise(object obj)
        {
            var json = JsonSerializer.Serialize(obj, JsonSerialisation.Options);
            json = json.Replace("u0027", "'");
            json = json.Replace("u0022", "\\\"");

            return json;
        }
    }
}
