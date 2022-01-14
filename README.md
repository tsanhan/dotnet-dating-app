Displaying online presence:

* now that we have a method to get the online users("GetOnlineUsers"),
* we'll store those users in a behavior subject 
    * like replay subject, but with buffer of 1 and initial value
* go to presence.service.ts

ok so now we are dome with the presence hub,
up next: we'll talk about a new hub, a message hub. when a user sends a massage to another we want the other user se it immediately, we'll start build this hub next. 
