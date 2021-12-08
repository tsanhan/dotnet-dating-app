using System;
namespace API.Helpers
{
    public class UserParams
    {
        // one of the things we need to set is the max page size
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10; // default page size
        public int PageSize
        {
            get => _pageSize; // getter
            set => _pageSize = Math.Min(MaxPageSize, value); // setter
        }

        // back to README.md


    }
}