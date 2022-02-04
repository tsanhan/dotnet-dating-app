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

        //1. also remove the repositories and replace with unit of work
        //  * switch all _userRepository with _unitOfWork.UserRepository
        //  * switch all _messageRepository with _unitOfWork.MessageRepository
        //  * switch all SaveAllAsync() to Complete()
        // ...
        //2. after no JIT interpolation errors, we'll continue fixing LogUserActivity.cs
        //3. go to LogUserActivity.cs
        // private IMessageRepository _messageRepository;
        private IMapper _mapper;
        // private IUserRepository _userRepository;
        private PresenceTracker _tracker;

        public IHubContext<PresenceHub> _presenceHub { get; }
        private readonly IUnitOfWork _unitOfWork;

        public MessageHub(
            IUnitOfWork unitOfWork,
            // IMessageRepository messageRepository,
            IMapper mapper,
            // IUserRepository userRepository,
            IHubContext<PresenceHub> presenceHub,
            PresenceTracker tracker
            )
        {
            _unitOfWork = unitOfWork;
            // _messageRepository = messageRepository;
            _mapper = mapper;
            // _userRepository = userRepository;
            _presenceHub = presenceHub;
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContent = Context.GetHttpContext();
            var otherUser = httpContent.Request.Query["username"].ToString();

            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);


            var messages = await _unitOfWork.MessageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);
            //4. this is where GetMessageThread is being called.
            // * we'll check if there are any changed EF is tracking and save them to DB
            if(_unitOfWork.HasChanges()) await _unitOfWork.Complete();
            //5. go back to README.md
            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group  = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
           
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send a messages to yourself!");

            var sender = await _unitOfWork.UserRepository.GetUserByUserNameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

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

            _unitOfWork.MessageRepository.AddMessage(message);


            var group = GetGroupName(sender.UserName, recipient.UserName);
            var groupEntity = await _unitOfWork.MessageRepository.GetMessageGroup(group);

            if (groupEntity.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
                if (connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", new
                    {
                        username = sender.UserName,
                        knownAs = sender.KnownAs
                    });

                }

            }

            if (await _unitOfWork.Complete())
            {
                await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }

        }


        private string GetGroupName(string current, string other)
        {
            var stringCompare = string.CompareOrdinal(current, other) < 0;
            return stringCompare ? $"{current}-{other}" : $"{other}-{current}";
        }

       
        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null)
            {
                group = new Group(groupName);
                _unitOfWork.MessageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);
            if (await _unitOfWork.Complete()) return group;
            throw new HubException("failed to join group");
            
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
         
            var group = await _unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);

            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            _unitOfWork.MessageRepository.RemoveConnection(connection);


            if(await _unitOfWork.Complete()) return group;
            throw new HubException("failed to remove from group");
          
        }
    }
}