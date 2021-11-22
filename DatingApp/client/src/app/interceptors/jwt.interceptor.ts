import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../services/account.service';
import { User } from '../models/user';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private account: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: User = {token: '', username: ''};
    //1. what is pipe?
    //2. take(1): we need the values from currentUser$ observable only one time so no need to unsubscribe

    this.account.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);
    if(currentUser.token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.token}`
        }
      });
    }
    return next.handle(request);

    //3. add to app.module.ts (go to)
    //4. can remove from members.service.ts (go to)
  }
}
