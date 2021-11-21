import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';

// 2. remember we have users protected by authentication, we need to pass the token in the headers
// this is a short term option
const htpOptions = {
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user') as any)?.token
  })
}


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  constructor(
    private http: HttpClient
  ) { }

  //1. start implementing
  getMembers() {
    return this.http.get<Member[]>(`${this.baseUrl}users`,/*3. */ htpOptions);
  }

  //4.
  getMember(username: string) {
    return this.http.get<Member>(`${this.baseUrl}users/${username}`, htpOptions);
  }
}
