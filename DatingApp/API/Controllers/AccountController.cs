using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;

        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
            return user;

        }

        //1. creating post endpoint (we send data in body)
        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto) //2. we'll create a separate dto because later we'll ask for more from RegisterDto,create and go to LoginDto.cs 
        {
            //3. get user from db
            /*in Users: 
            checkout the method FindAsync method: it's nice if we getting by a PK, our username is not a PK 
            option 2: FirstOrDefaultAsync, from inline doc: returns first element, this would work... 
            option 3: SingleOrDefaultAsync: this one throe exception if >1 in the list after condition 
            
            ok so option 3: they are pretty much the same... */

            var user = await this._context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("invalid username");

            //4. check password... so using hmac todo the reverse of what we did to register:
            // calculate the hash using the salt and the given password
            
            //there is an overload to this ctor.
            // we want to hash to be created not with random key, 
            //but with the salt (perviously generated key)
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            
            //5. to compare with stored hash we need to loop over the bytearray:
            for (var i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            }

            return user;

        }

        private async Task<bool> UserExist(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}