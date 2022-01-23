using System;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{

    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _tracker;
        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;

        }
        public override async Task OnConnectedAsync()
        {
            //4. ok so we'll send this to other only if the user is really new.
            var isOnline = await _tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
            if (isOnline) await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
            

            var currentUsers = await _tracker.GetOnlineUsers();

            //1. we see that we send messages to everybody, every single time.
            // * we don't really want to send it to everybody all the time for every connected user.
            // * it makes sense to send the users connected only to who is connecting right now (the one being connected)
            // * we'll change All => Caller
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);


        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //5. we'll do a similar thing when user is disconnected
            var isOffline = await _tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
            if (isOffline) await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
            //6. go to presence.service.ts        

            await base.OnDisconnectedAsync(exception);

            //2. we don;t want to send the same thing when a user disconnects.
            // * we allready know who disconnected himself using the 'UserIsOffline' method ⬆️
            // var currentUsers = await _tracker.GetOnlineUsers();
            // await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            //3. now we need to remember a user can connect from different devices.
            // * if a user connects from a second device there no sense in sending the message to 'Others' again, right?
            // * so even though we has a second connection id, we can easily check easily if the user is allready connected by user name.
            // * we'll send the messages, only to the user is really just connected and not if he connected from another device.
            // * go to PresenceTracker.cs

        }

    }
}