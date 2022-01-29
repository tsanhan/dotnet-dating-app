Section 17 summary:

so we went the SignalR path and started to see how to make things live in our application.

so what we've done is:
1. set up SignalR on the server and the client 
2. implemented online presence (so user can recognize when other users are online)
3. implemented live chant between the users 
    * we had some work needed to be done there, not because the groups setup, but because the following the group's memberships, because SignalR doesn't give us any way to do that natively

- now this is as far as I'm going to go with SignalR, other things you can do if you want:
    1. notify how many unread messages the user has when they connect to the presence SignalR hub
    2. 1 can be popped up as a toastr or be static in the navbar (like in facebook)


ok.. we are getting close to the end but before we finish there are some architecture work we need to do.
what we got now is many repositories, this means that we have many instances of our DataContext, this can be problematic and this is what we'll take care of in the sext section

so.. up next 'Unit of work' pattern and finishing touches
