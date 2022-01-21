Dealing with UTC date formats:

lets explain the issue:
 * on entering the chat room open the devtools => Network Tab => WS Tab.
 * we'll see, in the 'messages' tab the messages being passed to and from the client.
 * we'll notice in one of the messages (with "target": "ReceiveMessageThread" ) a json containing the 'dateRead' data something like: "2022-01-21T15:15:38.8916351"
 * this data don't tell un nothing about local time, not offset, not timezone, not even if this is a utc or not.
 * so different browsers assume different things, some think it's a utc, others don't.
 * you might (not 100% sure) see the values being different if you have different browser types installed.
 * what we'll need too do is to standardize this and return utc always.

 * server will return UTC but the client will read UTC but display local time.
 * the way this works is if we add Z ("2022-01-21T15:15:38.8916351Z") in the end of the data cumming in, then the browser will convert it to local time.

 * lest start by swishing the dates around to use UTC.
 * go to Message.cs 

 test it out:
 * on entering the chat room open the devtools => Network Tab => WS Tab.
 * in the same messages (with "target": "ReceiveMessageThread" ) the json will contain dateRead property with Z at the end.
 * this will be a UTC date, this means it will show the date a few hours ahead or behind, depending od where you are in the world.
 
 another test: open different browsers (if you have them installed) and enter the chat room.
 * you should see the 'dateRead' data from both users in the chat being the sage

 one more little feature: if [user1] sending [user2] a message but [user2] is not in the chat room with [user1], then [user2] will not know that.

 up next: notifying users when they got a new message.
 