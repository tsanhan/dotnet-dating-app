Using route resolvers:
ok so when entering https://localhost:4200/members/[member] we get errors.
our options are:
1. turn off the ability to go straight to Messages: this is not good - we want that.
2. optional chaining everywhere - makes a bad UX
3. route resolvers - the clean and good way.

* route resolvers are a way to get hold of the data BEFORE the component is constructed.
* that way we'll get everything we need to component the component

* technically resolvers are types of services.
* create a folder called 'resolvers' and create and go to member-detailed.resolver.ts

ok so we can test this in the browser to see if we still get errors when refreshing the member details page, success!

up next: send a message.
