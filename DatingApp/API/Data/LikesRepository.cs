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
            //1. start with the easyest one, just get the user like by the primary key
            return await _context.Likes.FindAsync(sourceUserId, likedUserId);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            //3. this is a bit tricky one, we'll query baked on predicate
            //   * predicate: 'liked' or 'liked by', so the question are:
            //   * 1. 'liked' => "which users this user has liked" => userId is the source user 
            //   * 2. 'liked by' => "which users have been liked by this user" => userId is the liked user

           
            IQueryable<AppUser> users;
            var likes = _context.Likes.AsQueryable();//get the likes
            //4. looks like 2 queries, but not really, 
            //  * we'll be joining the two and let EF figure out the join query for the db 

            //5. search for which users this user has liked
            if(predicate == "liked") {
                likes = likes.Where(like => like.SourceUserId == userId); //filter
                users = likes.Select(like => like.LikedUser); // select the relevant users (overriding initial users select)
            }
            //6. search for users that have been liked by this user
            else {//if predicate == "likedBy"
                likes = likes.Where(like => like.LikedUserId == userId); //filter
                users = likes.Select(like => like.SourceUser); // select the relevant users (overriding initial users select)
            }
            
            //7. we''ll use the DTO to select the properties we intersted in,
            //   * no need to configure Mapping here, these are not so mulch properties
            return await users.Select(user => new LikeDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            }).ToListAsync();
            //8. back to readme.md

        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            //2. also simple: get the users with his likes included
            return await _context.Users
                .Include(u => u.LikedUsers)// when a user will ad a like it will be added to the likedUsers list here
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}