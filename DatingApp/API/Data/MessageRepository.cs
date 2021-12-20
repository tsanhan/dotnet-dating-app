
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    //1. implement IMessageRepository
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context,/*4. add the mapper*/IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }
        
        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        //1. update the method to accept a messageParams object
        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
           //1. create a IQueryable
            var query = _context.Messages
                .OrderByDescending(m => m.MessageSent) // order by 'most recent first'
                .AsQueryable();

            //2. filter by container
            query = messageParams.Container switch 
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username),
                "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username),
                /*default case: unread messages like inbox*/
                _ => query.Where(u => u.Recipient.UserName == messageParams.Username && u.DateRead == null),
            };

            //3. project the query to a list of MessageDto
            // * need to inject in IMapper
            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            
            //5. return the paged list
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);

            //6. we can go to the conteroller and create an endpoint to get the messages for a user
            // * go to MessagesController.cs
        }

        public Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; 
        }
    }
}