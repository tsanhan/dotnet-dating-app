using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        //1. update to return a task
         Task<string> CreateToken(AppUser user);
         //2. back to TokenService.cs, point 5
    }
}