 parent-child components communication:

 we see that home component is a child of the app component.
 and the register component is a child of the home component.

 we'll pass data from home component to register component.

we want some data to work with so: 
1. we'll register 2 new users using postman (section 4 > Register user)
2. we'll remove authentication so we could get the users (in UsersController.cs,, add [AllowAnonymous] to GetUsers action)

go to app.component.ts to remove some functionally and move it to home component
go to home.component.ts

next up: child to parent communication;
