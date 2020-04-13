using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler.Core
{
    public class HttpProvider : IDataProvider, IDisposable
    {
        private readonly HttpClient Client = new HttpClient();

        public async Task<string> GetFrom(string url)
        {
            var response = await Client.GetAsync(new Uri(url));
            response.EnsureSuccessStatusCode();
            string responceBody = await response.Content.ReadAsStringAsync();
            return responceBody;
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
