using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;

namespace API.Data
{
    //1. implement the interface
    public class LikesRepository : ILikesRepository
    {
        //2. inject the context
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }
        //3. add this repository as a service in our app, go to ApplicationServiceExtensions.cs
        public Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppUser> GetUserWithLikes(int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}