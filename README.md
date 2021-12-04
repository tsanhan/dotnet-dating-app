Adding the main photo image to the nav bar:

a small thing that can add to the nav bar: the user profile photo (lets make it the main photo):
for this I want to return the user main photo with the user object:

start with the UserDTO.cs, go there.


then use it in the login method (when they register they don't have any photo yet)
go to AccountController.cs.

next, we use this new property in the client, go to user.ts

just to make typescript happy fix the initial value in jwt.interceptor.ts, go there.

and populate user.ts, lets see in the account service how we use the interface, go to account.service.ts.

try to test if it's working:
  * we not seeing any photo
  * after logout and login we get 500 error
  * try to troubleshoot it: 
    1. find the line with the error
    2. read the error message
    3. debug the error
    4. fix the error
    5. good luck!
  
  * solution:
    1. hover over the 'FirstOrDefault' we see it's been throwing the error we see in the error page
    2. so the source is Photos...
    3. why our user object doesn't have any photos?
    4. the photos is our related entity, so inside our account controller we don't use the repository, we inject the data context
    5. now we not going to use the repository, we won't change the policy of this controller.
    6. we only need to bring the photos, go to AccountController.cs: point 2

up next: let the user pick the main photo to update.
