using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    //1. first thing to do is to derive from IdentityUser, and set int as primary key
    public class AppUser : IdentityUser<int>
    {
        //2. when we derive from IdentityUser, we get cretin properties.
        // * we can see that we try to overide Id UserName and PasswordHash (yellow underline)
        // * so we don't need those properties, we've been provided to us by the parent IdentityUser
        // * we also don't need the PasswordSalt, it's being internally computed in side the Identity framework
        
        // public int Id { get; set; }
        // public string UserName { get; set; }
        // public byte[] PasswordHash { get; set; }
        // public byte[] PasswordSalt { get; set; }

        //3. the rest can stay as they are. we'll see some errors as we change things, don't worry about it
        //4. we want to create a new entity for user roles, create and go to Entities/AppRole.cs
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
        public ICollection<UserLike> LikedByUsers { get; set; }
        public ICollection<UserLike> LikedUsers { get; set; }

        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        
        //5. add AppUserRole collection        
        public ICollection<AppUserRole> UserRoles { get; set; }
        //6. if we run this API we'll get errors in our AccountController and Seed.cs
        // * the reason is that we removed the PasswordSalt property
        // * to fix that, at this point we'll just remove all thing related to create a password hash and salt
        // * because we'll be using Identity framework to create the password hash and salt later 
        // * go to AccountController.cs

        
    }
}