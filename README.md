Adding helper classes for pagination:

so what we age doing now is creating a pagination information (info about what is being paged)
the pagination information will returned in a header 
    * if we look at postman section 13: Get Users No QS, we see that no pagination info is returned
    * we can get the info in the client

so that the goal here, get the info into the header of the response.
create a Helpers/PaginationHeader.cs

up next: actually use these pagination classes

