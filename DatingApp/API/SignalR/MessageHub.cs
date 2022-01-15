using System;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    //1. derive from Hub, again
    public class MessageHub : Hub
    {
        private IMessageRepository _messageRepository;
        private IMapper _mapper;
        private IUserRepository _userRepository;

        public MessageHub(
            IMessageRepository messageRepository,
            IMapper mapper,
            IUserRepository userRepository // inject this.

            )
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContent = Context.GetHttpContext();
            var otherUser = httpContent.Request.Query["username"].ToString();

            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);

        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        //1. create a method for sending messages
        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            //2. just copy all the code from from CreateMessage method in MessageController.cs and paste it here [do it]
            // * then we'll adapt things as we go along:
            // * fist thing: we don't have access to API responces (these are http responses, we don't have access to that inside somthing that doesn't use HTTP)

            var username = Context.User.GetUsername(); // User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                /*return BadRequest*/
                throw new HubException("You cannot send a messages to yourself!");
            // HubException will appear in the client side as an exaction comming from the hub, we'll need to handle it there 

            //* we still need the users here, inject UserRepository into this file.
            var sender = await _userRepository.GetUserByUserNameAsync(username);
            var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

            if (recipient == null)
                throw new HubException("Not found user");// return NotFound(); 

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveAllAsync())
            {
                // return Ok(_mapper.Map<MessageDto>(message));
                //3. here we'll do something a bit different:
                // * we'll just send the message to everybody in the group, to put on their board.
                // * but first, we need the group name, right?
                var group = GetGroupName(sender.UserName, recipient.UserName);
                await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
                
            }


            //4. no need for this at all 
            // return BadRequest("Failed to send message");
            //5. back to README.md
        }
        private string GetGroupName(string current, string other)
        {
            var stringCompare = string.CompareOrdinal(current, other) < 0;
            return stringCompare ? $"{current}-{other}" : $"{other}-{current}";

        }


    }
}