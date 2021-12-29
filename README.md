Introduction:
this is where we refactor our app to use asp.net Identity, we'll learn:
1. Using .net Identity
2. Understand and use role management
3. Policy based authorization
* so our users will have roles (like user or admin) and there will be parts they won't have access to, other 
* when we use Identity we'll have access to features like:
4. UserManager<T> to manage users
5. SigninManager<T> to sign in the users
6. RoleManager<T> to manage roles

now, asp.net identity is a big old topic, we can't really go into it all.
but we can refactor our code to use it instead of our own custom authentication.

so... why??? our code is working, why change it? all is working right?
well reasons to use asp.net identity:
1. battle hardened, written and tested by Microsoft (for 7+ years now)
2. comes with password hasher with 10000 salt iterations, when we used our hashing + salt, we did 1 salting iteration: a single creation of a salt and the hash
3. asp.net identity is a full framework to manage members and roles, it has a lot of functionality
4. provides an EF schema to create the needed tables for it's needs (so we'll have tables created for us that Identity uses)
5. highly customizable


now take a big breath, this will be 'a lot about a lot' type of section...
but it won't be 'everything about everything' type of section ok?
- for example we wont cover email validation and confirmation for example, ok? 
- or even 'forgot password' and 'reset password' ok?
- we'll only use it to refactor what we already have... and a little more in that realm
- things like sign in, issue tokens, etc..

up next: start implementing these functionalities.