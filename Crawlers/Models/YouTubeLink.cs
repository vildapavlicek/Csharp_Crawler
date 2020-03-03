using System;
using Crawlers.Sites;
using Microsoft.EntityFrameworkCore;


namespace Crawlers.Models
{
    public class YouTubeLink
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Site { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        private readonly DbContextOptions<YouTubeLinkContext> options = new DbContextOptionsBuilder<YouTubeLinkContext>().UseNpgsql("Host=localhost;Database=crawler;Username=user;Password=password").Options;
        public async void Store()
        {
            using var db = new YouTubeLinkContext(options);
            db.Add(this);
            await db.SaveChangesAsync();
        }

        public YouTubeLink Update(YouTube yt)
        {
            yt.Update(Path);
            return this;
        }
    }
}