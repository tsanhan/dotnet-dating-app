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
    let currentUser: User = {token: '', username: '', photoUrl:'', /*1.add this*/ knownAs:'',/*2.add this*/ gender: ''};
    //3. go to members.service.ts, to update the getMembers method

    this.account.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);
    if(currentUser?.token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${currentUser.token}`
        }
      });
    }
    return next.handle(request);

  }
}
