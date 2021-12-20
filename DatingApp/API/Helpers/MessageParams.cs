namespace API.Helpers
{
    //1. we want pagination so we get easily derive from it
    public class MessageParams: PaginationParams
    {
        //2. other things we want to filter by
        public string Username { get; set; } // currently logged in user
        public string Container { get; set; } = "Unread"; // we return by default the unread messages

        //3. go to IMessageRepository.cs to use this class


        
        
    }
}