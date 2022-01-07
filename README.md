Adding the edit roles component:
we'll create 2 new components in the admin component:
1. user-management
2. photo-management

* make sure they are declared in the admin.module.ts
* make sure AdminPanelComponent is declared in the admin.module.ts file

* looks like a section that can be exported away as a feature module. if you want, take a HW to export all admin related things away as a feature module (except the admin guard - this we'll be needed outside the module to guard the module lazy load) 
 
go to admin-panel.component.html

* can test in browser: login with a moderator to see only the photo management panel and with admin both panels

* now we want is to display the users with their roles in the user management panel
* for that we'll start with the functionality.
* we'll create a new service for that, create and go to admin.service.ts

ok, now we can see the roles when we log in as admin.
our admin has no name at the top of the page, I wont add a knownAs now... hot fix: go to nav.component.html

up next: what we'll do with the 'Edit Roles' Button
