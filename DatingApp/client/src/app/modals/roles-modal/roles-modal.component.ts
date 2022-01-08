import { Component, OnInit, Input, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/models/user';

@Component({
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {

  // 1. we actually don't need any of these properties.
  // title?: string;
  // closeBtnName?: string;
  // list: any[] = [];

  // 2. we want to emit the list from this component.
  // there are plenty of ways todo that.
  // I chose to use an EventEmitter.
  updateSelectedRoles = new EventEmitter();

  //3. get the user info and the roles
  user:User;
  roles: any[];

  constructor(public bsModalRef: BsModalRef) {}

  ngOnInit() {

  }

  //4. implement the function to update the roles
  updateRoles() {
    //5. emit the list
    this.updateSelectedRoles.emit(this.roles);
    //6. close the modal
    this.bsModalRef.hide();
    // 7. go to the html
  }
}
