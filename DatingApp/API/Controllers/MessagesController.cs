using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //1. authorize and derive from base api
    [Authorize]
    public class MessagesController : BaseApiController
    {
        //2. we need access to: user repo  
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public MessagesController(
            IUserRepository userRepository,
            IMessageRepository messageRepository,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        //3. create a message action
        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            //4. get the username from the token
            var username = User.GetUsername();

            //5. a small assertion: check that I dont send to myself
            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send a messages to yourself!");


            //6. we need to populate the message object with data about the two members:
            // so first of all we need to get both users

            var sender = await _userRepository.GetUserByUserNameAsync(username);
            var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

            //7. a small assertion: check that the recipient exists
            if (recipient == null)
                return NotFound();

            //8. create the message object
            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            //9. save the message
            _messageRepository.AddMessage(message);

            //10. we need to return a massage dto
            // * as we see before when adding a photo we need to return CreatedAtRoute.
            // * but to get things moving and we don't have a route to get an individual message just now, we'll skip that
            // * we'll just return the message dto mapped from the message we created
            if(await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");

            //11. back to README.md
        }


    }
}