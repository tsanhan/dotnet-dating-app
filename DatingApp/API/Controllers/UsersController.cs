using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;

        }

        [HttpGet]
        // 1. this is sync code, this means that the tread is blocked until request fulfillment ( not scalable)
        // 2. even through web servers are multi threaded, this is wastfull, (say we have 100K users... not practical)
        // 3. how to make our code async: general rule: all DB access are async 
        // 4. we want to pass the task to another thread and let me be fee to accept other requests
        //  - this the passing is done using a Task (there is a Async vertion to ToList we'll see)
        public /*5. making async*/ async /*6. returning a task (like a JS promise)*/Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {

            var users = /*7. await response (for unwrap result from task*/ await _context.Users.ToListAsync(); // 8. ToListAsync is an extention method to wrap ToList in a Task
            // 9. another way to return the data and not a task but this is not a good because we still block until the task is finished
            // var users = _context.Users.ToListAsync().Result;
            // can read more here: https://stackoverflow.com/questions/13159080/how-does-taskint-become-an-int
            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {

            return await _context.Users.FindAsync(id); // Find search by primary key
        }

    }
}