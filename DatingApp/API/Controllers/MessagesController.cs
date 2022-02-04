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
        //2. also remove the repositories and replace with unit of work
        // * switch all _messageRepository with _unitOfWork.MessageRepository
        // * switch all _userRepository with _unitOfWork.UserRepository
        // * switch all SaveChangesAsync() to Complete()
        // ...
        //3. after no JIT interpolation errors, we'll continue fixing UsersController.cs
        //4. go to LogUserActivity.cs
        // private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IUserRepository _userRepository;
        public MessagesController(
            // IUserRepository userRepository,
            // IMessageRepository messageRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            // _userRepository = userRepository;
            // _messageRepository = messageRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send a messages to yourself!");



            var sender = await _unitOfWork.UserRepository.GetUserByUserNameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

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

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();

            var messages = await _unitOfWork.MessageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);


            return messages;
        }

        //1. we don't use this method anymore.
        // * we get the thread from the SignalR hub now
        // * we can go and follow the FE part to see that the component calling the service that calling this API, never runs
        // [HttpGet("thread/{username}")]
        // public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        // {
        //     var currentUsername = User.GetUsername();
        //     var messageThread = await _unitOfWork.MessageRepository.GetMessageThread(currentUsername, username);

        //     return Ok(messageThread);
        // }

        [HttpDelete("{id}")] 
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();
            var message = await _unitOfWork.MessageRepository.GetMessage(id); 


            if (message == null)
                return NotFound();

            if (message.SenderUsername != username && message.RecipientUsername != username)
                return Unauthorized();

            else message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted) _unitOfWork.MessageRepository.DeleteMessage(message);


            if (await _unitOfWork.Complete())
                return Ok();

            return BadRequest("Failed to delete message");
        }
        

    }
}