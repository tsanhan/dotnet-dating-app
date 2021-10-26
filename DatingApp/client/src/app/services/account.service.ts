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
  //6. create an observable to store the user in
  //the special type of observable is ReplaySubject (replaying values to every one who subscribe to it)
  private currentUserSource$ = new ReplaySubject<User>(1/* number of values to replay (buffer size)  */);
  currentUser$ = this.currentUserSource$.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post<User>/*4. generic type*/(this.baseUrl + 'account/login', model).pipe( //1. pipe: do something with the observable before we subscribe (so pipe returns observable)
      map((response: User/*5. use type*/) =>{ //2. mapping the data coming in to another data (wrapping as observable on the way out)
        const user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource$.next(user); //7. update the observable value
        }
      })

    )
  }


  // 8. create a helper method
  setCurrentUser(user:User){
    this.currentUserSource$.next(user);
  }

  // 3. logout method, now we'll create a type for our user: create and go to models/user.ts
  logout() {
    localStorage.removeItem('user')
    this.currentUserSource$.next();//8. update the observable value
  }

  //9. go to app.component.ts

}
