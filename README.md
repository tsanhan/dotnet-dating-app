Getting the message thread for 2 users:
getting the conversation between 2 user.
go to IMessageRepository.cs

* test the new method to get the message thread, section: 15.
* run 'Get Message thread [login] and [other user]', 
    * pay attention for who's token is in Authorization header 
    * this will have [login] be the sender and [other user] be the recipient.
* make sure the all the message to me have value in dateRead property

* all success!

up next, we'll see that we need to do on the client side

