Adding a likes entity:
* the first thing we need to add  two collection to our AppUser Entity:
    1. users I like
    2. users like me
* todo that we need to create a join entity (we'll call it UserLike)
* create and go to Entities/UserLike.cs

ok so after the context config, we'll add a migration:
run: `dotnet ef migrations add LikeEntityAdded`.
success!, we can se the new migration in Migrations folder and after runing the server we can also see the the table in sqlite editor.

up next: adding a repository for this table