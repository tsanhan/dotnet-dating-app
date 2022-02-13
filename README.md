Tidying up the member message component:

* ok so one small thing before we publish our app:
* on entering the the chat (message component) we see it'd not so convenient to scroll all the way down to the bottom of the chat to send the message, and after rerendering the chat the last message is on top.

* we change that, let's design the message board a bit differently.
* I just don't want to scroll a bit down every time I send a message, I want the scroll be in the bottom.
* go to member-messages.component.html