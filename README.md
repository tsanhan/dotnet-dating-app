Adding a presence tracker:
we would like to track who is currently connected to our user.
why we want to know that? because we want to mark parts of our UI (the 👤 icon) using this data.

* if we go to our PresenceHub.cs we'll notice that there is no way to get that information natively from the Hub (or SignalR in general)
* Microsoft didn't develop that functionality for a specific reason:
* if we ware in a web farm (A web farm is a group of two or more web servers (or nodes) that host multiple instances of an app. When requests from users arrive to a web farm, a load balancer distributes the requests to the web farm's nodes, like dockers in kubernetes), and we had more than one server we could not get the data from the other server (and this data is vital in terms of privacy) 

* so we need to think how will we store the connections without the help of SignalR.
* one option is to use an in memory scalable database (like redis), this is a good solution because the database can be distributed across multiple servers and can be scaled up easily.

* in this module we won't implement that solution (we don't have the time, and this is a core+angular module)

another option is to store this data in a database (we'll maybe take a look that that option later)

* we'll implement a simple one server - one in memory data structure.
* not scalable, but it's a good solution for a small one server app.
* that the easiest way to solve this challenge
* create a class for presence tracker inside the SignalR folder, and go to PresenceTracker.cs 

up next: display the current online users (using the 👤 icon)

