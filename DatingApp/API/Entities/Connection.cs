namespace API.Entities
{
    //1. create the Connection class
    public class Connection
    {
        // ctor just for ease of use
        public Connection(string connectionId, string username)
        {
            this.ConnectionId = connectionId;
            this.Username = username;
        }
        // a default ctor for EF ( it needs it when creating the tables)
        public Connection(){}
        

        public string ConnectionId { get; set; } //fun fact: for EF, [name]Id will result in a property id with the name [name] 


        public string Username { get; set; }

    }
    //2. go to IMessageRepository.cs do add some methods to manage our groups
}