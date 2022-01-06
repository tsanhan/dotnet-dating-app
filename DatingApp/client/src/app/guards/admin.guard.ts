import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../services/account.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  //1. we need a ctor to inject and use the account service

  constructor(private accountService: AccountService, private toastr: ToastrService/*to show if the user is not ok */) {


  }

  canActivate(): Observable<boolean> {
    //2. check if the user is admin or a moderator or not
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (user.roles.includes('Admin') || user.roles.includes('Moderator')) {
          return true;
        }
        this.toastr.error('You cannot enter this area');
        return false;
      })
    );
  }
  //3. add the guard to the app-routing.module.ts, go there

}
