using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    //1. derive from IdentityRole
    public class AppRole: IdentityRole<int>
    {
        //2. no need for properties at the moment
        //3. we do will want to get a list of roles a user is in (let's say a user is a user and also an admin)
        //4. identity does not support this list (it does supoort user => single role though),
        // * how would you approach this issue (the issue of assigning multiple roles to a user)?
        // * answer: this is a many to many relationship between the user entity and the role entity
        // * so create and go to Entities/AppUserRole.cs

        //5. add AppUserRole collection
        public ICollection<AppUserRole> UserRoles { get; set; }
        //6. do the same for AppUser.cs, go there, point 5.
    }
}