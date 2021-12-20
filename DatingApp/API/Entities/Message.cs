using System;
namespace API.Entities
{
    public class Message
    {
        //1. an Id, will be 
        public int Id { get; set; }
        
        //2 track the sender
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public AppUser Sender { get; set; }
        
        //3 track the recipient
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public AppUser Recipient { get; set; }
        
        //4. these(⬆️) properties define the relationship between the AppUser and the message

        //5.  message specific properties 
        public string Content { get; set; }
        public DateTime? DateRead { get; set; } //optional => if not been read
        
        public DateTime MessageSent { get; set; } = DateTime.Now; // on creation of the message

        // 6. if the sender deletes the message, the recipient still can see it, and the other way around
        // * only if both deleted the message, the message will be deleted 
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }

        //7. go to AppUser.cs to add collections of messages related to a user
        
    }
}