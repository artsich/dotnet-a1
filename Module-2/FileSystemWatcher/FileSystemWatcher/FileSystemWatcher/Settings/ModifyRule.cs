using Newtonsoft.Json;

namespace FileSystemWatcher
{
	[JsonObject("ModifyRule")]
	public class ModifyRule
    {
        public bool IsOrderNumberAdd { get; set; }

        public bool IsDateAdded { get; set; }

        public string DateFormat { get; set; }
    }
}
