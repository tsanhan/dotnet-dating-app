Deleting messages on the client:

dealing with client functionality, starting with a service.
go to message.service.ts.

lets test the functionality of the delete button... and.. get an error.
* try to debug the error, look at the output, find the line where the error is thrown.
* ok so it's somewhere in MessagesController.cs, go there.

test the delete button (from inbox for example), works!
now we need to test for the other user that he/she still get the message (in his/her outbox)

now lets do something interesting, try to open a different browser, with different login and simulate a conversation.
right now we;ll need to re fetch the data every time because the messages won't just pop out right?

that's ok because we don't maintain the connection to the api, but it will come later (a realtime chat capabilities)

up next: section summery


