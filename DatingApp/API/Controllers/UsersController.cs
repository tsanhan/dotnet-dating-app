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
        public UsersController(IUserRepository userRepository, IMapper mapper/*3. import the mapper*/)
        {
            _mapper = mapper; 
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto/*1. change that*/>>> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();
            // 4. map the objects to return
            var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto/*2. change that*/>> GetUser(string username)
        {
            var rtn = await _userRepository.GetUserByUserNameAsync(username);
            // 5. map the object to return
            var userToReturn = _mapper.Map<MemberDto>(rtn);
            
            return userToReturn;
        }

    }
}