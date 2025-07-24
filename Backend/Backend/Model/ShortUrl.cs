using System.ComponentModel.DataAnnotations;

namespace Backend.Model
{
    public class ShortUrl
    {
        public Guid Id { get; set; }

        [Required]
        [Url]
        public string OriginalUrl { get; set; }

        [Required]
        [MaxLength(10)]
        public string ShortCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Clicks { get; set; }

        // Зв’язок з користувачем
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
    }
}
