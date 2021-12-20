Getting the messages from the Repo:
* ok so now that we can create messages, we'll see how to receive them.
* we'll be now be implementing the GetMessagesForUser method in message repository
* now, we want to let the user to read inbox/outbox and unread messages besides the pagination issue.
* so we'll create a message params class for that, so create and go to Helpers/MessageParams.cs

* ok so we created an unread messages/inbox/outbox system.
* lest test this in postman, section 15:
    * run 'Get Default Messages for [login]' (these are the unread messages to [login]), success!
    * run 'Login as [another user] and save token to env',  success! (checkout in the tests, we save to another token)
    * run 'Get Default Messages for [another user]' (these are the unread messages to [another user] ), success!
    * test the outbox of [another user] and [login], success!

up next: getting the message thread (the conversation between the two users)
    

