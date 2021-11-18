using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        //1. not async because we just updating the tracking status in EF
         void Update(AppUser user);

         Task<bool> SaveAllAsync();

         Task<IEnumerable<AppUser>> GetUsersAsync();
         Task<AppUser> GetUserByIdAsync(int id);
         Task<AppUser> GetUserByUserNameAsync(string username);
    }
}