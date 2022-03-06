Changing the DB Server in our app:

ok lets use the new docker image:
1. delete the Migrations folder
2. run `dotnet ef database drop`
3. install postgres EF PostgreSql provider (https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL)
    * we can install it from the Nuget Gallery
    * remember to install version 5
4. update our connection string in the appsettings.Development.json file
    * we will only change the DB name in the Development file, not the production
    * change the file and return here.
5. change the app to use postgres instead of sqlite.
    * go to ApplicationServiceExtensions.cs
6. run `dotnet ef migrations add PostgresInitial -o Data/Migrations` to create new migration
7. now in Program.cs we call MigrateAsync and Seed.SeedUsers, so we make sure that if we don't have a DB or any users in the DB, we create the DB if needed and some users too.
    * so just run `dotnet watch run` and see the DB and users in the DB.
    * we can see our new db with all the data inside

if everything is ok, we can go and publish our app to Heroku.
* we must make sure the app is working on local environment before publish, everything is easier on local environment, development is faster, debugging is easier, etc. 

up next: Setting up Heroku for deployment
