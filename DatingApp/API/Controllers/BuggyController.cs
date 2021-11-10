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
            var thing  = _context.Users.Find(-1);
            if(thing == null)
            {
                return NotFound() ;// 404 not found
            }
            return Ok(); //🤣
        }

        //3. server error
        [HttpGet("server-error")] // buggy/server-error
        public ActionResult<string> GetServerError()
        {
            var thing  = _context.Users.Find(-1);
            var thingtoReturn = thing.ToString(); //NullReferenceExaption: 'thing' will be null (checkout Find inline doc)
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