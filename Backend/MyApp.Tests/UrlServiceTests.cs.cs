using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Backend.Config;
using Backend.Database;
using Backend.DTO;
using Backend.Model;

namespace Backend.Tests
{
    public class UrlServiceTests
    {
        private readonly UrlService _service;
        private readonly AppDbContext _context;
        private readonly Mock<IOptions<ShortUrlSettings>> _mockSettings;

        public UrlServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _mockSettings = new Mock<IOptions<ShortUrlSettings>>();
            _mockSettings.Setup(s => s.Value).Returns(new ShortUrlSettings
            {
                BaseUrl = "http://localhost/"
            });

            _service = new UrlService(_context, _mockSettings.Object);
        }

        [Fact]
        public async Task CreateUrlAsync_ValidUrl_SavesToDatabase()
        {
            var request = new CreateUrlRequest { OriginalUrl = "https://example.com" };
            var userId = Guid.NewGuid();

            var result = await _service.CreateUrlAsync(request, userId);

            var urlInDb = await _context.ShortUrls.FirstOrDefaultAsync();
            Assert.NotNull(urlInDb);
            Assert.Equal(request.OriginalUrl, urlInDb!.OriginalUrl);
            Assert.Equal(userId, urlInDb.CreatedById);
        }

        [Fact]
        public async Task CreateUrlAsync_Duplicate_ThrowsException()
        {
            var url = new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://example.com",
                ShortCode = "abc123",
                CreatedById = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };
            _context.ShortUrls.Add(url);
            await _context.SaveChangesAsync();

            var request = new CreateUrlRequest { OriginalUrl = "https://example.com" };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateUrlAsync(request, Guid.NewGuid()));
        }

        [Fact]
        public async Task GetAllUrlsAsync_ReturnsAllUrls()
        {
            var url = new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://test.com",
                ShortCode = "123abc",
                CreatedById = Guid.NewGuid()
            };
            _context.ShortUrls.Add(url);
            await _context.SaveChangesAsync();

            var baseUrl = "http://localhost/";

            var result = (await _service.GetAllUrlsAsync(baseUrl)).ToList();

            Assert.Single(result);
            Assert.Equal("https://test.com", result[0].OriginalUrl);
            Assert.Equal("http://localhost/123abc", result[0].ShortUrl);
        }

        [Fact]
        public async Task GetUrlByIdAsync_ValidId_ReturnsCorrectUrl()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                PasswordHash = "dummyhash" // ОБОВ'ЯЗКОВО
            };
            var url = new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://dotnet.com",
                ShortCode = "123abc",
                CreatedAt = DateTime.UtcNow,
                Clicks = 10,
                CreatedBy = user,
                CreatedById = user.Id
            };

            _context.Users.Add(user);
            _context.ShortUrls.Add(url);
            await _context.SaveChangesAsync();

            var baseUrl = "http://localhost/";

            var result = await _service.GetUrlByIdAsync(url.Id, baseUrl);

            Assert.Equal("https://dotnet.com", result.OriginalUrl);
            Assert.Equal("http://localhost/123abc", result.ShortUrl);
            Assert.Equal(10, result.Clicks);
            Assert.Equal("admin", result.CreatedBy);
        }

        [Fact]
        public async Task DeleteUrlAsync_Admin_CanDelete()
        {
            var url = new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://delete.com",
                ShortCode = "123abc",
                CreatedById = Guid.NewGuid()
            };

            _context.ShortUrls.Add(url);
            await _context.SaveChangesAsync();

            await _service.DeleteUrlAsync(url.Id, Guid.NewGuid(), isAdmin: true);

            var exists = await _context.ShortUrls.AnyAsync(u => u.Id == url.Id);
            Assert.False(exists);
        }

        [Fact]
        public async Task DeleteUrlAsync_NotOwner_ThrowsException()
        {
            var ownerId = Guid.NewGuid();
            var otherId = Guid.NewGuid();

            var url = new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://protected.com",
                ShortCode = "123abc",
                CreatedById = ownerId
            };

            _context.ShortUrls.Add(url);
            await _context.SaveChangesAsync();

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                _service.DeleteUrlAsync(url.Id, otherId, isAdmin: false));
        }

        [Fact]
        public async Task GetByShortCodeAsync_ReturnsCorrectUrl()
        {
            var url = new ShortUrl
            {
                Id = Guid.NewGuid(),
                OriginalUrl = "https://short.com",
                ShortCode = "123abc",
                CreatedById = Guid.NewGuid()
            };

            _context.ShortUrls.Add(url);
            await _context.SaveChangesAsync();

            var result = await _service.GetByShortCodeAsync("123abc");

            Assert.NotNull(result);
            Assert.Equal("https://short.com", result!.OriginalUrl);
        }

        [Fact]
        public async Task IncrementClickCountAsync_IncrementsClick()
        {
            var id = Guid.NewGuid();

            var url = new ShortUrl
            {
                Id = id,
                OriginalUrl = "https://click.com",
                ShortCode = "123abc",
                CreatedById = Guid.NewGuid(),
                Clicks = 5
            };

            _context.ShortUrls.Add(url);
            await _context.SaveChangesAsync();

            await _service.IncrementClickCountAsync(id);

            var updated = await _context.ShortUrls.FindAsync(id);
            Assert.Equal(6, updated!.Clicks);
        }
    }
}
