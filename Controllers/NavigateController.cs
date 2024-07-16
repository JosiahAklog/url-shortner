using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Controllers
{
    [Route("navigate")]
    [ApiController]
    public class NavigateController : ControllerBase
    {
        [HttpGet("{shortUrlCode}")]
        public IActionResult NavigateToLongUrl(string shortUrlCode)
        {
            if (ShortUrlsController.urlStore.TryGetValue(shortUrlCode, out var longUrl))
            {
                ShortUrlsController.urlHits[shortUrlCode]++;
                return Redirect(longUrl);
            }
            return NotFound("Short URL not found.");
        }
    }
}
