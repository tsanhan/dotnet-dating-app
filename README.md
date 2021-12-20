setting up the entities for messaging.
our messaging system will have a message entity.
pay attention: when setting a feature up we normally following a normal flow:
first we start with the entity.
so create and go to Entities/Message.cs

* ok so for add a migration we need to shut down the server and type (from the API folder):
`dotnet ef migrations add MessageEntityAdded`, success!
* we should find a new migration file in the Migrations folder.
* nothing special there, pay attention that DateRead is nullable but MessageSent is not.
* we see the the relationships there

up next: the second thing when setting up a feature: the repository.