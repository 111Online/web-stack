using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NHS111.Models.Models.Web.Enums
{
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