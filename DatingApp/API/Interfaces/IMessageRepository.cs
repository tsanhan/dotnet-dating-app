using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        //1. add methods for the repository to implement
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDto>> GetMessagesForUser();

        Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId);

        Task<bool> SaveAllAsync();
        //2. create and go to DTOs/MessageDto.cs
        //3. import the MessageDto class from the API.DTOs namespace
        //4. create and go to the implementation of this repo: Data/MessageRepository.cs

    }
}