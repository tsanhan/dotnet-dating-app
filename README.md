first thing, a little fix in AddLike method in LikesRepository.cs,
go to LikesRepository.cs
Setting up the likes functions in the Angular app:
we'll mainly use the members.service.ts, go there.

so we cna like a user.
* what happens when we click 'like' again?, we get an 400 error.
* the error doesn't give us too much information.
* I want to see the errors that I get from the API, the message that we set
* and if we go to the errors page and click on 'test 400 error', we get the error but, it just a generic 400 error.
* in the console we see the error response (if not and u see an observable, go to error.interceptor.ts)
* anyway we can see the error message in the error response in the console
* but we need to be carful, if we don't pass a string when returning BadRequest from the API we'll get an object.
  * to test this go to BuggyController.cs and and click on 'test 400 error', now we get an object in the error property.
* so there are different types of 400 errors (the one with and without passed string to when returning BadRequest and validation)
* so we'll fix our interceptor to have everything correct, go to error.interceptor.ts

* test in the browser: click on 'test 400 error' => generic error, re-liking a user => 400 with relevant massage

up next: building the 'list' page to show who the user like, and who like him.