Adding the send message method to the hub.

* ok so I want to add the ability to send a message via the hub
* every connected client in that group could receive that message

* the starting point iof this functionality is via the messages controller.
* we wont change anything there but the CreateMessage method will be the source of our logic when we create the logic in the message hub [can look at the method to remember what was done there] 
* go to MessageHub.cs

* ok so now that we have our send message method 
* very similar to what we had before, in the messages controller, 
* we just refactored it to work with our SignalR hub 

up next: the client side of this.

