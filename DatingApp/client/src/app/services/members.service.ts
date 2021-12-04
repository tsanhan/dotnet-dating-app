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

  members:Member[] = [];

  constructor(
    private http: HttpClient
  ) { }


  getMembers() {
    if(this.members.length > 0) {
      return of(this.members);
    }
    return this.http.get<Member[]>(`${this.baseUrl}users`).pipe(
      tap(members => this.members = members)
    );
  }

  getMember(username: string) {
    const member = this.members.find(m => m.username === username);
    if(member !== undefined) {
      return of(member);
    }
    return this.http.get<Member>(`${this.baseUrl}users/${username}`);
  }

  updateMember(member: Member) {
    return this.http.put(`${this.baseUrl}users`, member).pipe(
      tap(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }

  setMainPhoto(photoId: number) {
    return this.http.put(`${this.baseUrl}users/set-main-photo/${photoId}`, {}); // we need to send something to the server
  }

  //1. add this method to the service
  deletePhoto(photoId: number) {
    return this.http.delete(`${this.baseUrl}users/delete-photo/${photoId}`);
  }
  //2. go to the photo-editor.component.ts
}
