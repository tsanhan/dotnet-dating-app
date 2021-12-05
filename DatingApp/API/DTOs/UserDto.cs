namespace API.DTOs {
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }

        public string PhotoUrl { get; set; } 

        //1.add this property
        public string KnownAs { get; set; }
    
    }
}