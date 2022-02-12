import { Observable, Observer } from 'rxjs';
import { ConfirmDialogComponent } from './../modals/confirm-dialog/confirm-dialog.component';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {

  bsModalRef: BsModalRef;

  constructor(private modalService: BsModalService) { }

  confirm(
    title:string = 'Confirm',
    message = 'R U sure that U R sure U want to Do this?',
    btnOkText = 'Ok',
    btnCancelText= 'Cancel' ): Observable<boolean>{
      const config:ModalOptions = {
        initialState: {
          title,
          message,
          btnOkText,
          btnCancelText
        }
      }
      //1. reference the dialog to the 'show' method
      this.bsModalRef = this.modalService.show(ConfirmDialogComponent, config);
      //2. return the observable and implement the logic (create getResult method)
      return new Observable(this.getResult());
      //4. so lets use this, we'll use this instead of the simple confirm in our guard, go to prevent-unsaved-changes.guard.ts

  }
  //3. we'll return an observable so who ever want to see the modal can subscribe to the result of the modal
  // * why we want to return an observable?
  // * answer: because in this particular time, there is no answer from the modal yet, we just showing it to the user
  // * who ever subscribe to what the 'confirm' method returns will not get any data event until the 'next' method is called
  // * the next method will be called when I say it will be called!
  // * to control this logic we'll we'll create a method to help with getting the result
  // * what we'll do is create an observer logic when he subscribes to an observable
  private getResult(){
    // we returning a method that will run on every subscription,
    // the client of the observable need just to implement the next/complete/error
    // but what happen in the observable?
    return (observer: Observer<boolean>) => {
      // we'll subscribe to the onHidden EventEmitter (you know it from the parent/child communication, it's the @Output... well nice to know it's actually a Subject under the hood)
      const subscription = this.bsModalRef.onHidden?.subscribe(() => {
        // we'll invoke the observer's next method with the result of the modal
        observer.next(this.bsModalRef.content.result);
        // and complete the observer
        observer.complete();
      });
      // if we'll go to the inline documentation of the Observable in the 'new Observable' line,
      // we'll see that this method need to return some TeardownLogic object
      // which can be an 'Unsubscribable' object,
      // which is just a method that can be called to unsubscribe from the observable
      // so I just create it and return it
      return {
        unsubscribe(){
          subscription?.unsubscribe();
        }
      }
    }
  }

}
