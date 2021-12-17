Controller methods for the likes feature:
ok so we'll create a controller for our like:
create ang do to Controllers/LikesController.cs

after we have the controller test it, section 14:
* first we login
* then we create a like, success! if you get an 404 try resetting the API
* then 'get liked users' (`{{url}}/api/likes?predicate=liked`), success, got the data
* then 'Get Liked Users Liked by' (`{{url}}/api/likes?predicate=likedBy`), success, no data
* you can test the other way around by logging in as the user you liked, and run 'Get Liked Users Liked by', success!, got the data about the first user

up next: setting this in the client


