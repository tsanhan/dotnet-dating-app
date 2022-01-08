import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
  //1. pasting the code from the example in ngx-bootstrap
  title?: string;
  closeBtnName?: string;
  list: any[] = [];

  //2. we inject the modal reference to the constructor so we can control the modal from the component
  constructor(public bsModalRef: BsModalRef) {}

  ngOnInit() {
    this.list.push('PROFIT!!!');
  }
  //2. go to the html to continue the code
}
