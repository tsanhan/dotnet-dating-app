using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
         void Update(AppUser user);

         Task<bool> SaveAllAsync();

         Task<IEnumerable<AppUser>> GetUsersAsync();
         Task<AppUser> GetUserByIdAsync(int id);
         Task<AppUser> GetUserByUserNameAsync(string username);

        //1. change this to return PagedList and accept user parameters
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);// we can't call it params, because it's a reserved keyword

        Task<MemberDto> GetMemberAsync(string username);

        //2. go to UserRepository.cs and apply the change

    }
}