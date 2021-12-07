Introduction:
1. implement paging (pagination), sorting and filtering in the client and the api
2. deferred execution (doing it later) using IQueryable
5. using .net core Action Filters (on every received action in the api, we can do something, we'll use it to update the 'last active' property of the user)
6. adding a TimeAgo pipe to the client
7. implement caching in the client for paginated resources

ok so lets understand pagination and why we need it:
* lets say you got 1M users
* we don't want to fetch them all at once, so it helps avoid performance problems
* so we pass parameters by the query string:
    * http://localhost:5001/api/users?pageNumber=1&pageSize=5
* page size should be limited (the user can decide how many items to fetch)

ok so what is deferred execution?
                                            // expression tree
IQueryable<User> => var query = _context.Users.Where(x => x.Gender == gender)
                                              .OrderBy(x => x.UserName)
                                              .Take(5)// pagination: take pageSize
                                              .Skip(5)// pagination: skip (pageNumber-1)*pageSize

* we can be explicit by adding .AsQueryable() to the end of the expression tree

* at this point nothing is being executed, we just have the expression tree (an IQueryable)
* we can execute it later, using .ToList(), .ToListAsync(), .ToArrayAsync() or .ToDictionary() etc...
* we can also use .Count() to get data from the database, this data is important for pagination
* so we'll have 2 queries for each request:
    * 1. get the total number of relevant items
    * 2. get the items based on 1 and the pagination parameters
