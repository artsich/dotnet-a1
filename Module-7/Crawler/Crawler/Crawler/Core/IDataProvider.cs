using System;
using System.Threading.Tasks;

namespace Crawler.Core
{
    public interface IDataProvider
    {
        Task<string> GetFrom(string url);
    }
}
