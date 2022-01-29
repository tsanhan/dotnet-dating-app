Introduction:

this is the final section before we go and publish our app.
in this section we'll see the unit of work and other finishing touches.

learning goals:
1. implement the unit of work pattern and understand why we need it.
2. optimizing our queries to the DB 
    * we didn't really gave a though what queries we use, we just made sure they work
    * as in this module and in real life, optimization comes towards the end
    * you cna look at what u did and what u cna improve 
3. adding a confirm dialog service
4. and some more little finishing touches


ok so lets see what unit of work is:
the idea of UOW is to take a transactional approach, what does that mean?
* when a request coming to the API, this is considered a transaction
* maybe we need to add or remove or update things in the DB
* in the end of this request we write the changes to the DB
* we don't want to do it several time along the way
* we want to look at each request as as a separate transaction

right now what we've got is something like this:
- our controllers are injecting repositories as part of 'Scoped' DI 

                Message Repository
            /
controller  -   User Repository
            \
                Some Other Repository

- these repositories have different methods in them that interact with the DB:

Message Repository (has GetThings(), SaveChanges())
User Repository (has GetThings(), SaveChanges())
Some Other Repository (has GetThings(), SaveChanges())

- and each of these repos need it;s own instance on the DataContext

* so this is not a good architecture, why? (ideas?)
* we can have what we call 'data inconsistency' issues if one SaveChanges() method work and another doesn't
    * can look at the 'race condition' problem in parallel programming.

* now the idea of ef is that it can track all of our changes.
* So what we need is one place where we can save our changes after ef has tracked them, no matter which repository we got them from.
* and where the UOF comes in.

- the UOF injects the DataContext, it passes the context as a parameter to the different repos.
- so we define our repos inside UOF
- and the UOF will have the SaveChanged method
    * and this is responsible for saving all the transactions and changes that was done iin the repos only when the transaction is complete 

the architecture will look something like this:
-----------------
Unit   Of   Work |
                 |(has the SaveChanges())
 --DataContext-- |
-----------------
        |
        |- Message Repo (has only GetThings())
        |- User Repo (has only GetThings())
        └ Some Other Repo (has only GetThings())

- about the controller, we can use our repos in the controller but only via the UOF.
- the controllers are all injecting UOF alone, not any repo

up next: Implementing the unit of work
        