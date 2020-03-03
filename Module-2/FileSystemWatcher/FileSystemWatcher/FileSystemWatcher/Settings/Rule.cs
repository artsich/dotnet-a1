using Newtonsoft.Json;

namespace FileSystemWatcher
{
	[JsonObject("Rule")]
	public class Rule
    {
        public string DestinationPath { get; set; }

        public string Pattern { get; set; }

        public ModifyRule ModifyRule { get; set; }
	}
}
