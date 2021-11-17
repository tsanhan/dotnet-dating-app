using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        
        public string PublicId { get; set; } 

    
        //1. tell entity framework that this entity has a relationship with another entity
        //* this is 'fully defining' a relationship
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        
        
    }
}