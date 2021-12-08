namespace API.DTOs {
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }

        public string PhotoUrl { get; set; } 

        public string KnownAs { get; set; }

        //1.add this property, we'll sent another bit of info to the client when we login 
        // this is to save us from making an api call just to know the user's gender
        public string Gender { get; set; }
        //2. go to AccountController.cs
    
        
    }
}