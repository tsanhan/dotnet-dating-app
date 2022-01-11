import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from "rxjs/operators";
import { environment } from 'src/environments/environment';
import { User } from '../models/user';
import { PresenceService } from './presence.service';
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource$ = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource$.asObservable();

  constructor(private http: HttpClient,
    private presenceService: PresenceService //1. inject the presence service
    ) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
          //2. create the hub connection
          this.presenceService.createHubConnection(user);
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
          //3. create the hub connection
          this.presenceService.createHubConnection(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;

    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);



    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource$.next(user);
  }

  logout() {
    localStorage.removeItem('user')
    this.currentUserSource$.next();
    //4. close the hub connection
    this.presenceService.stopHubConnection();
    //5. now on any case of closing the browser or navigate away from the page, the hub connection will be closed automatically
    // * we only need to worry about in-app sceneries
    //6. go to README.md
  }

  getDecodedToken(token: string) {
    const tokenParts = token.split('.');
    const payload = tokenParts[1];
    const decodedPayload = atob(payload);
    return JSON.parse(decodedPayload);

  }
}
