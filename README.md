Photo management challenge:
this will be a photo management solution:
* when a user upload a photo, it must be approved before anyone can see them.

requirements:
1. any photos a user uploads should be unapproved
2. only admins or moderators can approve photos.
3. no other user should be able to see unapproved photos.
4. the user that uploaded the photo should be able to see the photo.
    * but it should be clearly identified as "awaiting approval"
5. what about the first photo? when a user uploads their first photo, this should not be set as their main photo
    * so they can delete it if they don't like it while it's still in an "unapproved" state. 
6. when an admin or a moderator approves a photo for a user, and this user does not have a main photo, then this should set the approved photo to 'main'

so the photos can be in one of 2 states: "approved" and "unapproved", if "unapproved", the user see the photo identified as "awaiting approval", otherwise it;s all the same as before.

now I will help you in describing the steps to to the challenge not the actual code.
if you want the code itself, there is a PDF for that.

* in StudentAssets/PhotoManagementChallenge folder you find the steps to take, it's only the steps, no code or details.
* it also contains some mock screenshots of how the UI should be look like. 
* it also contains some tests in postman you can take, you can find them in section 18 
* I suggest doing the API first and then the client side.

[ go over in brief over the PDF]

* if you feel you messed up and the code is not working at all, you can always use 'git stash',
    * all the code will be stashed and if you want you can get it later

* IMPORTANT: you cna do things in a different way then what;s in the PDF, as long as it;s working it's all good!.

* onw last thing: if you look at point 6. in the recipe, you find that you should use Query filter.
    * now we didn't learned about query filters in EF
    * they are almost never used in EF 
    * this is an opportunity to research something new: Entity Framework Query Filter 
        * this is an easy way to ensure you never return any unapproved photos.
        * and you can also add an Ignore Query Filter where you DO want unapproved photos (like for the current user/ admin/ moderator )

* up next: publishing our app.

