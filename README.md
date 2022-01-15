Creating a message hub.
* the purpose of the message hub for the members to be able to live chat with each other.
* we'll make this work by using SignalR groups.
* each pair of users will be in their own group.
* messages inside a group does not spread to other groups.

* because we'll need use the connected and disconnected methods inside a hub, we'll create another hub for the messages.
* so inside SignalR folder, create and go to MessagesHub.cs

* after we created and the message hub and took care of the connection to the hub,
* we need to add the functionality in the hub of sending messages.

* up next: adding the send message method to the hub

