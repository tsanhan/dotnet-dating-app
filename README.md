Setting up modals:
so when we click on the button, we want to show the modal, this does not need a different screen.

how do we create a modal?
* look at https://valor-software.com/ngx-bootstrap/old/7.1.2/#/modals
* check the version of agx bootstrap you have.

when do we use modal? when it makes sense:
1. when we want to update a small number of things
2. when what we update is deeply integrated with where is are in the app
3. what the UI/UX guy says so!

what we want to show in the modal is the user's current roles and the ability to add to them or remove them.

* we have a Template modal that can do basic things like show some view.
    * briefly go over the code and look at the ng-template with the template reference variable (#template)
    * we see that we can use the variable to grab the template from the html file and pass it to a method given to use by the modal service

* we also have a Component modal that can do more complex things like get data outside the modal.

* we;ll start with implementing the component modal.
go to the shared module to add the modal module. go to shared.module.ts

* for modal components we'll create a new folder named 'modals', and a component named roles-modal
* BTW this component is loaded dynamically, it does not have a initial local so what does this component need? a selector!
* go to user-management.component.ts to show this component as a modal

we can test this in the browser by clicking on any button in the admin panel (login as admin)

up next: tweak the modal to show the user's name, roles with checkboxes, and a button to update the roles.




