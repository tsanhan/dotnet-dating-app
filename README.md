Introduction:
learning goals:
publishing the app and understand:
1. how to prepare the app for publishing
2. what to consider before publishing an app to the world.
3. we will switch databases, with all due respect. sqlite is not a production DB
    * EF make this process very easy
4. we'll serve static content from our API server.
    * static content = HTML/JS/CSS/images/etc... files.
    * our angular app will be hosted by out .net server
    * now this is not a must, you can have our API and client be hosted in different locations (will slow your app)
5. publishing to Heroku (it's free!)

6. now one important thing to understand with software projects. they never done! only stopped work on.
    * software project need to maintenance, and be improved.
    * so we'll see how to integrate Heroku with Github
7. related to 7, we'll see how we'll work on a separate branches so the main branch will stay stable.


ok.
so what to consider when publishing:

1. environment variables 
    * that secret info u store in your app
        * our cloudinary settings
        * our token key
    * we need to think where we store them
    * the safest place is to store them in environment variables on the hosting server itself 
    * now you can store then in the appsettings file (that file won't be served)
        * it's ok but environment variables are considered safer
        * also, .net core take env variables over other locations when there is a conflict in settings

2. localhost
    * need to think where we hardcoded 'localhost'.
    * why do you think this is critical?
    * answer: when we use the client app, we don't have our API running on our computer anymore...

3. CORS
    * in case client and API are in different domains
    * in that case we'll need to update our CORS configuration
    * like adding which origin it's ok to serve the resource from.
        * if you remember, the client can interact only with approved origins.
        * we'll need to add to the API the client origin (because this is where it originated from)
    * we wont need to do much here, if anything, we'll host the client and server from same origin.

4. database
    * up until now we used sqlite.
    * what to consider when choosing a DB?
        * const/performance/publish location of the app/what is available
    * all the above is things to consider but because we use EF, it doesn't really matter.
    * what matters is is EF support that DB.
    * so in EF support a lot of databases, it's does not support ALL of them.
        * for example, EF is not a good fit for NoSql DBs like mongo, it's not relational.
    * heroku is navigating us to use postgres sql, so we'll do that

5. Cost (the budget for this project)
    * and when we think about cost, we need to think what are we buying:
    * Capacity/Scalability: the resources you buy to run the app (CPU/Memory/Storage)
        1. up front (price of every machine)
        2. and how well can you adopt and buy more in order to scale
            * horizontal scale: buy more resources to scale
            * vertical scale: resize current resources to scale 
         
    * we won't worry about this because we'll publish to a free platform

6. Seed Data.
    * we have seed data in our db, as a training app it's perfectly fine to put seed data
    * if this was a real production app... let's think...
    * do you want to publish a dating app with no data?
    * like an empty restaurant, do you come in into a restaurant like that?
    * i think seed data will still be in our app

7. Fake delays.
    * this will have to go!
    * there won't be any fake delays

        
