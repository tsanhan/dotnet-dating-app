using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    //1. create a group class
    public class Group
    {

        // ctor just for ease of use
        public Group(string name)
        {
            this.Name = name;
        }
        // a default ctor for EF ( it needs it when creating the tables  )
        public Group(){}

        [Key]
        public string Name { get; set; }
        //* this will be the only property, 
        //* it will also be our primary key
        //   * we don't want duplicate groups

        //* we'll have a list of connections (we'll create this entity in a sec)
        //* we initializing with an empty list so all we'll need to do is to add connections to it.
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }
    //*2. create Connection Entity and go to Connection.cs
}