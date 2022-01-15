Adding the hub connection to the message service:

* ok so we have our message hub configured in the API.
* lets see what we need to do in the client side of things.

* as all functionality starts, we go for the implementation of the logic, the relevant service.
* go to message.service.ts 

up next: implementing the service logic in the message component:
    * create the connection,
    * receive the messages from the messageThread$ observable
    * stopping the connection on user navigation away