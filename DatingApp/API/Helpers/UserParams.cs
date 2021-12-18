using System;
namespace API.Helpers
{
    public class UserParams/*4. derive from the new class*/: PaginationParams
    {
        //1. we need only the first four properties for pagination
        //2. so we put them in a class and let this class (and any other that wants pagination) inherit from that class
        //3. create and go to Helpers/PaginationParams.cs
        // private const int MaxPageSize = 50;
        // public int PageNumber { get; set; } = 1;
        // private int _pageSize = 10; 
        // public int PageSize
        // {
        //     get => _pageSize; 
        //     set => _pageSize = Math.Min(MaxPageSize, value); 
        // }

        //5. create and go to Helpers/LikesParams.cs
        public string CurrnetUsername { get; set; }
        public string Gender { get; set; }
        
        
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 150;
        
        
        public string OrderBy { get; set; } = "lastActive";

        


    }
}