using Microsoft.EntityFrameworkCore;

namespace Crawlers.Models
{
    public class YouTubeLinkContext :DbContext
    {
        public DbSet<YouTubeLink> Links { get; set; }
        public YouTubeLinkContext(DbContextOptions<YouTubeLinkContext> options) : base(options)
        {
        }
    }
}
