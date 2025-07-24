using Backend.Controllers.Attributes;
using Backend.DTO;
using Backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class AboutController : ControllerBase
{
    private readonly IAboutService _aboutService;

    public AboutController(IAboutService aboutService)
    {
        _aboutService = aboutService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        var result = await _aboutService.GetAsync();
        return Ok(result);
    }

    [HttpPut]
    [AdminOnly]
    public async Task<IActionResult> Update([FromBody] AboutDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Description))
            return BadRequest(new { message = "Content cannot be empty." });

        await _aboutService.UpdateAsync(dto);
        return NoContent();
    }
}
