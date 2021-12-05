Adding a reusable date input.
the naive way to handle this wat to just add `type="date"` to the input
this is not work on mac's safari.

so the reason we weed a reusable date input is because we can't relay on html date picker
every browser will have a different way to handle this (mac's safari does not handle this at all)

the solution is to use JS to create a date picker, this way it will be the same on all browsers (that follow ES), our install bootstrap package we use has this in it
go to shared.module.ts

just in case we need date functionality anywhere else in the app, we'll create a custom form input
this will be a bit more complex because we'll be using Bootstrap's date picker.
we can see how to this in the documentation
create a reusable 'date-input' component, in forms and go to date-input.component.ts

now that we have a date picker and additional properties we'll visit the API 'register' method
