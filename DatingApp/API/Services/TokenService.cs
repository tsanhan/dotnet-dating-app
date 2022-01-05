using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {


        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;

        // 2. we'll inject add the user manager (not the role manager, we want the user roles)
        // * and initialize the field from parameter
        public TokenService(IConfiguration config,/*inject the user manager*/ UserManager<AppUser> userManager)
        {

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _userManager = userManager;
        }
        // 4. make the method async
        // * need to update the Interface, go to ITokenService.cs
        public async Task<string> CreateToken(AppUser user)
        {

            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                //1. we have 2 claims here, adding the roles here is safe, the user can't trick the server using different claim
                // * if the token is modified, if u don't have the sectret key, the token will be invalid
            };

            //3. get the user roles (a list of roles this user belongs to)
            var roles = await _userManager.GetRolesAsync(user);

            //5. add these roles to the list of claims (Select in Linq is like map in js)
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));// we use ClaimTypes because JwtRegisteredClaimNames don't have roles property 
            
            //6. now because we chaned this method to be async we need to update the account controller 
            // * go to AccountController.cs

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}