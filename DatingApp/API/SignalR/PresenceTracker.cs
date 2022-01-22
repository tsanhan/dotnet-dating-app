using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>();

        public Task UserConnected(string username, string connectionId)
        {
            lock (OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers.Add(username, new List<string>());
                }
                OnlineUsers[username].Add(connectionId);
            }
            return Task.CompletedTask;
        }
        
        public Task UserDisconnected(string username, string connectionId)
        {
            lock (OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(username)) return Task.CompletedTask;

                OnlineUsers[username].Remove(connectionId);

                if (OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
                }
            }

            return Task.CompletedTask;
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
            }
            return Task.FromResult(onlineUsers);
        }

        //1. create the method to get the connections per user
        public Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;
            lock (OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);
            }
            return Task.FromResult(connectionIds);
        }
        //2. now there is something else:
        // * in our message hub, we can only send messages to users that are connected to that particular hub.
        // * and the idea is to send a notification to users that are not connected to the message hub (they are not offline but they also not in a conversation)
        // * luckily, we can access the context of a different hub inside a hub...
        // * in our case we can access the presence hub context inside the message hub
        // * go to MessageHub.cs to to that
    }
}
