using Backend.Database;
using Backend.DTO;
using Backend.Model;
using Microsoft.EntityFrameworkCore;

public class AboutService : IAboutService
{
    private readonly AppDbContext _context;
    private readonly string _defaultAboutPath;
    public AboutService(AppDbContext context)
    {
        _context = context;
        _defaultAboutPath = Path.Combine(AppContext.BaseDirectory, "DefaultAbout.txt");
    }

    public async Task<AboutResponseDto> GetAsync()
    {
        var about = await _context.AboutSection.FirstOrDefaultAsync();

        if (about == null)
        {
            string defaultText = await ReadDefaultAboutTextAsync();

            about = new About
            {
                Description = defaultText,
                LastUpdated = DateTime.UtcNow
            };

            _context.AboutSection.Add(about);
            await _context.SaveChangesAsync();
        }

        return new AboutResponseDto
        {
            Description = about.Description,
            LastUpdated = about.LastUpdated
        };
    }

    public async Task UpdateAsync(AboutDto dto)
    {
        var entity = await _context.AboutSection.FirstOrDefaultAsync();

        if (entity == null)
        {
            entity = new About
            {
                Description = dto.Description,
                LastUpdated = DateTime.UtcNow
            };
            _context.AboutSection.Add(entity);
        }
        else
        {
            entity.Description = dto.Description;
            entity.LastUpdated = DateTime.UtcNow;
            _context.AboutSection.Update(entity);
        }

        await _context.SaveChangesAsync();
    }

    private async Task<string> ReadDefaultAboutTextAsync()
    {

        
        if (!File.Exists(_defaultAboutPath))
        {
            return "Default about text is missing.";
        }

        return await File.ReadAllTextAsync(_defaultAboutPath);
    }
}
