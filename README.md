Setting up Heroku.

Heroku is great, it's free.
1. sign up to heroku.
2. install heroku cli (from https://devcenter.heroku.com/articles/heroku-cli)

3. one small thing here...
heroku is smart to figure out how to deploy your app.
but the support for .NET Core is not officially available.
so we'll relay on the heroku community to build "build packs"
* search google for "heroku dotnet buildpack"
* first result is the jincod/dotnetcore-buildpack package
* go to https://elements.heroku.com/buildpacks/jincod/dotnetcore-buildpack
* we'l come back to this later, no command is needed, we just setting things up.

4. check the heroku cli installation.
   1. `heroku --version` (after restarting the cli (vscode and any other terminals))
5. create a new app.
   1. go to the heroku dashboard (https://dashboard.heroku.com/apps)
   2. create a new app, give it an available name.
   3. login to the account `heroku login` and login through the browser.
6. we'll be using the heroku cli to deploy, follow the 'Deploy using Heroku Git' in the 'Deploy' section in the app.
   1. so we'll installed and logged in to the heroku.
   2. we already have a git so we'll
   3. so we'll add the remote heroku to the local git `heroku git:remote -a dating-app-hackeru`, the -a is the app name.
   4. install the buildpack `heroku buildpacks:set https://github.com/jincod dotnetcore-buildpack#5.0.17`, the 5.0.17 is the version of the buildpack.
7. next we'll need an add on on heroku
   1. go to the 'Resources' tab in the dashboard (https://dashboard.heroku.com/apps/dating-app-hackeru/resources)
   2. we'll be searching for the add on 'postgres' and bee using the 'Heroku Postgres' addon
   3. we'll use the free tier of the addon, this will give us 100 rows in the DB, as can be seen here: https://elements.heroku.com/addons/heroku-postgresql
   4. grate now we have the postgres as our DB.
   
8. in heroku there is a safe place to store things.
   1. we can store our cloudinary credentials as environment variables.
   2. go to the settings tab in the dashboard (https://dashboard.heroku.com/apps/dating-app-hackeru/settings)
   3. under the 'config vars' section we'll be adding the cloudinary credentials:
      1. key: `CloudinarySettings:CloudName` value: `deacdmcrz`
      2. key: `CloudinarySettings:ApiKey` value: `741997661245544`
      3. key: `CloudinarySettings:ApiSecret` value: `[your api secret]`
   4. in dotnet core the environment variables take priority over the `appsettings.json` file, so even if you have these setting have different values in the `appsettings.json` file, the environment variables will be used.

now we have a little issue here with heroku.
the DATABASE_URL here is not stable, it can be changed, it's temporary automatically generated.
our code need to take this into account and pull the configuration from there and not form appsettings.Development.json.

we'll deal with this next!

next up: pulling the connection string from an environment variable and lunch the app.
    
