Updating the seed method:
ok so lets update our seed method.
go to Seed.cs

lets drop and recreate the database.
run `dotnet ef database drop`
and then `dotnet watch run` as usual to rebuild the db and run the API.
we can see the database is full of seed data with password hashes (others, from Identity)
to see the database as t's now.

up next: updating the Account Controller, so we'll use the Users Manager and the Sign In Manager to both create and sign in the user
