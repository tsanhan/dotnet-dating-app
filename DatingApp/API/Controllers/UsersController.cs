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
        

        private readonly IUnitOfWork _unitOfWork;
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
           //1. this list is making the big query, do we need it? lets see... 
           //4. lets fix this query:
            var gender = await _unitOfWork.UserRepository.GetUserGender(User.GetUsername());
            
            if(string.IsNullOrEmpty(userParams.Gender))
            {
                //3. so the the only need on this Entity is it's gender.
                // * let's create a query in the repo just for that, go to IUserRepository.cs
                //5. user the new variable
                userParams.Gender = gender == "male" ? "female" : "male";
                //6. go to README.md
            }

            userParams.CurrnetUsername = User.GetUsername();//2. we don't need to get the username from the Entity, we can get it from the Token.
            var users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(
                users.CurrentPage, 
                users.PageSize, 
                users.TotalCount, 
                users.TotalPages);
            return Ok(users);
        }   

        
        [HttpGet("{username}", Name = "GetUser"),]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var rtn = await _unitOfWork.UserRepository.GetMemberAsync(username);

            return rtn;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            var username = User.GetUsername();
            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(username);

            _mapper.Map(memberUpdateDTO, user);

            _unitOfWork.UserRepository.Update(user);


            if (await _unitOfWork.Complete())
            {
                return NoContent();
            }
            return BadRequest("Failed to update user");

        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.GetUsername();
            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(username);

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

            if (await _unitOfWork.Complete())
            {
                return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem adding photos");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var username = User.GetUsername();

            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(username);

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already the main photo");

            var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;


            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to set photo to main");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var username = User.GetUsername();

            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(username);

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo == null) return BadRequest("Photo not found");

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null) 
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete photo");
        }
    }
}