Refactoring the message components to use the hub:

* lets see what we need to do in our message component.
* but we'll start with the member-detail.component.ts, this is where we load the messages.
* go to member-detail.component.ts

* if we test this we could see the existing messages (if there are in the DB) coming from the hub in the beginning

* but we are missing still 2 things:
    1. we lost our way to send messages 🙁
    2. so we sure don't have a real time chant...

up next: sending messages via the hub