Optimizing the presence:

* ok so we get our SignalR service working... but it's not really optimal.
go to PresenceHub.cs

test this:
- [user1] and [user2] connected tot eh site, in Matches page.
- [user1] exiting the site (disconnects) and [user2] don't see toastr message but does see the 👤 icon gray
- [user1] reconnects and [user2] don't see toastr message and the 👤 icon turns green

this works!
 ok so now we have optimized the presence, we updating a single user (that is really connected) to everybody that is allready connected without the annoying toastr message.

next we want a similar thing for the messages: not returning the message thread to both users we'll send it only to the connecting client and update the 'DateRead' property if the client is already connected in the same group (chat).

up next: Optimizing the messages



