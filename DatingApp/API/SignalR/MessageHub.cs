using System;
using System.Threading.Tasks;
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

        public MessageHub(
            //2. we need to inject two thing we need access to:
            // * the message repository
            IMessageRepository messageRepository,
            // * th mapper, when we'll send a massage, we'll have to map it into a dto
            IMapper mapper
            )
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        //3. override the OnConnectedAsync method
        public override async Task OnConnectedAsync()
        {
            //4. ok, so the concept is that we'll create a group for each user pair that want to chat
            //* this group is created/joined to once the [currentUser] (looged in user) enters the message page in [otherUser]'s profile.
            //* now, I don't know if [otherUser] logged in or not, or in the messages page of [currentUser] or not.
            //* BUT the name of the group need to be as such so [currentUser] and [otherUser]  will enter the same group (identified by name)
            //* we need to define the group name
            //* lets call it "[username]-[username]" (ordered alphabetically)
            //* example: dave and lisa will enter the same group, a group with the name: "dave-lisa", because the names are ordered alphabetically
            //* if dave will be createing the group, lisa will find it and join it, and vice versa
            //* how will we get the username of [currentUser]? we'll user Context.User.
            //* how will th get the username of [otherUser]? currentUser will send it over using query string.

            //* lets start with getting the context of the current connection.
            var httpContent = Context.GetHttpContext();
            var otherUser = httpContent.Request.Query["username"].ToString();

            //5. lets create a provate method to create the group name (we'll need it later on other methods)
            //7. use the method
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            //8 and now we join (using our connection Id) to a specific group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            //9. when a user start a group i don't know if he allready started a conversation... so...
            // so what I'll do is I'll send the total message thread to everybody on the group.
            // so on creation of the group the thread will be empty, (only one user will get it)
            // and on a [otherUser] joining the group, he'll get the thread up until now 
            // * this will be all the messages the [currentUser] sent, 
            // * will be sent to both users - (everybody in the group, remember?)
            var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
            //10. now you might be thinking this is not optimal:
            // * sending the thread the first time, when the thread is empty.
            // * sending the thread the second time to someone that already started the conversation (the hase the thread allready, right?)
            // well, don't worry about it, we'll start simple and optimize later.
        }
        //11. override the OnDisconnectedAsync method
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            // when a user disconnect, SignalR will remove him from all groups he's in automatically.
        }
        //12. after creating the hub we'll need to go to the startup.cs to ad it to as an endpoint (go to Startup.cs) 

        //6. create a GetGroupName method
        private string GetGroupName(string current, string other)
        {
            var stringCompare = string.CompareOrdinal(current, other) < 0;
            return stringCompare ? $"{current}-{other}" : $"{other}-{current}";
            // return to point 7.
        }
        

    }
}