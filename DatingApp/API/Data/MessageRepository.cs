
using System;
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
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
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

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .OrderByDescending(m => m.MessageSent) 
                .AsQueryable();

            query = messageParams.Container switch 
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username),
                "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username),
                _ => query.Where(u => u.Recipient.UserName == messageParams.Username && u.DateRead == null),
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);

        }
        //1. accept usernames instead of ids
        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            //2. we want to update the entities, and to marked them as been read.
            // * for that we can't use the dto to update the DB, we need the entity
            // * so we'll execute the query and update the entities from memory

            //3. get the conversation
            var messages = await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)// we need the sender photo being eager loaded
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)// we need the recipient photo being eager loaded
                .Where(m => 
                m.Recipient.UserName == currentUsername && m.Sender.UserName == recipientUsername || //messages to me
                m.Recipient.UserName == recipientUsername&& m.Sender.UserName == currentUsername)    // messages from me
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();

            //4. get the unread messages of the current user (sent to me)
            var unreadMessages = messages.Where(m =>  m.DateRead == null 
                && m.Recipient.UserName == currentUsername).ToList();

            //5. update the unread messages
            if(unreadMessages.Any())
            {
                foreach (var um in unreadMessages) um.DateRead = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            
            
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
            //6. go to MessagesController.cs to implement the usage of this method
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; 
        }
    }
}