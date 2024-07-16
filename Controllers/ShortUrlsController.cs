using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace UrlShortener.Controllers
{
    [Route("shortUrls")]
    [ApiController]
    public class ShortUrlsController : ControllerBase
    {
        // Change access modifier to internal
        internal static readonly Dictionary<string, string> urlStore = new Dictionary<string, string>();
        internal static readonly Dictionary<string, int> urlHits = new Dictionary<string, int>();

        [HttpPut("{id}")]
        public ActionResult<string> CreateOrUpdateShortUrl(string id, [FromBody] UrlRequest request)
        {
            var shortUrl = $"https://urllshorten.azurewebsites.net/navigate/{id}";
            urlStore[id] = request.LongUrl;
            urlHits[id] = 0; // Initialize hits counter
            return Ok(shortUrl);
        }

        [HttpGet("{id}")]
        public ActionResult<string> GetShortUrl(string id)
        {
            if (urlStore.TryGetValue(id, out var longUrl))
            {
                var shortUrl = $"https://urllshorten.azurewebsites.net/navigate/{id}";
                return Ok(shortUrl);
            }
            return NotFound("Short URL not found.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteShortUrl(string id)
        {
            if (urlStore.Remove(id))
            {
                urlHits.Remove(id);
                return NoContent();
            }
            return NotFound("Short URL not found.");
        }

        [HttpGet("{id}/hits")]
        public ActionResult<int> GetHits(string id)
        {
            if (urlHits.TryGetValue(id, out var hits))
            {
                return Ok(hits);
            }
            return NotFound("Short URL not found.");
        }

        public class UrlRequest
        {
            public string LongUrl { get; set; }
        }
    }
}
