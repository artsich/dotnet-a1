using System.IO;
using System.Threading.Tasks;

namespace Crawler.Core
{
    public class FileSystemProvider : IDataProvider
    {
        public async Task<string> GetFrom(string url)
        {
            return await File.ReadAllTextAsync(url);
        }
    }
}
