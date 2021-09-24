using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    // the name AppUser is because User is a very used name
    // so to distinguish between our apps user and a user coming from other part of .net 
    public class AppUser
    {
        public int Id { get; set; } // EF use this name to make our lives easier (as for primary key, incrementaling etc..)
        public string UserName { get; set; } // using name w/uppercase N because of conversion (will confuse us when well learn about .net core identity)

    }
}