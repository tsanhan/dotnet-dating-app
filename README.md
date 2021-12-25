Using query params.
1. we'll start with the easy part:
    * clicking on the 'Message' button (next to the like button in the member-detail page)
    * go to member-detail.component.ts
    * test that the button works (it does!), and... we done with this part!

2. now how will we access this tab from outside the component:
    1. from the ✉️ icon in the card in members page,
    2. from the messages themselves (in the messages page) when clicking on a message.

    * for that we'll use query parameters, this will be a routing functionality, go to member-card.component.html

up next: Using route resolvers