Adding policy based authorization:
policy is kind of a group of rules, like "if a user has a role, and that role has permission (part of the policy), then he can do something"

we'll start implementing the policy based authorization with a controller only for the admin.
create and go to AdminController.cs

lets test the roles via the admin controller:
* testing the 'RequireAdminRole' policy:
    * Section 16: 'Get Roles as admin', (using the admin token) success!
    * Section 16: 'Get Roles as [login]' (using a member token) 403 Forbidden (cool!)

* testing the 'ModeratePhotoRole' policy:
    * Section 16: 'Get Photos to moderate as admin' (using the admin token) success!
    * Section 16: 'Get Photos to moderate as [login]' (using a member token) 403 Forbidden (cool!)

great, our policies are working!

up next: implement logic for the 'users-with-roles' endpoint, returning the users with their roles 
