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
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);// 1. accept a messageParams object
        //2. go to MessageRepository.cs to update this method
        Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId);

        Task<bool> SaveAllAsync();

    }
}