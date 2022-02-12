import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {

  //1. add a reference to bootstrap modal
  bsModalRef: BsModalRef;

  //2. inject the modal service
  constructor(private modalService: BsModalService) { }

  //3. one method
  confirm(
    title = 'Confirm',
    message = 'R U sure that U R sure U want to Do this?',
    btnOkText = 'Ok',
    btnCancelText: 'Cancel' ){
      const config:ModalOptions = {
        initialState: {
          title,
          message,
          btnOkText,
          btnCancelText
        }
      }
      //4. show the dialog.
      this.bsModalRef = this.modalService.show('confirm', config);
      //5. we'll need the dialog to go where 'confirm' is.
      //6. create a 'confirm-dialog' component in modals folder and go to modals/confirm-dialog/confirm-dialog.component.ts
  }

}
