
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    //1. implement IMessageRepository
    public class MessageRepository : IMessageRepository
    {
        //3. add private field for the context
        private readonly DataContext _context;
        //2. add constructor
        public MessageRepository(DataContext context)
        {
            _context = context;
        }

        //4. simple staff
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }
        
        //5. simple staff
        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        //6. simple staff
        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        //7. no need to worry about this for now
        public Task<PagedList<MessageDto>> GetMessagesForUser()
        {
            throw new System.NotImplementedException();
        }

        //8. no need to worry about this for now
        public Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId)
        {
            throw new System.NotImplementedException();
        }

        //9. simple staff
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; // if there are any changes, return true
        }
        //10. add this repo as a service to the DI system, go to ApplicationServiceExtensions.cs
    }
}