using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class TokenService : ITokenService //1. implementing the interface
    {
        //2. add the service as singleton in startup.cs (go to)
        public string CreateToken(AppUser user)
        {
             
        }
    }
}