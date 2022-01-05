Adding roles to the app.
we'll have 3 roles:
1. admin
2. moderator
3. member

to add roles to our app we'll start with adding these roles to the seed data.
go to Seed.cs

we'll need to stop the server, drop the database, and recreate it with the new seed data:
* run `dotnet ef database drop` to drop
* run `dotnet watch run` to start the server and reseed the database

- we can see the roles in the database in the AspNetRoles table.
- we can see the users <=> roles joint table AspNetUserRoles table.

cool so we have the roles in our database.
up next: adding roles to JWT to have access to them when user authenticates.