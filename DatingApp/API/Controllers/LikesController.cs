using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        //1. no need for the repositories
        // private readonly IUserRepository _userRepository;
        // private readonly ILikesRepository _likesRepository;
        private readonly IUnitOfWork _unitOfWork;


        //2. inject IUnitOfWork
        public LikesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            // _likesRepository = likesRepository;
            // this._userRepository = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            //3.  change all '_userRepository' with  '_unitOfWork.UserRepository'
            // * and all '_likesRepository' with  '_unitOfWork.LikesRepository'
            var likedUser = await _unitOfWork.UserRepository.GetUserByUserNameAsync(username);

            var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest("you can't like yourself");

            var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId, likedUser.Id);

            if (userLike != null) return BadRequest("you already like this user");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);
            //4. change the SaveAllChanges() method to Complete()
            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to like user");

            //5. go next to MessagesController.cs

        }

        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            
            var users = await _unitOfWork.LikesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }



    }
}