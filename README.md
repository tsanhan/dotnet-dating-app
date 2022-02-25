Section 18 summary:
in this section we implemented Unit Of Work, we learned:
1. the UOW pattern (we no need repositories to create the data context instance - via DI)
    * we have a more transactional approach in our controllers and hubs
2. optimizing queries to the DB
    * we need to remember that EF make the DB query very easy (next to what was before EF, or ORM in general )
        * advantages: easy to query, easy to swap databases
        * disadvantage: ef is an abstraction, you lose the control over the end result, the actual query the DB is running, so it's easy to make the not optimal (but also make the more optimal - as we saw)
    
3. adding a confirmation dialog
4. finishing touches

up next:
we started to work on something we didn't finish: the photo management area
so this will be a challenge: implement a feature on your own.



