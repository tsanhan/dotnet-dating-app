namespace API.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        
        // we'll use a photo storage service that need this property... 
        // so this is to save a new migration when we get there, 
        public string PublicId { get; set; } 
        
    }
}