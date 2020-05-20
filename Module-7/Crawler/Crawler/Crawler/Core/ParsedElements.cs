using System.IO;

namespace Crawler.Core
{
    public class ParsedElement
    {

        public string BaseFolder { get; set; }

        public string BaseName { get; set; }

        public string SavePath => Path.Combine(BaseFolder, BaseName);

        public string Content { get; set; }
    }
}
