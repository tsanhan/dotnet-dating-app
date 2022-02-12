Fixing UTC dates again:

ok so lets understand the problem:
- go to the chat and send a message to someone.
- the message is added the message list with the green timeego.
- after refreshing the page the time ego data is now 2 hours forward in time (it's in the future, stating the message read 2 hours ego)

* the reason for this is that we have lost the 'Z' from our date time
* now this is a bug in ef core (https://github.com/dotnet/efcore/issues/4711)
* if we look at the data in the Messages table we'll see that the DateRead and MessageSent are stored unspecified, it don;t have the 'Z'
* what ef is doing is really just converting these to UTC.
* but the problem is that there is no information in the DB to confirm that.
* that's why we needed to "add the the 'Z'" when reading this data and putting it in a model, on the way out, outside the DB.
* now there is a different type of date time, the DateTimeOffset, but It's not very useful...
    * it's easier to work with UTC and to send UTC back to the client.
* what we'll do is we'll go to to a pre written code from the issue on github: (https://github.com/dotnet/efcore/issues/4711#issuecomment-689489146)

* this solution is good, it's about converting the date time to a UTC using the SpecifyKind method.
  * we already used this method before in Automapper.
  * this time we'll apply this in the level of the data context
  * this way we'll have the conversion happening on the way out of out DB and we won't be needed to use it anywhere else.
  * we'll copy the UtcDateAnnotation class from the issue to our project.
  * go to DataContext.cs

* now we can test in postman (section 13, Get Outbox Messages for Lisa) that the dates are in UTC
  * also we can see the in the chat that the times of timeego make sense after page refresh
            
* up next: we'll continue some of the query optimizations.
