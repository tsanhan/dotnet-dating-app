using System.Text.Json;
using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        // 1. simple extention method ctor
        public static void AddApplicationHeader(
            this HttpResponse response,
            int currentPage,
            int itemsPerPage,
            int totalItems,
            int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            
            response.Headers.Add("Pagination", //in the past we used "X-Pagination", but there is no need for that now
                JsonSerializer.Serialize(paginationHeader));
            
            // 2. now because we adding custom header we need to add a cors header
            //  * Access-Control-Expose-Headers is a special header that allows us to expose the header to the client
            //  * this is part of the HTTP protocol
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

            // 3. now we need a class so we can get the pagination parameters from from the user
            //  * looking at the UsersController/GetUsers method we see that there is nothing we get from the user for pagination
            //  * lets get the data as query params
            //  * go create class Helpers/UserParams.cs
        }
    }
}