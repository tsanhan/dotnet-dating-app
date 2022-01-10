using System;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    //1. derive from Hub
    public class PresenceHub: Hub //2. on entering Hub we see some virtual methods we can overide (like OnConnectedAsync, and OnDisconnectedAsync)
    {
        
        //3. we'll overide them to see what is going on when a client connect of disconnet to our hub
        public override async Task OnConnectedAsync()
        {
            //4. we have access to the Clients group, these are all the clients that are connected to our hub
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
            // * 'others' is the group - current client that triggered this method
            // * we'll sent the message and specify the method inside the client to be called ("UserIsOnline")
            // * we will also use our access to the context to get the user name and send it too
            // * we reason why we don,t return anything is because the method return Task<void> (or Task in short), 
            //   * if we don't return anything it's like the void of no return type
            //   * for example if the method returned Task<string>, we could return a string (the async part would wrap the string as a task)
            //   * but the method returning void we can skip the return
        }

        //5. we'll overide the Disconnect method to see what is going on when a client disconnects from our hub
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //6. we'll let everybody know that this user is offline
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
            //7. we'll send the exception to the parent class (Hub), maybe it does something important withit
            await base.OnDisconnectedAsync(exception);
        }

        //8. go to startup.cs to add SignalR as a service and to add the routing the hub as endpoint
    }
}