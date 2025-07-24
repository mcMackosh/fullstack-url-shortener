using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class CreateUrlRequestDto
    {
        [Required]
        [Url]
        public string OriginalUrl { get; set; }
    }

    public class ShortUrlResponseDto
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Clicks { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CreateUrlRequest
    {
        [Required]
        [Url]
        public string OriginalUrl { get; set; } = null!;
    }

    public class UrlListItemResponceDto
    {
        public Guid Id { get; set; } = default!;
        public string OriginalUrl { get; set; } = default!;
        public string ShortUrl { get; set; } = default!;
        public Guid CreatedBy { get; set; } = default!;
    }
}
