namespace API.DTOs
{
    public class CreateMessageDto
    {
        //1. not much here, just basic data to transfer
        public string RecipientUsername { get; set; }

        public string Content { get; set; }
        //2. back to README.md
    }
}