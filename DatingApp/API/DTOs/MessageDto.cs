using System;
using System.Text.Json.Serialization;
using API.Entities;

namespace API.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }

        public string SenderPhotoUrl { get; set; }


        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public string RecipientPhotoUrl { get; set; }

        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }

        //1. ok we added back the SenderDeleted and RecipientDeleted but we don't want to return them to the user
        // * only use them in the BE
        // * we'll use JsonIgnore annotation for that
        [JsonIgnore]
        public bool SenderDeleted { get; set; }
        [JsonIgnore]
        public bool RecipientDeleted { get; set; }

        //2. back to README.md
    }
}