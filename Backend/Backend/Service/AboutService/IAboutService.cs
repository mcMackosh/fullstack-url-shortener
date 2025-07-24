using Backend.DTO;

public interface IAboutService
{
    Task<AboutResponseDto> GetAsync();
    Task UpdateAsync(AboutDto dto);
}