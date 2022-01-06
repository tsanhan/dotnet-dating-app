Editing user roles.

go to AdminController.cs

testing the edit-roles endpoint:
* Section 16: run 'Edit Roles for [login]' (edit the username in the url), success! 
* check that the roles are changed by running 'Get Roles as admin' and look for [login], success!
* now that [login] has is a moderator, run 'Get Photos to moderate as [login]'
 * first try: failed because we need a new token.
    * question: why we need a new token? 
    * answer: because the token holds the roles sent by the client
 * run 'Login as admin and save token to env' to ge the new token
 * run 'Get Photos to moderate as [login]'. 200, success! 

up next: set up the client part (an admin component).
