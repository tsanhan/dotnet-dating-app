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

        // 1. we'll add some properties to the UserParams class to filter the users
        public string CurrnetUsername { get; set; }
        public string Gender { get; set; }
        // 2. go to UsersController.cs
        
        


    }
}