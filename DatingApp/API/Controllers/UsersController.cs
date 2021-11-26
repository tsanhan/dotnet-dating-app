using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var users = await _userRepository.GetMembersAsync();
            
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var rtn = await _userRepository.GetMemberAsync(username);
            
            return rtn;
        }

        //1. we need to create a DTO to receive as a parameter so go and create MemberUpdateDTO.cs
        //2. coming back from auto mapper, we need to add this:
        
        //  * PUT: in REST: update a full entity (PATCH: update only a few fields)
        //  * no need for parameters, because there is no conflict with anything in the API controller 
        //  * the name UpdateUser is not really relevant
        //  * what is relevant is the method(HTTP verb), the parameters in the route and those we take in the method
        //  * what do you thing would happen if we had, for example [HttpGet("{id}")]?
        //  * answer: we would get a conflict in our routes and will need to somehow change to [HttpGet("id/{username}")] or something to have a different route
        //  * right now there is no conflict, this is hte only PUT method we have
        [HttpPut]
        //3. no need to return anything, the client has all the data about the updated entity
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            //4. we want to have hold of the user and username, 
            // * we don't believe to the client giving us the right username.
            // * we'll authenticate against the token, and we'll get the username from the token
            // * in the controller we have access to the ClaimsPrincipal (it's an object created from the token sent from the client side)
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // I'm looking for the NameIdentifier claim (nameid in the payload in the jwt)
            var user = await _userRepository.GetUserByUserNameAsync(username);

            // map the DTO to the user automatically (otherwise we would have to do it manually)
            // no need for that: user.City = memberUpdateDTO.City;
            _mapper.Map(memberUpdateDTO, user); 

            _userRepository.Update(user); 
            // now the entity is flagged as updated by EF (it's not saved yet and it doesn't matter if the entity was actually modified)

            if(await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }

            // if failed, return a bad request
            return BadRequest("Failed to update user");
            // 5. test our api in postman, and see if it works, 
            // * go to postman section 9
            // * start with the login to save the token as an environment variables
            // * and then update the user
            // * good - 204: no content
            // * go to member/edit in the client to see the updated data
        }

    }
}