using System.Linq;
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
    public class MessageHub : Hub
    {
        private IMessageRepository _messageRepository;
        private IMapper _mapper;
        private IUserRepository _userRepository;
        private  PresenceTracker _tracker;

        public IHubContext<PresenceHub> _presenceHub { get; }

        public MessageHub(
            IMessageRepository messageRepository,
            IMapper mapper,
            IUserRepository userRepository,
            //1. add the presence hub's context to be able to send messages to the presence hub
            IHubContext<PresenceHub> presenceHub,
            //2. add the presence tracker to know who is online
            PresenceTracker tracker
            )
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _presenceHub = presenceHub;
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContent = Context.GetHttpContext();
            var otherUser = httpContent.Request.Query["username"].ToString();

            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await AddToGroup(groupName);

            var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await RemoveFromMessageGroup();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send a messages to yourself!");

            var sender = await _userRepository.GetUserByUserNameAsync(username);
            var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

            if (recipient == null)
                throw new HubException("Not found user");

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message);


            var group = GetGroupName(sender.UserName, recipient.UserName);
            var groupEntity = await _messageRepository.GetMessageGroup(group);

            if (groupEntity.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            //3. here, when we send a message, if the recipient is NOT in the group, we'll check if he is even online
            else {
                var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
                if (connections != null)
                {
                    // if this code runs, we know the recipient is online but not in the same mesage group as the sender
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", new {
                        username = sender.UserName,
                        knownAs = sender.KnownAs
                    });
                    // the client does not listen to 'NewMessageReceived' method yet... we'll deal with that now.
                    //go to presence.service.ts 
                }
                
            }

            if (await _messageRepository.SaveAllAsync())
            {
                await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }

        }


        private string GetGroupName(string current, string other)
        {
            var stringCompare = string.CompareOrdinal(current, other) < 0;
            return stringCompare ? $"{current}-{other}" : $"{other}-{current}";
        }

        private async Task<bool> AddToGroup(string groupName)
        {
            var group = await _messageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null)
            {
                group = new Group(groupName);
                _messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);
            return await _messageRepository.SaveAllAsync();
        }

        private async Task RemoveFromMessageGroup()
        {
            var connection = await _messageRepository.GetConnection(Context.ConnectionId);
            _messageRepository.RemoveConnection(connection);
            await _messageRepository.SaveAllAsync();
        }
    }
}