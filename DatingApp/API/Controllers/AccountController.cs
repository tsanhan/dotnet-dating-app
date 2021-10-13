using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;

        }

        [HttpPost("register")]/*POST: http call to add resource, POST can contain Body*/
        // build an action to register new users (that's why the POST)
        /*
            ApiConteroller maps the key to parameter, otherwise: [FromBody] string username... ,
            because we can get data from different places (query string, route, header, etc...)
         */
        public async Task<ActionResult<AppUser>> Register(string username, string password)
        {
            // well use the hashing algo for pass hash
            // 'using' when we finish using 'hmac' it will we disposed of (inherited from IDisposable (can investigate parents) - calling dispose() on destruction - like when exiting scope)
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = username,
                PasswordHash = hmac.ComputeHash(/*need to provide bytes*/System.Text.Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key // because it's randomly generated
            };

            _context.Users.Add(user); /*look at the description, here we only saying that we *want* to add, 
                                        we do this by tracking this action and the entities related to this action*/

            await _context.SaveChangesAsync(); // the actual operation
            return user;

        }
    }
}