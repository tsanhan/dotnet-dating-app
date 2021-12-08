using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _mapper = mapper;
            _photoService = photoService;
            _userRepository = userRepository;
        }

        //1. update this method to return a PagedList and add the Pagination reader 
        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers(/*4. adding FromQuery*/[FromQuery] UserParams userParams)
        {
            var users = await _userRepository.GetMembersAsync(userParams);
            // 2. we allways have an access to re Response object in a controller (property of controller base)
            //  * oops, change AddApplicationHeader to AddPaginationHeader in the extension method
            Response.AddPaginationHeader(
                users.CurrentPage, 
                users.PageSize, 
                users.TotalCount, 
                users.TotalPages);
            return Ok(users);
            //3. test functionality in postman,  seasion 13, start with 'Get Users No QS'.
            //  * aaand we get 415 Unsupported Media Type, our API is not that smart with our parameters...
            //  * to fix this we need to add [FromQuery] to the parameters, 
            //      * the query string does suppose to be inside an objects
            //      * so the API does not know what to do with it
            //  * we need to add a new parameter to the UserParams class,
            //  * ok so we get only 10 users, why is that? (because we are using defaults in the UserParams class)
            //  * what about the headers, we get them, great!

            //5. test functionality in postman,  seasion 13: 'Get Users with pagination'.
            //  * we get the pagination headers and responses looking good based on params passed!
            //6. one thing i don't like: the Pagination header is in title case, and i want it in camel case. 
            //  * to fix this go to HttpExtensions.cs
        }   

        [HttpGet("{username}", Name = "GetUser"),]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var rtn = await _userRepository.GetMemberAsync(username);

            return rtn;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            var username = User.GetUsernae();
            var user = await _userRepository.GetUserByUserNameAsync(username);

            _mapper.Map(memberUpdateDTO, user);

            _userRepository.Update(user);


            if (await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to update user");

        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.GetUsernae();
            var user = await _userRepository.GetUserByUserNameAsync(username);

            var result = await _photoService.UploadPhotoAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };


            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photos");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var username = User.GetUsernae();

            var user = await _userRepository.GetUserByUserNameAsync(username);

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already the main photo");

            var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;


            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set photo to main");
        }

        // 1. add this method
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUsernae();

            var user = await _userRepository.GetUserByUserNameAsync(username);

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo == null) return BadRequest("Photo not found");

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null) 
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete photo");
        }
    }
}