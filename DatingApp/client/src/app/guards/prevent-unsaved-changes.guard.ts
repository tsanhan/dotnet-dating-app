import { ConfirmService } from './../services/confirm.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {

  //1. inject the confirm Service
  constructor(private confirmService: ConfirmService) { }

  canDeactivate(
    component: MemberEditComponent,
  ): boolean | Observable<boolean> {
    if (component.editForm.dirty) {
      //2. use the confirm service to show the modal.
      // * no need to subscribe the guard does this for us
      // * no need to unsubscribe the guard does this for us
      return this.confirmService.confirm();
      // return confirm("are u sure u want to continue, any unsaved changed will be lost ")
      //3. ba ck to README.md
    }
    return true;
  }



}
