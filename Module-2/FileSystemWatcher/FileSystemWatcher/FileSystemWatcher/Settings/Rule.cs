using Newtonsoft.Json;

namespace FileSystemWatcher
{
    [JsonObject("ModifyRule")]
    public class ModifyRule
    {
        public bool IsOrderNumberAdd { get; }

        public bool IsDateAdded { get; set; }
    }

    [JsonObject("Rule")]
    public class Rule
    {
        [JsonProperty("Default path")]
        public string DefaultPath { get; }

        [JsonProperty("Destination path")]
        public string DestinationPath { get; }

        [JsonProperty("Pattern")]
        public string Pattern { get; }

        [JsonProperty("Modify Rule")]
        public ModifyRule ModifyRule { get; }
    }
}
