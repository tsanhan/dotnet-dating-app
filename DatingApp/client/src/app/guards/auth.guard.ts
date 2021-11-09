import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../services/account.service';

//1. decorated with Injectable (so it's acting like a service- even though we won't inject it anywhere)
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate { //2. implementing the interface (can read inline doc)

  // 7. import what we need to work with
  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  canActivate(
    //5. we dont need any of these
    // //3. the route being activated
    // route: ActivatedRouteSnapshot,
    // //4. the state of our router (where we are now)
    // state: RouterStateSnapshot
    // ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree/*6. this we can return, we need only an observable */ {
  ): Observable<boolean> {
    // 8. implement the method
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (user) return true;
        this.toastr.error('You Shall Not Pass! 🔥🧙‍♂️🔥')
        return false; //8.1 - why it's ok now( no error)? because pipe returns an observable
      })
    )
    //9. to to app-routing.module.ts
  }

}
