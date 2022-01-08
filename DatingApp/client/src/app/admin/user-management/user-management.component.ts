import { AdminService } from './../../services/admin.service';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {

  users: Partial<User[]> = [];
  bsModalRef: BsModalRef; // add reference to the modal

  constructor(private adminService: AdminService,
    private modalService: BsModalService//1 inject BsModalService
    ) { }


  ngOnInit(): void {
    this.getUsersWithRoles();
  }
  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe((users: Partial<User[]>) => {
      this.users = users;
    });
  }

  //2. implement openRolesModal
  openRolesModal(user: User) {
    //4. go to the html to add the click event to run this function
    //5. now, at this point what I usually do is just create a working code.
    // * I do that so I could tweak it later for my own needs, recreate the example from:
    // https://valor-software.com/ngx-bootstrap/old/7.1.2/#/modals#service-component
    const initialState: ModalOptions = { // this is the initial state of the modal (data to pass to the modal)
      initialState: {
        list: [
          'Open a modal with component',
          'Pass your data',
          'Do something else',
          '...'
        ],
        title: 'Modal with component'
      }
    };
    // this will show the modal with the passed data (the 'list' property and 'title' property)
    this.bsModalRef = this.modalService.show(RolesModalComponent, initialState);
    // this will probably also pass data to some 'closeBtnName'property ... we'll see
    this.bsModalRef.content.closeBtnName = 'Close';
    //6. go to roles-modal.component.ts to continue the code
  }
}
