using System.Net.Http;
using Crawlers.Sites;
using Crawlers.Models;

namespace Crawlers.Crawler
{
    public static class CrawlerManager
    {
        private static readonly Crawler Crawler = new Crawler(new HttpClient());

        public static void AddToCrawl(YouTubeLink link)
        {
            var path = "watch?v=" + link.Path;
            var yt = new YouTube(path);
            Crawler.RegisterSite(yt);
        }
    }
}
