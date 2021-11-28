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
        public UsersController(IUserRepository userRepository, IMapper mapper, /*1. inject photo service*/ IPhotoService photoService)
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

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var rtn = await _userRepository.GetMemberAsync(username);

            return rtn;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            //4. use the extention
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

        //2. HTTP: we creating a new resource
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            // 1. make sure we have somthing in the file property (the file name right)
            var username = User.GetUsernae();
            var user = await _userRepository.GetUserByUserNameAsync(username);

            var result = await _photoService.UploadPhotoAsync(file);// 'step into' the UploadPhotoAsync

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

            if(await _userRepository.SaveAllAsync())
                return _mapper.Map<PhotoDto>(photo);
            
            return BadRequest("Problem adding photos");
        }
        
        
    }
}