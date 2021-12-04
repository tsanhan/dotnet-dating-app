namespace API.DTOs {
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }

        public string PhotoUrl { get; set; } //1. ad this, this will be the main photo
    
    }
}