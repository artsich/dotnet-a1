using Newtonsoft.Json;
using System.Collections.Generic;

namespace FileSystemWatcher.Settings
{
    [JsonObject("AppSettings")]
    public class Setting
    {
        [JsonProperty("Localization")]
        public string Localization { get; set; }

        [JsonProperty("Rules")]
        public IList<Rule> Rules { get; set; }
    }
}