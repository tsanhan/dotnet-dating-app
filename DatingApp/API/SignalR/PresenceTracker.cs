using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        //1. we'll store the dictionary of username => list of connectionIds
        // * the reason for this is the fact the a user can connect from different devices
        // * or even same device, different tabs.
        // * each connection has a different connection id.

        private static readonly Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>();
        

        //2. add a user to the dictionary when they connect
        public Task UserConnected(string username, string connectionId)
        {
            //3. now we need to be careful about this dictionary.
            // * this dictionary will be shared accross all the clients.
            // * a dictionary is not thread safe resource,
            //      * we'll have problems if users will want to update the dictionary at the same time
            // * so we'll need to lock the dictionary
            lock (OnlineUsers)
            {
                //4. if the username is not in the dictionary, add it
                if (OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers.Add(username, new List<string>());
                }

                //5. add the connectionId to the list of connectionIds for this username
                OnlineUsers[username].Add(connectionId);
            }

            return Task.CompletedTask;  
        }

        //6. remove a user from the dictionary when they disconnect
        public Task UserDisconnected(string username, string connectionId)
        {
            lock (OnlineUsers)
            {
                // * if the username is not in the dictionary, return (event though we should never get here)
                if (!OnlineUsers.ContainsKey(username)) return Task.CompletedTask;
                
                OnlineUsers[username].Remove(connectionId);

                if (OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
                }
            }

            return Task.CompletedTask;
        }

        //7. get a list of usernames connected to the server at a given moment
        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
            return Task.FromResult(onlineUsers);
        } 

        //8. we want this class to be a service living across the lifetime of the application
        // * so we'll add it as a singleton service
        // * go to ApplicationServiceExtensions.cs
        

    }
}