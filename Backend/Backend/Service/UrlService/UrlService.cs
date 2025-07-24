using Backend.Config;
using Backend.Database;
using Backend.DTO;
using Backend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public class UrlService : IUrlService
{
    private readonly AppDbContext _context;
    private readonly ShortUrlSettings _settings;

    public UrlService(AppDbContext context, IOptions<ShortUrlSettings> options)
    {
        _context = context;
        _settings = options.Value;
    }

    public async Task<IEnumerable<UrlListItemResponceDto>> GetAllUrlsAsync(string currentUrl)
    {
        return await _context.ShortUrls
            .Select(u => new UrlListItemResponceDto
            {
                Id = u.Id,
                OriginalUrl = u.OriginalUrl,
                ShortUrl = currentUrl + u.ShortCode,
                CreatedBy = u.CreatedById
            })
            .ToListAsync();
    }

    public async Task<ShortUrlResponseDto> GetUrlByIdAsync(Guid id, string currentUrl)
    {
        var url = await _context.ShortUrls
            .Include(u => u.CreatedBy)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (url == null)
            throw new KeyNotFoundException("URL not found.");

        return new ShortUrlResponseDto
        {
            Id = url.Id,
            OriginalUrl = url.OriginalUrl,
            ShortUrl = $"{currentUrl}{url.ShortCode}",
            CreatedAt = url.CreatedAt,
            Clicks = url.Clicks,
            CreatedBy = url.CreatedBy?.Username ?? "Unknown"
        };
    }

    public async Task<ShortUrl> CreateUrlAsync(CreateUrlRequest request, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(request.OriginalUrl))
            throw new ArgumentException("Original URL is required.");

        bool alreadyExists = await _context.ShortUrls.AnyAsync(u => u.OriginalUrl == request.OriginalUrl);
        if (alreadyExists)
            throw new InvalidOperationException("This URL already exists.");

        var shortCode = await GenerateUniqueShortCodeAsync();

        var newUrl = new ShortUrl
        {
            Id = Guid.NewGuid(),
            OriginalUrl = request.OriginalUrl,
            ShortCode = shortCode,
            CreatedById = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.ShortUrls.Add(newUrl);
        await _context.SaveChangesAsync();

        return newUrl;
    }

    public async Task DeleteUrlAsync(Guid id, Guid userId, bool isAdmin)
    {
        var url = await _context.ShortUrls.FindAsync(id);

        if (url == null)
            throw new KeyNotFoundException("URL not found.");

        if (!isAdmin && url.CreatedById != userId)
            throw new UnauthorizedAccessException("You do not have permission to delete this URL.");

        _context.ShortUrls.Remove(url);
        await _context.SaveChangesAsync();
    }

    public async Task<ShortUrl?> GetByShortCodeAsync(string shortCode)
    {
        return await _context.ShortUrls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
    }

    public async Task IncrementClickCountAsync(Guid id)
    {
        var url = await _context.ShortUrls.FindAsync(id);
        if (url == null)
            throw new KeyNotFoundException("URL not found.");

        url.Clicks++;
        await _context.SaveChangesAsync();
    }

    private async Task<string> GenerateUniqueShortCodeAsync()
    {
        string code;
        do
        {
            code = Guid.NewGuid().ToString("N")[..8];
        }
        while (await _context.ShortUrls.AnyAsync(u => u.ShortCode == code));

        return code;
    }
}
