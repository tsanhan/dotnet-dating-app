using System;
using API.Entities;

namespace API.DTOs
{
    public class MessageDto
    {
        //1. we start with pasting everithid from Message.cs
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }

        //public AppUser Sender { get; set; }//2. remove that
        public string SenderPhotoUrl { get; set; }//3. replace with the photo url


        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        // public AppUser Recipient { get; set; }//4. remove that
        public string RecipientPhotoUrl { get; set; }//5. replace with the photo url

        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }//6. no need for that = DateTime.Now;

        //7. no need for that
        // public bool SenderDeleted { get; set; }
        // public bool RecipientDeleted { get; set; }
        //8. back to IMessageRepository.cs point 3.
    }
}