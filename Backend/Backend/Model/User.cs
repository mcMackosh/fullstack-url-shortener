using System.ComponentModel.DataAnnotations;

namespace Backend.Model
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(10)]
        public string Role { get; set; } = "User";

        public ICollection<ShortUrl> ShortUrls { get; set; }
    }
}
