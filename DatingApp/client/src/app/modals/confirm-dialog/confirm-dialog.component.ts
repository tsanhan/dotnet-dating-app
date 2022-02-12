import { BsModalRef } from 'ngx-bootstrap/modal';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit {
  //1. add variables to store the title, message, and the buttons
  // * these will be visible to the confirm service (because it'll have an instance of this class)

  title: string;
  message: string;
  btnOkText: string;
  btnCancelText: string;
  result:boolean;

  //2. we need the reference to the modalRef here too
  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit() {
  }

  //3. add a method to handle the confirm options
  confirm() {
    this.result = true;
    this.bsModalRef.hide();
  }

  //4. decline the confirm
  decline() {
    this.result = false;
    this.bsModalRef.hide();
  }
  //5. go to the html to design the modal.

}
