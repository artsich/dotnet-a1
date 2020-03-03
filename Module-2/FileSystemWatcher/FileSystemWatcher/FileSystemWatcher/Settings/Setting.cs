using Newtonsoft.Json;
using System.Collections.Generic;

namespace FileSystemWatcher.Settings
{
    [JsonObject("AppSettings")]
    public class Setting
    {
		public string DefaultPath { get; set; }

		public string Localization { get; set; }

		public string[] ListeningFolders { get; set; }

        public Rule[] Rules { get; set; }
    }
}