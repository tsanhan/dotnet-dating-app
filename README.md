Creating an angular production build:

we can build out project using `ng build --configuration production`
so what dows it do (https://angular.io/guide/deployment#production-optimizations)?

1. when we normally compile our code to run on the browser we actually do in IN the browser
    * this is called JIT compilation (https://angular.io/guide/deployment#jit-compilation)
    * this is the default behavior of angular
    * Compiled in the browser.
    * Each file compiled separately.
    * good for local development.
    
    * now a very large chunk of the vendor.js file is the agular compiler
    * this is why we can debug in the browser ts files, we have the actual ts code and the post compilation js code, anf the mapping between them

2. when we run `ng build --configuration production` the the compilation is done when we build the project.
    * tha means the 'wwwroot' will have the compiled version of our app.
    * this is called AOT compilation (https://angular.io/guide/aot-compiler)
    * All code compiled together, inlining HTML/CSS in the scripts (and minimized)
    * the app will use the production environment configuration file.
    * More secure, original source not disclosed.
    * we can't debug in the browser as easy as put a breakpoint on a line in a .ts file

    * Good for production.

* after we run the build for production lets look at the files in the wwwroot folder
    * we can see that the files that ware created have a hashing data attached to them
    * something like: main.[hash].js
    * tha't called 'cache busting' (google it)
    * basically it means the the browser will not fetch new files with the same name, but instead it will use the cached version.
    * the reason for this is to optimize the script fetching by the browser.
    

* before we take a look at the result in the browser let's remove the delay 😅 in loading.interceptor.ts

* we can see the small files being served in the devtools.

up next: Switching the DB Server to PostGres (a more production ready DB)