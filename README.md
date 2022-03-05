Adding a fallback controller:

* so if we just go to /members the api doesn't know what to do.
* so what we'll add a fallback controller.
* we have a hint to work with: all of our routes are prefixed with /api
* lets create another controller that does not have prefixed /api 

* create a controller in go to Controllers/FallbackController.cs

* ok so we see things a re working fine.

[make sure your build/defaultConfiguration is "development" in angular.json to have the build have the files in dev mode]

* if we look at the files being serves (network tab, filter for js), we'll see vendor.js,
* this is a big file and we can reduce it by creating a production build.
* up next: Creating an angular production build.

