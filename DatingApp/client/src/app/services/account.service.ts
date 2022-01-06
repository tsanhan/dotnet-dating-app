import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from "rxjs/operators";
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource$ = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource$.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    )
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model)
    .pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user: User) {
    //4. when setting the current user we'll set the roles as empty array
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role; //we use role as the property name in the token
    //5. now sometime the 'role' property is an array, sometime it's a string (depends on if single of many roles)
    // * so we need to check if it's an array or not
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    //6. now we can use this data in a new guard we need to create.
    // * create and go to admin.guard.ts


    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource$.next(user);
  }

  logout() {
    localStorage.removeItem('user')
    this.currentUserSource$.next();
  }

  //1. at the first time we need to look inside our token
  getDecodedToken(token: string) {
    const tokenParts = token.split('.');
    const payload = tokenParts[1];
    //2. now we need to decode the payload
    const decodedPayload = atob(payload); //atob is a built-in function in js that decodes base64
    return JSON.parse(decodedPayload);

    //3. next thing is to update our User interface, got to user.ts
  }
}
