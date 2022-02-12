Optimizing queries part two:

* lets see what we do when we get a list of users.
* lets open the terminal to see the output of the query.
* and query using postman without all the noise, section 16, 'Get Users as Lisa'
* now from the looks of the query it looks like we selecting everything FROM everything, joining the 'Photos' and for what?
  * well that's just to get the user's gender, go to UsersController.cs, 'GetUsers' method

* lets see the new output after running the query in postman
* we down to selecting one field from one table. noice!

* now I'll give you a HW to find other places you can optimize your queries like this one.
* I'm done with this, just wanted to show the idea.

* up next: in the client app, we'll see a better way to ask for user's confirmation (like when navigating away after editing the profile) 

