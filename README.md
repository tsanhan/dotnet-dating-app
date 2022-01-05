Adding the roles to the JWT token.

in the end of the day, roles are just claims, so we;ll just add them as claims.
go to TokenService.cs

ok so want to test:
1. can register a new user - and we automatically put it in a member role
2. test the roles themselves - only admins can get a list of users, and members can get individual user

test in postman:
1. section 16: 'Register User Bob and save token to env' to register bob.
    * copy token to see the roles in jwt.io, we can see the role, success.

2. section 16: 'Login as [login] and save token to env' just to get the token 'token'.
    * section 16: 'Get Users as [login]', this route is for Admins only, we get 403 Forbidden, success!
        * 403 Forbidden: you are Authenticated but not authorized to access this resource.
3. test the admin: section 16: 'Login as admin and save token to env' to get the the admin token as 'admin_token' in the env.
    * you check how the token looks like in jwt.io.
    * section 16: 'Get Users as Admin' to successfully get the list of users.
    * section 16: 'Get User by username as [login]' to successfully get the user, (using 'token' env var)
    * section 16: 'Get User by username as Admin' to fail (403) as Admin to get a user by username (using admin_token env var), success!

now, the truth is nobody uses roles like this, in the Authorize annotation, like this..
* remove them from UsersController.cs, go there, point 4.
* the alternative way to use roles in through 'policy based authorization', which is more flexible.

up next: Adding policy based authorisation

    

