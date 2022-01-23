using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>();

        //1. we'll change the return type to Task<bool> to indicate if this is a really new user;
        public Task<bool> UserConnected(string username, string connectionId)
        {
            //2. we'll flag isOnline to false 
            bool isOnline = false;
            
            lock (OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers.Add(username, new List<string>());
                    isOnline = true;//3. is really a new user
                }
                OnlineUsers[username].Add(connectionId);
            }
            //4. return isOnline;
            return Task.FromResult(isOnline);
        }
        
        //5. do similar thing for UserDisconnected, change the return type to Task<bool> to indicate if this is a really disconnected;
        public Task<bool> UserDisconnected(string username, string connectionId)
        {
             //6. we'll flag isOnline to false 
            bool isOffline = false;

            lock (OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(username)) return Task.FromResult(isOffline); //7. this is a odd place to get into, but we'll return false if the user is not connected.

                OnlineUsers[username].Remove(connectionId);

                if (OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
                    isOffline = true;//8. the user is really disconnected
                }
            }

            return Task.FromResult(isOffline);// 9.return isOffline;
            //10 back to PresenceHub.cs, point 4, we'll see how we use this boolean data.
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

        public Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;
            lock (OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);
            }
            return Task.FromResult(connectionIds);
        }
        
    }
}
