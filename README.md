Adding a confirmation service to the angular app:

* so we have one place we ask to confirm (non admin use): on navigating away after editing the profile without saving it first.
* it's a default JS confirm dialog.
* lets do better!

* we'll use ngx-bootstrap's modal service (https://valor-software.com/ngx-bootstrap/#/components/modals)
* I can tell you that in angular there is basically 2 types of modals:
   1. the content of the modal is a template in the view. 
   2. the content of the modal is another component.
    - in both ways the component has a reference to the modal and the component showing the modal using a service.

* I want to use the modal with a service anywhere I want in the application.
* the problem is that option 2 is most close, but not just close enough... 

* a simple scenario:
  1. I don't want a reference to a template inside my component for the modal (option 1)
  2. I don't want a reference to another component for the modal (option 2)

* I want to be DRY, reference only a modal service when I want to use a modal!  

* we'll create this service, the job of the service will only to create a confirmation modal.

* let's how I do that: create a confirm service and go to services/confirm.service.ts 

* ok so now we have a confirm dialog and service, next up: lets start linking them all together.