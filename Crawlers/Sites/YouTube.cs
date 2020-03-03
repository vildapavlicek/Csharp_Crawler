using System;
using Crawlers.Crawler;
using HtmlAgilityPack;
using Crawlers.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawlers.Sites
{
    public class YouTube : ICrawable
    {
        private const string notFound = "NotFound";
        private const string Domain = "youtube.com/";
        private const string HttpProtocol = "http://";

        private int CurrentIteration { get; set; }
        private int MaxIterations { get; set; }
        public string Path { get; private set; }

        public YouTube(string path)
        {
            if (path == "")
            {
                throw new ArgumentException("Path cannot be empty");
            }

            Path = path;
            CurrentIteration = 0;
            MaxIterations = 10;
        }

        public YouTube(string path, int maxIterations) : this(path)
        {
            MaxIterations = maxIterations;
        }

        public async Task Crawl(HttpClient client)
        {
            while (!IsFinished())
            {
                string URL = HttpProtocol + Domain + Path;

                var GETTask = client.GetStringAsync(URL);
                var response = await GETTask;

                //var nextVideoLink = ParseLinks(response).Update(this).Store;
                var link = ParseLinks(response);
                link.Store();
                Path = link.Path;

                CurrentIteration++;
            }
        }

        public bool IsFinished()
        {
            return CurrentIteration > MaxIterations;
        }


        public YouTubeLink ParseLinks(string data)
        {
            var page = new HtmlDocument();
            page.LoadHtml(data);

            var sidebarNode = page.DocumentNode.SelectSingleNode("//div[contains(concat(' ',normalize-space(@class),' '),'watch-sidebar-body')]");

            if (sidebarNode is null)
            {
                throw new NodeNotFoundException();
            }

            var tempDoc = new HtmlDocument();
            tempDoc.LoadHtml(sidebarNode.InnerHtml);

            var nextVideoNode = tempDoc.DocumentNode.SelectSingleNode("//ul/li/div");


            string href = "";
            string title = "";

            foreach (HtmlNode n in nextVideoNode.ChildNodes)
            {
                if (n.Name == "a" && n.HasAttributes)
                {
                    href = n.GetAttributeValue("href", notFound);
                    title = n.GetAttributeValue("title", notFound);
                }
            }

            if (href == notFound || title == notFound)
            {
                throw new NodeAttributeNotFoundException();
            }

            var now = DateTime.Now;
            Console.WriteLine($"{CurrentIteration}. | {now} | PARSE | Title: '{title}' | Path: '{href}'");

            return new YouTubeLink
            {
                Path = href,
                Site = "YouTube",
                Description = title,
                Time = DateTime.Now,
            };
        }

        public void Update(string path) => Path = path;
    }
}