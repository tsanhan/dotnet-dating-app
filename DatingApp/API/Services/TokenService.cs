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
        //3. store key 
        // * symmetric means same key is used to encrypt AND decrypt - in our case sign and verify) 
        // * asymmetric means different keys, public and private (how https/ssl works);
        // jwt uses symmetric key because this key not leaving the server
        
        private readonly/*assignment (many times) is only in  same class ctor or on declare*/ SymmetricSecurityKey _key;
        
        //2. inject the configuration
        public TokenService(IConfiguration config)
        {
            // 4.
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]/*not exists yet*/));
        }
        public string CreateToken(AppUser user)
        {
             //1. install System.IdentityModel.tokens.jwt from nuget

            //5. adding claims to token:
            var claims = new List<Claim>{
                // the only claim for now
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            //6. adding credentials, need the key and algorithm
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); // using the strongest to sign our token 
     
            // 7. describe our token (how it's going to look)
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            // 8. after creating our token, we need token hander
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 9. returning the token after handling it
            return tokenHandler.WriteToken(token);

        }
    }
}