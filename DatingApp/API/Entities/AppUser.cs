using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set;} = DateTime.Now; 
        public DateTime LastActive { get; set; } = DateTime.Now; 
        public string Gender { get; set; } 
        public string Introduction {get;set;}
        public string LookingFor {get;set;}
        public string Interests {get;set;}
        public string City {get;set;}
        public string Country {get;set;}
        public ICollection<Photo> Photos {get;set;} 

        //1. please use THIS name, the 'Get' in GetAge is important, we'll se later on
        public int GetAge() {
           return DateOfBirth.CalculateAge();
        }
    }
}