using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    public class RedirectController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public RedirectController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            var url = await _urlService.GetByShortCodeAsync(shortCode);
            if (url == null)
                return NotFound("Short URL not found.");

            await _urlService.IncrementClickCountAsync(url.Id);
            return Redirect(url.OriginalUrl);
        }
    }
}
