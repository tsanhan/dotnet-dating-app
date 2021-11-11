using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;

        }
        //1. create some methods to return different types of errors 
        [Authorize] // with return 401 unauthorized
        [HttpGet("auth")] // buggy/auth
        public ActionResult<string> GetSecret()
        {
            return "secret string";
        }

        //2. return not found
        [HttpGet("not-found")] // buggy/not-found
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null)
            {
                return NotFound();// 404 not found
            }
            return Ok(); //🤣
        }

        //3. server error
        [HttpGet("server-error")] // buggy/server-error
        public ActionResult<string> GetServerError()
        {

            //1. things used to be like this try catch block but:
            // * this way we dont get any infomation in the console logs (because we swallowed the exception)
            // * the data about the exception is in ex, we don;t do much about it
            // so we wont use try catch blocks all over the place, it's a bad practice and it's not the 1990's 
            // we'll create our own middleware to handle  exceptions specifically in a global way

            // try
            // {
            //     var thing = _context.Users.Find(-1);
            //     var thingtoReturn = thing.ToString();
            //     return thingtoReturn; //🤣
            // }
            // catch (Exception ex)
            // {
            //     return StatusCode(500, "Computer Says no!");
            // }

            
            var thing = _context.Users.Find(-1);
            var thingtoReturn = thing.ToString();
            return thingtoReturn; //🤣

        }


        //4. bad request
        [HttpGet("bad-request")] // buggy/bad-request
        public ActionResult<string> GetBetRequest()
        {
            return BadRequest("this was not a good request");
        }

        //5. to to RegisterDTO to add another validator

    }
}