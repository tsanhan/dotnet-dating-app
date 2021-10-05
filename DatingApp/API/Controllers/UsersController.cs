using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController] // 2.
    [Route("api/[controller]")]// 3. convention over configuration
    public class UsersController : ControllerBase // 1. first thing first
    {
        private readonly DataContext _context;
        public UsersController(DataContext context) // 4. DI the context and right click on context => initial field from parameter
        {
            _context = context;

        }

        // 5. adding endpoints for all the users and specific user:
        [HttpGet]
        // 6. returning action result to wrap the data in a proper manner (we can dig into the class to see it support async operation)
        // 7. the data is IEnumerable (a basic iteration capable list), we can use List but it's an overkill
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {

            var users = _context.Users.ToList();
            return users;
        }

        // 8.
        [HttpGet("{id}")] //id: route parameter:  api/users/3
        public ActionResult<AppUser> GetUser(int id)
        {

            return _context.Users.Find(id); // Find search by primary key
        }

    }
}