using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class MemberDto
    {
        // pasted from AppUser with slight changes
        public int Id { get; set; }
        public string UserName { get; set; }
        // public byte[] PasswordHash { get; set; }
        // public byte[] PasswordSalt { get; set; }
        // public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        
        public string KnownAs { get; set; }
        public DateTime Created { get; set;} // = DateTime.Now; 
        public DateTime LastActive { get; set; } // = DateTime.Now; 
        public string Gender { get; set; } 
        public string Introduction {get;set;}
        public string LookingFor {get;set;}
        public string Interests {get;set;}
        public string City {get;set;}
        public string Country {get;set;}
        // public ICollection<Photo> Photos {get;set;} 
        public ICollection<PhotoDto/*need to create PhotoDto*/> Photos {get;set;} 
    }
}