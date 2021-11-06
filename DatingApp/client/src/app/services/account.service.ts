import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from "rxjs/operators";
import { User } from '../models/user';
@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = 'https://localhost:5001/api/';
  private currentUserSource$ = new ReplaySubject<User>(1);// 3. don't worry if u not clear on what is going on here... we'll come back to this when we look at routing
  currentUser$ = this.currentUserSource$.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource$.next(user);
        }
      })
    )
  }


  //1. create the resister method
  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model)
    .pipe(
      map((user: User) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource$.next(user);
        }
        return user; // first try without this line(this line is't needed): we'll console 'undefined' in the console in register.component.ts
      })
    )
  }
  //2. go to register component

  setCurrentUser(user: User) {
    this.currentUserSource$.next(user);
  }

  logout() {
    localStorage.removeItem('user')
    this.currentUserSource$.next();
  }


}
