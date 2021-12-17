using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    //1. derive from BaseApiController nad make sure we authenticate the user
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikesRepository _likesRepository;

        //2. inject repositories for users and likes
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            this._userRepository = userRepository;
        }

        //3. a mothod to like another user.
        //  * we create an entity but we dore return it? why?
        //  * we return data from POST so the client get the data created, here the client knows everything about the created entity
        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            //4. get the current user id
            var sourceUserId = User.GetUserId();

            //5. get the user to like
            var likedUser = await _userRepository.GetUserByUserNameAsync(username);

            //6. get the source user
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            //7. let's add some checks:
            //  * did't find the user to like?
            if (likedUser == null) return NotFound();
            
            //  * does the user want to like themself?
            if (sourceUser.UserName == username) return BadRequest("you can't like yourself");// a bit grim this one... 

            //  * does the user already like the user to like?
            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
            
            if (userLike != null) return BadRequest("you already like this user");

            //8. create the like
            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            //9. save the changes to the database
            // * we don't have save method in likes repository so we use the user repository
            // * this is temporary, now that that we have more then one repository...
            // * we need to think about how many instances of DataContext we have
            // * we'll think about a better option later 
            // * specifically, how to handle multiple saving of data, and all the entities EF is tracking
            // * and the architecture pattern for that (it's called 'unit of work')
            if (await _userRepository.SaveAllAsync()) return Ok();
            
            return BadRequest("Failed to like user");
            

            
        }
    
        //10. a method to get the users (as LikeDto) related to a user
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string predicate)
        {
            var users = await _likesRepository.GetUserLikes(predicate, User.GetUserId());
            return Ok(users);
        }

        //11. back to readme.md
       
    }
}