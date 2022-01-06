Adding an admin guard:
ok so we'll create an admin guard, but to to that we need to set up some stuff:
we'll start with the account service.
go to account.service.ts

ok so time to test in the browser:
- log in as [login] (for me it's pat)
    * we see the Admin nav button, because we didn't do anything to hide it.
    * we can access it because [login] is a moderator.
- login as another simple member.
    * we still see the Admin nav button, but we can't access it.

up next: learning about custom directive to hide nav buttons (it can be done in a more simple way but I want to teach you custom directives 😅)

