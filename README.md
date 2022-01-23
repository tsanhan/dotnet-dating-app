Optimizing the messages:
ok so now lets take a look at the message hub to see what we can improve there.

go to the OnConnectedAsync method in MessageHub.cs

test all of this (clear the tables for clean environment)

- [user1] in a chat room, [user2] is not in the chat room.
- [user1] sends a message to [user2], marked as unread.
- [user2] receiving a notification and entering the chat room.
- [user1] have her message being read.


ok so we have SignalR set up, active, optimized, and we good to go.
 up next section summery.