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
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [Authorize(Policy = "RequireAdminRole")] 
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users
            .Include(r => r.UserRoles) 
            .ThenInclude(r => r.Role)   
            .OrderBy(r => r.UserName)   
            .Select(u => new            
            {
                u.Id,  
                Username = u.UserName, 
                Roles = u.UserRoles.Select(r => r.Role.Name).ToList()  
            })
            .ToListAsync();

            return Ok(users);
        }


        [Authorize(Policy = "ModeratePhotoRole")] 
        [HttpGet("photos-to-moderate")]
        public  ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }

        //1. to edit the roles, we'll create an endpoint for that
        // * the endpoint will be get the username from the url, and the roles from the query params 
        [Authorize(Policy = "RequireAdminRole")] 
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(',').ToArray();
            var user = await _userManager.FindByNameAsync(username);
            if(user == null) return NotFound("Could not Find User");


            var userRoles = await _userManager.GetRolesAsync(user);

            //2. no we don't have the ability to set the user roles, only to add/remove roles
            // * so we'll use some group logic here (add missing roles and remove extra roles)

            //add missing roles
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            
            if (!result.Succeeded) return BadRequest("Failed to add to roles");
            
            //remove extra roles
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            
            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            // returning the current user roles
            return Ok(await _userManager.GetRolesAsync(user));
            
            //3. back to README.md 
        }
    
    }
}