import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';



//1. remember: this service is a singleton
// * question when is it created? (on first inject, a component need the service)
// * when is it destroyed? (on app close)
// * so services are good fit to store application state
//    * there are other things like redux, mobx, etc... all these be an overkill for our small app
// * our service is good enough to store application state
// * if we loaded the members before, we don't need to load them again
@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;

  //2. our members state;
  members:Member[] = [];

  constructor(
    private http: HttpClient
  ) { }


  //3. edit this method to check if we already have members stored
  getMembers() {
    if(this.members.length > 0) {
      return of(this.members); //'of' will create an observable wil one value
    }
    return this.http.get<Member[]>(`${this.baseUrl}users`).pipe(
      // tap is a operator that will do something with the result (not changing the result itself)
      tap(members => this.members = members)
    );
  }

  //4. edit this method to check if we already have members stored
  getMember(username: string) {
    const member = this.members.find(m => m.username === username);
    if(member !== undefined) {// if we found the member
      return of(member);
    }
    return this.http.get<Member>(`${this.baseUrl}users/${username}`);
  }

  //5. edit this method to check if we already have members stored to update them too!
  updateMember(member: Member) {
    return this.http.put(`${this.baseUrl}users`, member).pipe(
      tap(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }
  //6. go to the member-list.component.ts to do something different with the members

}
