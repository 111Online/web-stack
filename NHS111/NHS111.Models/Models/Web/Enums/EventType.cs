using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NHS111.Models.Models.Web.Enums
{
    // If this enum is updated, the Logging API needs the same change made to it

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventType
    {
        None,
        Browser,
        BrowserVersion,
        OperatingSystem,
        DeviceType
    }
}