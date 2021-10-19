using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto) // 1. using the DTO
        {
            if(await UserExist(registerDto.Username)) return BadRequest("Username is taken"); //3. appling the check, we can return BadRequest (400 http status) because we return ActionResult   

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(), // 4. all users names are in lower case 
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
            return user;

        }

        //2. we want our usernames to be unique so we'll build a halper method to ensure that
        private async Task<bool> UserExist(string username){
            return  await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}