using System.Net.Http;
using System.Threading.Tasks;

namespace Crawlers.Crawler
{
    public interface ICrawable
    {
        Task Crawl(HttpClient client);
    }
}
