namespace API.DTOs
{
    public class LikeDto
    {
        //1. so what we want to return as a like:
        public int Id { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        
        public string KnownAs { get; set; }
        
        public string PhotoUrl { get; set; }
        
        public string City { get; set; }
    
        //2. just so happens this is also what we need to create the member card (in the member list)
        //   * this is because we'll display the users we like in the same manner, in cards
        //3. go back to ILikesRepository.cs
    }
}