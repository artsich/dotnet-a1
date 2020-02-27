
using Newtonsoft.Json;

namespace FileSystemWatcher.Settings
{
    [JsonObject("AppSettings")]
    public class Setting
    {
        [JsonProperty("Localization")]
        public string Localization { get; set; }
    }
}
