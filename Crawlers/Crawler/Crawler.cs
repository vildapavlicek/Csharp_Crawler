using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawlers.Crawler
{

    public enum Status
    {
        Waiting,
        Working,
        Done,
        Error,
    }

    public class Crawler
    {
        public Status Status;
        private readonly HttpClient Client;


        public Crawler(HttpClient client)
        {
            Status = Status.Waiting;
            Client = client;
        }

        public async Task RegisterSite(ICrawable site)
        {
                await site.Crawl(Client);
                Status = Status.Working;
        }
    }
}
