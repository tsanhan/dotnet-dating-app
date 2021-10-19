namespace API.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; } //1. here it doest matter if we use Username and not UserName
        public string Password { get; set; }
    }
    // 2. go to AccountController to use it
}