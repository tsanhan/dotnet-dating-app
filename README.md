Adding a custom directive:
* we used directives from angular (structural directives - start with *, and attribute directives- looks list attributes - like bsRadio or ngClass we used)
* lets create our own structural directive to "not show" the Admin nav button is the user is not in a specific role.

- create a folder called "directives" and create a directive called 'has-role'.
- make sure the new directive is added to the app.module.ts file, in the declarations array. go there to make sure.
- why do you think it's place is in the declarations array? what common among all the classes in the declarations array? 
  * the the answer is that the declarations array is where all the view related classed are (components and directives, component derived from directives btw... 🤫)
- go to has-role.directive.ts


test in browser:
* login with [login] that has the role of Moderator, the Admin nav button should show.
* login with other simple member ind it should not show (checkout the devTools, the li did not even rendered)

so this is how we build and use custom structural directive.
up next: adding some content into the admin panel so the admin can actually manage the user roles.



