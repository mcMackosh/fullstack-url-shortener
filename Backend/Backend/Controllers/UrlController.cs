using Backend.DTO;
using Backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UrlController(IUrlService urlService, IHttpContextAccessor httpContextAccessor)
    {
        _urlService = urlService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}/";

        var urls = await _urlService.GetAllUrlsAsync(baseUrl);
        return Ok(urls);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}/";
        var url = await _urlService.GetUrlByIdAsync(id, baseUrl);
        return Ok(url);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateUrlRequest request)
    {
        var userId = GetUserId();
        var createdUrl = await _urlService.CreateUrlAsync(request, userId);
        return CreatedAtAction(nameof(GetById), new { id = createdUrl.Id }, createdUrl);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();
        var isAdmin = User.IsInRole("ADMIN");

        await _urlService.DeleteUrlAsync(id, userId, isAdmin);
        return NoContent();
    }

    private Guid GetUserId()
    {
        var userIdStr = User.FindFirstValue("UserId");
        if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
            throw new UnauthorizedAccessException("Invalid or missing user ID.");
        return userId;
    }
}
