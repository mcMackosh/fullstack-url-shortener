namespace Backend.DTO
{
    public class AboutDto
    {
        public string Description { get; set; } = string.Empty;
    }

    public class AboutResponseDto
    {
        public string Description { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
    }

}
