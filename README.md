Updating the message hub with group tracking:

ok so now that we have our entities configured and our migration added, we can go back to our message hub.
we'll start with adding some private methods, at the bottom of the file, handling adding and removing from a groups.

go to MessageHub.cs

ok so all that work just for a little functionality (a known misunderstanding between the 'dev' and the 'product' over time taking to implement a "small" feature)

test this out: 
- two screens, sending chat messages, we see the 'unread' flag does not exist
- moving one user away from the chat,other user send message, the 'unread' flag is set
- moving the user back to the chat, the 'unread' flag is cleared from the other user chat screen.

up next: dealing with the date issue and using utc time (it'll happens mostly when viewing in different browsers)
