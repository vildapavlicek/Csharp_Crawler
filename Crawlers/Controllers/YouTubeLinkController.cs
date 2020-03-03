using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crawlers.Models;
using Crawlers.Crawler;

namespace Crawlers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YouTubeLinkController : ControllerBase
    {
        private readonly YouTubeLinkContext _context;

        public YouTubeLinkController(YouTubeLinkContext context)
        {
            _context = context;
        }

        // GET: api/YouTubeLink
        [HttpGet]
        public async Task<ActionResult<IEnumerable<YouTubeLink>>> GetLinks()
        {
            return await _context.Links.ToListAsync();
        }

        // GET: api/YouTubeLink/5
        [HttpGet("{id}")]
        public async Task<ActionResult<YouTubeLink>> GetYouTubeLink(int id)
        {
            var youTubeLink = await _context.Links.FindAsync(id);

            if (youTubeLink == null)
            {
                return NotFound();
            }

            return youTubeLink;
        }

        // PUT: api/YouTubeLink/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutYouTubeLink(int id, YouTubeLink youTubeLink)
        {
            if (id != youTubeLink.Id)
            {
                return BadRequest();
            }

            _context.Entry(youTubeLink).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YouTubeLinkExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/YouTubeLink
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<YouTubeLink>> PostYouTubeLink(YouTubeLink youTubeLink)
        {
            // _context.Links.Add(youTubeLink);
            // await _context.SaveChangesAsync();

            CrawlerManager.AddToCrawl(youTubeLink);

            return CreatedAtAction(nameof(GetYouTubeLink), new { id = youTubeLink.Id }, youTubeLink);
        }

        // DELETE: api/YouTubeLink/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<YouTubeLink>> DeleteYouTubeLink(int id)
        {
            var youTubeLink = await _context.Links.FindAsync(id);
            if (youTubeLink == null)
            {
                return NotFound();
            }

            _context.Links.Remove(youTubeLink);
            await _context.SaveChangesAsync();

            return youTubeLink;
        }

        private bool YouTubeLinkExists(int id)
        {
            return _context.Links.Any(e => e.Id == id);
        }
    }
}
