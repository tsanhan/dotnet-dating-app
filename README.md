Adding filtering to the API:
* right now re returning on members page, all the users, including the logged user,
    this make no sense.
* another thing: we want to let the user choose the gender.

* so:
    1. setup a default gender to return (opposite the current logged in gender)
    2. looking gin how to exclude the logged in user

* we'll start with UserParams.cs

* up next: adding additional filter parameters