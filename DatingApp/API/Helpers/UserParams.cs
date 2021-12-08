using System;
namespace API.Helpers
{
    public class UserParams
    {
        
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10; 
        public int PageSize
        {
            get => _pageSize; 
            set => _pageSize = Math.Min(MaxPageSize, value); 
        }

        public string CurrnetUsername { get; set; }
        public string Gender { get; set; }
        
        //1. add properties
        public int MinAge { get; set; } = 18;// we don't allow ages under 18
        public int MaxAge { get; set; } = 150;// this will sure give us all the users
        //2. go to UserRepository.cs to add additional filering to GetMembersAsync method
        
        


    }
}