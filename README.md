Tracking the message groups:

* this sounds like not so hard thing to do... but lets thing for a minute...
* in MessageHub.cs we adding a user to a group, and on disconnection SignalR removing us from the group implicitly (we don;t need to do anything)
* but  we have no way of knowing who is inside the group.
* And the reason is (same as with the online presence) is that if we had more than one server, then SignalR has got no way of knowing if a user's connected to a different server.
* So we have to do this ourselves, the original plan was to use the presence tracker inside PresenceTracker.cs
* I though adding another dictionary to the OnlineUsers dictionary, but that would be a bit of a mess.
* not just <username, connection list>, I though adding extra level, adding group name: <groupname, dict<username, connection list>>
* anyway this became tricky and not practical.

* if you remember there ware 3 ways to do this that we talked about:
1. in memory - we used it for online presence
2. in a database(not the best idea) - user group are something that not really fit into the persistent data idea
3. in a distributed cache (Redis) - optimal solution

* lets look at option 2 now:

create new Entity called 'Group' and go to Group.cs

* ok so after all that we'll create a new migration to include our changes:
* lets stop the API server and run `dotnet ef migrations add GroupsAdded`
* lets see the migration file that was created.

up next: see what we need to do in the message hub to make use of this.

