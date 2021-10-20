using System.ComponentModel.DataAnnotations;
namespace API.DTOs
{
    public class RegisterDto
    {
        //1. data annotation, 
        [Required]
        //we can add different validations:
        // [Phone]
        // [Email]
        // [StringLength(4)]
        // [RegularExpression("asd")] - if we want a custom format
        public string Username { get; set; } 

        [Required]
        public string Password { get; set; }
    }
}