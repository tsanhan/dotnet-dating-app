using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper; 
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            // 3. do the same here
            var users = await _userRepository.GetMembersAsync();
            
            // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            // 2. update this to use the new projection
            var rtn = await _userRepository.GetMemberAsync(username);
            
            // 1. no need for this
            // var userToReturn = _mapper.Map<MemberDto>(rtn);

            // 4. test in postman
            // not working... we still quering the full Entity (with the password hash and salt)
            // why do you think that is?
            // answer: because AutoMapper uses the GetAge method in the AppUser
            // the method needs a property from an AppUser Entity object, therefor an AppUser object must be created.
            // to fix this go to AppUser.cs
            return rtn;
        }

    }
}