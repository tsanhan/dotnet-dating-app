using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();

            return Ok(users);
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

            if(result.Error != null){
                return BadRequest(result.Error.Message);
            }
            var photo = new Photo {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            
            if(user.Photos.Count == 0){
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync()){
                return CreatedAtRoute("GetUser",new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
            }
            
            return BadRequest("Problem adding photos");
        }
        
        //1. we updating something so it's a PUT
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            //2. when we getting the user this way we validatint they are who they say they are
            // we can trust the infomation inside the token, our server signed the token, and the user send the token
            var username =  User.GetUsernae(); 

            //3. we gen the photos eagerly here, so we have access to the photos
            var user = await _userRepository.GetUserByUserNameAsync(username);

            //4. this is synchronous, we allready have the user and it's photos in memory, no walk to the DB
            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            //5. this will happen because we will prevent the user set a main photo as main
            if(photo.IsMain) return BadRequest("This is already the main photo");

            //6. we are setting the main photo to false
            var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
            if(currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;
            

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set photo to main");
            //7. test in postman, section 11: 
            //  1. login
            //  2. make sure you have at least 2 photos
            //  3. get user by username
            //  4. use the set main photo PUT call with the right photoId
        }

    }
}