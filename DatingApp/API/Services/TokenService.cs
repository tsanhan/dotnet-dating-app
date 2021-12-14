using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {


        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {

            var claims = new List<Claim>{
                //1. so what we doing here is we are adding claims to the token, the UserName to the NameId claim
                // so we don't have a lot of identifiers here it terms of ClaimNames, we have NameId, UniqueName, and thant's basically it.
                // so we change the NameId claim to store the user id
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                //2. and have UniqueName claim to store the username
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),

                //3. so now we have to update the claims extensions and update where to find the user name
                // go to ClaimsPrincipalExtensions.cs
            };

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