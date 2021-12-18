using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {

        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);

        Task<AppUser> GetUserWithLikes(int userId);

        // 1. update to get likes params
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
        // 2. fo to LikesRepository.cs for the implementation

    }

}