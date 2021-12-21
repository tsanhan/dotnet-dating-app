using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);

        // 1. a little change of plans, we'll accept the usernames and not the ids.
        //  * there is no real problem, it's just the we used username until now so for consistency we'll use username
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername);
        //2. reflect the change in repo, go to MessageRepository.cs  

        Task<bool> SaveAllAsync();

    }
}