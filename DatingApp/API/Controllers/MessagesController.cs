using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
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

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send a messages to yourself!");



            var sender = await _userRepository.GetUserByUserNameAsync(username);
            var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

            if (recipient == null)
                return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();

            var messages = await _messageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);


            return messages;
        }


        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();
            var messageThread = await _messageRepository.GetMessageThread(currentUsername, username);

            return Ok(messageThread);
        }

        //1. add a method to delete a message
        [HttpDelete("{id}")] // take the id of the message we want to delete
        public async Task<ActionResult> DeleteMessage(int id)// no need to return anything when deleteing
        {
            var username = User.GetUsername();// get the username
            var message = await _messageRepository.GetMessage(id); // get the message

            // run some checks:

            // the message dos not exist
            if (message == null)
                return NotFound();

            // this is not your message buddy, your not the sender or the recipient
            if (message.SenderUsername != username && message.RecipientUsername != username)
                return Unauthorized();

            // who is deleting this message?
            if (message.Sender.UserName == username) message.SenderDeleted = true;
            else message.RecipientDeleted = true;

            // if they both deleted the message (this is not the first call to this endpoint with this id), then delete it
            if (message.SenderDeleted && message.RecipientDeleted) _messageRepository.DeleteMessage(message);


            if (await _messageRepository.SaveAllAsync())
                return Ok();

            return BadRequest("Failed to delete message");
        }
        //2. now we need to go to the repository to return messages that where not deleted by the user resqueting the messages  
        // * ok? if a user deleted a message he/she won't see it anymore in the inbox/outbox
        // * go to MessageRepository.cs 

    }
}