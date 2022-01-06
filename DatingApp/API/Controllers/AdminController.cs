using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;

        //1. add a constructor to inject what we need
        public AdminController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }


        [Authorize(Policy = "RequireAdminRole")] 
        [HttpGet("users-with-roles")]
        //2. make the method async that return a task 
        public async Task<ActionResult> GetUsersWithRoles()
        {
            //3. get all users
            var users = await userManager.Users
            .Include(r => r.UserRoles)  // go to the joint table
            .ThenInclude(r => r.Role)   // to get to the roles table
            .OrderBy(r => r.UserName)   // order by username
            .Select(u => new            // and project to anonymous object
            {
                u.Id, // id  
                Username = u.UserName, // username
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList() // list of roles the user in 
            })
            .ToListAsync();

            // we return the results, not paginated (we allready learned that), you can paginate, as HW 
            return Ok(users);
            //4. back to README.md
        }


        [Authorize(Policy = "ModeratePhotoRole")] 
        [HttpGet("photos-to-moderate")]
        public  ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }

    
    }
}