using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        // these will be calculated and returned to un as byte arrays from our BE so we'll save them as such in our DB 
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}