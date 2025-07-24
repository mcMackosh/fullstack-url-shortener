using Backend.Database;
using Backend.DTO;
using Backend.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;


public interface IUrlService
{
    Task<IEnumerable<UrlListItemResponceDto>> GetAllUrlsAsync(string currentUrl);
    Task<ShortUrlResponseDto> GetUrlByIdAsync(Guid id, string baseUrl);
    Task<ShortUrl> CreateUrlAsync(CreateUrlRequest request, Guid userId);
    Task DeleteUrlAsync(Guid id, Guid userId, bool isAdmin);
    Task<ShortUrl?> GetByShortCodeAsync(string shortCode);
    Task IncrementClickCountAsync(Guid id);
}