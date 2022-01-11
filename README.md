Client side SignalR:
to implement a client side SignalR we need to install the SignalR.Client NuGet package.
* run `npm i @microsoft/signalr`
* add a new root endpoint to the environments files, go to environment.ts

* now the we added the routes to the environments files, we'll start with the implementation programmatically.
* we'll start by creating a dedicated service to track the online presence of the users.
* create a new service called 'presence', go to presence.service.ts

- lets test this in the browser.
* first thing we can see in the console, when we logged in, is the printing of WebSocket connection information
    * we can see that this is not https, but wss (the secure version of ws)
    * we can spot the connection to localhost:5001/hubs/presence
    * we can see the access token in the query string

* now lets open another tab and connect with any user and see what happens in the first tab.
    * we see the 'disconnection' and 'connected' toasts on the page
    * why we can login with the same user and still get the notifications?
    * this is because the socket protocol itself does not know who you really are, it only knows the identifier shred with the client. 

* now of course we don't use sockets just to pop a toastr if a user has logged in or out, thats annoying 
* lets do so that in a more gentle way.

* in a the cards in the 'Matches' page, lets change the color of the '👤' icon to be green if the user is online.

up next: do that.