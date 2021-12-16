using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        //1. so we'll support 3 methods:
        // 1.1. get a like (from the joined table)
        // 1.2. get a user with the likes included
        // 1.3. get a likes for a user ('liked by' or 'liked')
        //2. we'll use a dto to select the properties we intersted in
        // * so create a dto, create and go to DTOs/LikeDto.cs

        
        //3. implement 1.1, using the primary key (based on two fields)
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);

        //4. implement 1.2, using the userId
        Task<AppUser> GetUserWithLikes(int userId);

        //5. implement 1.3, we are looking for a list of users that have been liked or liked by
        Task<IEnumerable<LikeDto>> GetUserLikes(string predicate /*predicate: 'liked' or 'liked by'*/, int userId);
        //6. NOw we'll create the implementation of this interface
        //  * create and go to Data/LikesRepository.cs
    }

}