using System;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    
    [Authorize]//1. non anonymous users will be able to connect to the hub... that's makes sense!
    // * now we'll need to things a bit differently then what we did in the API controllers.
    // * and this is because SignalR (websocket) does not send authentication headers
    // * SignalR can allways send a query strings.
    // * so we send a query string parameter called 'access_token', that's the SignalR way...
    // * lets see ho we use it in the IdentityServiceExtensions.cs, go there.
    public class PresenceHub: Hub 
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
            await base.OnDisconnectedAsync(exception);
        }

    }
}