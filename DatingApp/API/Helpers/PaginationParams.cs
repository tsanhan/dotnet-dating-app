using System;

namespace API.Helpers
{
    public class PaginationParams
    {
        //1. paste the pagination related properties
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10; 
        public int PageSize
        {
            get => _pageSize; 
            set => _pageSize = Math.Min(MaxPageSize, value); 
        }
        //2. back to UserParams.cs. point 4

    }
}