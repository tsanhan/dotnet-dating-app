Updating the account controller:
ok so now that we have some users in our database,
we'll use the user manager in the Account Controller, go to AccountController.cs

* ok so lets test the account controller with postman:
    * section 16: 'Login as lisa and save token to env', success!
    * section 16: 'Register User Bob and save token to env', success!, we can see him in the DB
    * section 16: 'Get Users as Lisa', success!, we get all the users of the opposite gender,
    * update the user: section 9: 'Update user',we get 204, lets see the updated data:
        * section 12: 'Get User by username', change the url to [login] and run, success!, we get the updated data

* great we are back to where we started before using Identity.

now we'll take it one step further, and add roles. 
