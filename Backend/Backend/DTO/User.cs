namespace Backend.DTO
{

    public class LoginResponseDto
    {

        public Guid Id { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }

       
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }

    public class LoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
