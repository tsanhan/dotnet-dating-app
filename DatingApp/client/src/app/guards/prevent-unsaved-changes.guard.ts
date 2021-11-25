import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {
  canDeactivate(
    component: MemberEditComponent, //1. we can access the component we'r inside right now
  ): boolean {
    if(component.editForm.dirty) {
      //2. this will return a boolean, if the boolean is false the user will stay in the form
      return confirm("are u sure u want to continue, any unsaved changed will be lost ")
    }
    return true;
  }

  // 3. add the guard to app-routing.module.ts, go there

}
