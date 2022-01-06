using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        //we'll have only 2 endpoints:

        //1. get all users - testing the authorization
        [Authorize(Policy = "RequireAdminRole")] //we don't have this policy yet, so we'll create it later
        [HttpGet("users-with-roles")]
        public  ActionResult GetUsersWithRoles()
        {
            return Ok("Only admins can see this");
        }


        //2. get photos to moderate - testing the authorization
        [Authorize(Policy = "ModeratePhotoRole")] //we don't have this policy yet, so we'll create it later
        [HttpGet("photos-to-moderate")]
        public  ActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }

        //3. now we need to create these policies
        // * go to IdentityServiceExtensions.cs
    }
}