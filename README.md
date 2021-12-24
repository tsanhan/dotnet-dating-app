Styling the message thread:
lets focus in the member-messages.component.html, go there.

* ok so we have styled out messages component a bit.
* now pay attention: when we refresh the page in on '/members/[other user]', we load twice:
    1. once to load the description and other data of the member
    2. second time to load the messages

the second time is the problem, our user might not want this data, but I'm still fetching it.

so up next, we'll take a look at that and how we will approach this issue.
