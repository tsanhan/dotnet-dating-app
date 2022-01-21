using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        //1. add a method to add a group.
        void AddGroup(Group group);

        //2. add a method to remove a connection
        void RemoveConnection(Connection connection);

        //3. add a method to get a connection
        Task<Connection> GetConnection(string connectionId);

        //4. add a method to get message group
        Task<Group> GetMessageGroup(string groupName); 

        //5. these ⬆️ methods will help up manage our connections and groups with SignalR
        //6. lets implement them, but before we do that we'll need to have the entities available in our DbSets,
        //  * we'll need to add them to the DbContext, go to DataContext.cs
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername);
        Task<bool> SaveAllAsync();

    }
}