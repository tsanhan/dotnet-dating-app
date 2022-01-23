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
        private PresenceTracker _tracker;

        public IHubContext<PresenceHub> _presenceHub { get; }

        public MessageHub(
            IMessageRepository messageRepository,
            IMapper mapper,
            IUserRepository userRepository,
            IHubContext<PresenceHub> presenceHub,
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
            //10. get the group from the adding the group operation
            var group = await AddToGroup(groupName);
            //11 at this point the group was updated, so we'll send the group to all the client in the group.
            // * why we send the the group to the clients in the group?
            // * you understand in a sec (point 13)
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group); // new method, we'll deal with it in theclient


            var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);
            //1. ok so we see here that we sent all the message thread (the second time) to both of the users.
            // * even though one allready got it
            // * lets see what we can go about it
            // go to method AddToGroup here

            // 12. ok lets go back a bit. 
            // * this is the first time a user connects to the newly created group.
            // * the user connected (now alone in the group) need the messages, so we'll it to him
            // * first of all, change sending to a Group to sending to the Caller
            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
            // 13. now lets take one step forward.
            //  * [user1] and sending a message to [user2], [user2] is connected but not in the chat
            //  * the message in [user1] board marked as unread (sure [user2] didn't see it yet)
            //  * when [user2] gets the notification, he entering the chat with [user1]
            //  * and this is where the problem happends!
            //  * the message thread (with the new message marked as read) is being send to [user2] alone (Clients.*Caller*.SendAsync...)
            //  * so [user1] does not get the message thread and does not know his new message was just been read.
            //  * so this is why (in 11) we send the group to the clients in the group.
            //  * so on group update, the user can see if his partner joined the group.
            //  * and if he did, he can mark his messages as been read (like right now) and update his own message thread.

            //  * but before we do that in the client side lets deal with the OnDisconnectedAsync method too. 
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //14. get the group
            var group  = await RemoveFromMessageGroup();
            //15. update the members of the group (with the updated group) that the group changed
            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
            //16. now we'll deal with the client side.
            // * create a new interface for group in the 'models' folder.
            // * go to group.ts
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

        //2. we returned a boolean for success on saving, but we can return the group.
        // * why we return the group to the members of the groups?
        // * so the user connected will know who is inside the group.
        // * he will wil know if the recipient in the group and mark the messages as being read.

        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _messageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null)
            {
                group = new Group(groupName);
                _messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);
            //3. change this small thing
            if (await _messageRepository.SaveAllAsync()) return group;
            throw new HubException("failed to join group");
            
        }

        //4. the remove from group is a bit more tricky.
        // * we want to return the group from this method.
        private async Task<Group> RemoveFromMessageGroup()
        {
            // 5. we'll have to get the group for this ⬇️ connection
            //  * lets build this method in the repository,
            //  * but first we have to add it to the interface, go to IMessageRepository.cs 
            // var connection = await _messageRepository.GetConnection(Context.ConnectionId);

            // 6. so we get the group for the current connection
            var group = await _messageRepository.GetGroupForConnection(Context.ConnectionId);

            // 7. get the connection from the group.
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            _messageRepository.RemoveConnection(connection);

            //8. last check if successfull and return the group

            if(await _messageRepository.SaveAllAsync()) return group;
            throw new HubException("failed to remove from group");
            //9. ok so now we have two methods that return the currently populated group to return to the members of the group to investigate.
            // * lets use them, go back to the method OnConnectedAsync
        }
    }
}