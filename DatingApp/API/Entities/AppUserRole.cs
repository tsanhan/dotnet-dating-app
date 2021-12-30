using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    //1. derive from IdentityUserRole<int>
    public class AppUserRole: IdentityUserRole<int>
    {
        //2. just like we did for UserLike, we'll add properties for the join table
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
        
        //3. back to AppRole.cs to add the AppUserRole collection, point 5.
    }
}