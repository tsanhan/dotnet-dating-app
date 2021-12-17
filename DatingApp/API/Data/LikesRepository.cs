using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }
        
        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            //1. this is where we need to fix the bug
            // * if we try to add an existing like, this method return null (no existing like)
            // * the reason for that is that FindAsync accept likedUserId first and sourceUserId second
            // * how do I know that? when calling this method the terminal will print the EF created query for the DB
            // return await _context.Likes.FindAsync(sourceUserId, likedUserId);
            return await _context.Likes.FindAsync(likedUserId, sourceUserId);
            //2. and back to README.md
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            
           
            IQueryable<AppUser> users;
            var likes = _context.Likes.AsQueryable();//get the likes
           
            if(predicate == "liked") {
                likes = likes.Where(like => like.SourceUserId == userId); 
                users = likes.Select(like => like.LikedUser); 
            }
            else {
                likes = likes.Where(like => like.LikedUserId == userId); 
                users = likes.Select(like => like.SourceUser); 
            }
            
            
            return await users.Select(user => new LikeDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            }).ToListAsync();

        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(u => u.LikedUsers)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}