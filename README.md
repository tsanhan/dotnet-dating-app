Notifying users when they receive a message:

* ok so now we have some more information about our users:
* we know if they or not and we know if that in a group or not.
* we can use the combination of those to send them a message if they not inside a chat with that other user.
 
* so we can start with the message hub, we want to say this:
    * when [user1] sending a message to [user2], if [user2] is not connected to the same chat we want to send [user2] a notification, only if [user2] is online.

* so using the DB we know if [user2] is in the same group, and using the presence tracker we know if [user2] is online.
* one thing to remember: our users can connect from different devices, so they can have different connection ids...
* so we need a method to get a the different connections for a particular logged  in user.

* ok, so we'll start with the Presence Tracker, and create that method:
* go to PresenceTracker.cs

* ok,  before testing lets clear the table 'Groups' and 'Connections' and try this. we see it;s working!

but lets debug this for a sec:
[user1] and user [user2] be in the same group (chat).
- now, [user1] going to another chat with another user ([user3]).
- [user2] sending a message, [user1] see a message have been sent and click it
- when [user1] navigated back, the chat is empty...

not making sense...
this is because re directly updated the route...
angular will try to reuse routes a possible (just updating the view, not really go over the logic in the component)

we'll need to change the route reuse strategy.
go to member-detail.component.ts

test again with the scenario in debug, now it's working! 

* ok.. for now we did get this thing going... but this is not optimal.
* we'll try to send what we need only to the person who needs it and not everything to everyone.

up next: optimizing the presence.

