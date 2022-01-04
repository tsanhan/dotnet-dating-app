Refactoring and adding a new migration:
ok so now that we have Identity configured, we'll create a migration.
run: `dotnet ef migrations add IdentityAdded`.
we see in the terminal we get an error, this is because we removed a column (the PasswordSalt)
to see if we have a successful migration, got to Program.cs.

we still need to seed some data into our new database.
so, up next: updating the seed method to still be able to add some seed data on empty database.  

