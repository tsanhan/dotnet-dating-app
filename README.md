Sending messages via the hub.

* lest finlay send messages to our hub:
* if we look for a sec in the MessageHub.cs, in the SendMessage method, we'll see that in the end we receive a message from our hub via the 'NewMessage' method.

* so we need to write code to accept messages via the 'NewMessage' method from the hub, and show it.

* so lets start in the message.service.ts.

* test this in the browser, it's working!
* there is a problem though, we see that the messages are flagged as 'unread' still.
* am... how do we deal with that (ideas?)
* well... this little thing is more functionality is more work then you'll think.
* we'll need to track the group the memberships (as we did with the online presence)
* why? because we need to know who is in the group.
* if two users are connected (in the group), we'll mark the message as read as we send the message from the BE hub to the FE.

up next: we'll be tracking the message groups

