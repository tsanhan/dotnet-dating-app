using System.Text.Json;
using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        // 1. simple extention method ctor
        public static void AddPaginationHeader(
            this HttpResponse response,
            int currentPage,
            int itemsPerPage,
            int totalItems,
            int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            
            // 1. add option
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add("Pagination", 
                JsonSerializer.Serialize(paginationHeader, /*2. use options*/options));
            // 3. test in postman, cool works!
            // 4. back to README.md
            
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

           
        }
    }
}