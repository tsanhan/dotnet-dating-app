
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

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups
            .Include(c => c.Connections)
            .Where(c => c.Connections
            .Any(x => x.ConnectionId == connectionId))
            .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
            .Include(u => u.Sender)
            .Include(u => u.Recipient)
            .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
            .Include(x => x.Connections)
            .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            
            var query = _context.Messages
                .OrderByDescending(m => m.MessageSent)
                //7. lets change the projection to be here, before all the 'Where' filters happen
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            query = messageParams.Container switch
            {
                //8. will need to work with the MessageDto and not Message entities
                // * about Recipient/Sender Deleted properties, we'll need to do something else:
                // * we'll need them back in MessageDto but not send back to the client, we'll deal with that in a sec
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false && u.DateRead == null),
            };

            //9. no need to project here anymore
            // var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            
            //10. can return straight away
            return await PagedList<MessageDto>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);
            //11. now go to MessageDto.cs to fix that Recipient/Sender Deleted properties issue
        }
        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            //1. ok so this where we build getting the message thread.
            // * what is making this query so big?
            // * first of all we include a lot here,all the photos for the sender and the recipient 
            // * now we need this info because we populating our message dto 
            // * only then we start filtering using the 'Where' part. 
            // * in the end we mapping to the MessageDto objects.

            var messages = await _context.Messages
                //3. this means we don't need the include statements anymore, the projection will take care of that
                // * automapper knows it needs the sender and recipient to create the DTO
                // .Include(u => u.Sender).ThenInclude(p => p.Photos)
                // .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .Where(m =>
                m.Recipient.UserName == currentUsername && m.Sender.UserName == recipientUsername && m.RecipientDeleted == false ||
                m.Recipient.UserName == recipientUsername && m.Sender.UserName == currentUsername && m.SenderDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                //2. lets o something different, lets project the mapping BEFORE we sending the query to the DB
                //  * this means we'll be working with MessageDto list now.
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)

                .ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null && 
                //4. at first we'll have an error, we don't Recipient object in MessageDto, but we do have RecipientUsername... so...
                // m.Recipient.UserName
                m.RecipientUsername == currentUsername).ToList();

            if (unreadMessages.Any())
            {
                foreach (var um in unreadMessages) um.DateRead = DateTime.UtcNow;

            }
            // 5. we don't to map to MessageDto list, so...
            // return _mapper.Map<IEnumerable<MessageDto>>(messages);
            return messages;
            //6. back to README.md
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }
      
    }
}