Optimizing queries part one:

- lets open the TERMINAL tab and enter the chat messages page in the user profile.
- if we look at the query in the terminal we'll see it a really big one...
- we select everything...(make sure in the appsettings.Development.json the LogLevel for Microsoft is Information)
- so what we'll do now is to make this query much cleaner.
- this query is being build in the MessageRepository.cs, go there.

* now if we'll look at the query at the messages (when fetching the message thread)
* we'll see a MUCH cleaner minimal query.
* we selecting what we actually need from the DB

nice.

- lets try another one... lets do the inbox/outbox messages 
- lets go with postman this time, section 15, "Get Outbox Messages for Lisa"

- we can see the query is fine but lets still see what we can do...
- the query is getting the messages using the GetMessagesForUser method in MessagesController.cs, go there (point 7)

- ok so our query is a bit more organized.
* if we look at the data coming in from the query, we can notice something is off...
* even thought dateRead is *SET* to UTC, it's not *STORED* as UTC
* I'll tell you a little secret, this shit have been like that for years...

* bot if we look at the GetMessageThread in MessageRepository.cs we see that we set dateRead as utc AFTER the automapper have done it's work... and thats where we mapped to UTC, so this is annoying...
* this will break our (read ... hours ego) feature in the messages after a refresh


up next: Fixing UTC dates... again