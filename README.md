Adding an admin component:

create a folder src/admin, and create a component there named admin-panel
go to admin-panel.component.ts

test: the admin panel should be displayed (https://localhost:4200/admin as logged in user)

now the basic issues are:
1. normal users should not be able to access the admin panel (via direct url)
2. normal users should not see the Admin nav button

up next: add the admin guard

