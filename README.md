Preparing the angular app and serving this from the API server:
* first thing is to see how we'r going to serve our client app from out web api kestrel server
    * what is kestrel server? 
        * doc: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel
    * it's the web server .net core uses to serve clients.
    * it's like iis but cross platform, all .net core web projects are served using kestrel

* first of all we'll find all the places we used localhost in the client app (find in folder in vscode)
    * we find 2 files:
        environment file - no problem, we have a production version of that file
        test-errors.component.ts, we'll change this, go to test-errors.component.ts

* next, in angular.json, in architect/build/options
    * the outputPath is something in dist/client
    * this path is not really hold our app, we serve it from memory.
    * if we to build out app (ng build), we'll the dist folder being created and the compilation result is in the 'client' folder 
        * the maximum budget error can be easily fixed in angular.json
    * now we don't want the build to be in dist/client, we want it to be in the api.
    * go to angular.json and update the outputPath to "../API/wwwroot"
        * 'wwwroot' is the default content folder the kestrel server will use to serve static files
        * in the end angular app is static files (js/html/css)

* now we'll need to tell our API to use static files,
    * go to Startup.cs

* lets build the the app into the www folder, run `ng build` in the client
* we see the folder was created in the API folder

* lest see what happens if we just start our API server
* we should see our app on localhost:5001
* if you need to logout and in again think why you needed to do that?
    * the local storage with the existing token was stored for was of different origin, is was for port 4200, we are on 5001 now!

* we still have a bug... try to refresh the page when in some route (like https://localhost:5001/members)
    * why is that, what do you think? 
    * our API understand what to do then we go to the basic url with no route,
    * once there is route the browser call for the page  https://localhost:5001/members
    * is actually a GET request for the 'members' controller 
    * we don't have a 'members' controller, and if we did it would return data, not an angular component

up next: tell our API what to do with angular routes, and we'll do that by adding a fallback controller (what to do if it cannot find a route)


