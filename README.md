Sending messages:
ok so the next task is to activate the 'send' button in the messages thread page.
so.. functionality starts with a service, go to message.service.ts

ok so now test the message sent in the browser, all works fine!

now, some fixes:
1. in the global messages page, we start off in the 'Inbox' tab, I want to start in the 'Unread' tab.
2. a small fix: we see we don't have a right image in outBox (this is because I didn't ise ngSwitch in the right manner

up next: fixing these issues.
