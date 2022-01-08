import { AdminService } from './../../services/admin.service';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {

  users: Partial<User[]> = [];
  bsModalRef: BsModalRef;

  constructor(
    private adminService: AdminService,
    private modalService: BsModalService,
    private toastr: ToastrService

  ) { }


  ngOnInit(): void {
    this.getUsersWithRoles();
  }
  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe((users: Partial<User[]>) => {
      this.users = users;
    });
  }

  openRolesModal(user: User) {
    //1. non need for this.
    // const initialState: ModalOptions = {
    //   initialState: {
    //     list: [
    //       'Open a modal with component',
    //       'Pass your data',
    //       'Do something else',
    //       '...'
    //     ],
    //     title: 'Modal with component'
    //   }
    // };

    //2. create a config object
    const config: ModalOptions = {
      class: 'modal-dialog-centered', // make sure our modal is centered on the screen (https://getbootstrap.com/docs/4.1/components/modal/#vertically-centered)
      initialState: {
        user,

        //3. passing the roles.
        // we need some logic here.
        // we need to pass all the users with 'checked' property, marked as true for existing roles
        // you want to give it a try?
        roles: this.getRolesArray(user)
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    //5. deal with what happens when the roles data emits
    this.bsModalRef.content.updateSelectedRoles.subscribe((values:any[]) => {
      //6. filter all the checked roles and map them to the names
      const roleValues = values.filter(el => el.checked).map(el => el.name);

      //7. there is no way a user has no roles!
      if (roleValues.length) {
        this.adminService.updateUserRoles(user.username, roleValues).subscribe(() => {
          user.roles = roleValues;
        }, error => {
          console.log(error);
        });
      }
      else {
        //8. I want to show an error message to the user if he tries to remove all the roles
        // * inject Toastr to this component
        this.toastr.error(`User ${user.username} can't be with no roles!`);
        //9. back to README.md
      }
    });
  }


  // 4. implement getRolesArray
  getRolesArray(user: User) {
    const roles: any[] = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
      { name: 'Admin', value: 'Admin' },
      { name: 'Moderator', value: 'Moderator' },
      { name: 'Member', value: 'Member' },
    ];

    availableRoles.forEach(role => {
      let isMatch = false;
      for (const userRole of userRoles) {
        if (userRole === role.value) {
          isMatch = true;
          role.checked = true;
          roles.push(role);
          break;
        }
      }
      if (!isMatch) {
        role.checked = false;
        roles.push(role);
      }
    });
    return roles;

    //5. try to use your js knowledge to make this simpler:
    // return availableRoles.map(x => ({
    //   ...x,
    //   checked: user.roles.some(y => y === x.value)
    // }))
  }

}
