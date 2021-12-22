import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, take, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { PaginatedResult } from '../models/pagination';
import { User } from '../models/user';
import { UserParams } from '../models/userParams';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';



@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members:Member[] = [];

  memberCache = new Map<string, PaginatedResult<Member[]>>();
  userParams: UserParams;
  user: User;

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {
    accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
    });
  }

  public get UserParams(): UserParams {
    return this.userParams;
  }

  public set UserParams(userParams : UserParams) {
    this.userParams = userParams;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }
  getMembers(userParams: UserParams) {
    const cacheKey = Object.values(userParams).join('-');
    const response = this.memberCache.get(cacheKey);
    if(response) return of(response);
    //1. remove this
    let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    //2. remove this and pass http
    return getPaginatedResult<Member[]>(`${this.baseUrl}users`,params, this.http).pipe(
        tap(res => this.memberCache.set(cacheKey, res))
      )
  }


  addLike(username: string) {
    const url = `${this.baseUrl}likes/${username}`;
    return this.http.post(url, {});
  }

  getLikes(predicate: string,pageNumber: number, pageSize: number ) {
    //3. remove this
    let params = getPaginationHeaders(pageNumber,pageSize);
    params = params.append('predicate',predicate)
    //4. remove this and pass http
    return getPaginatedResult<Partial<Member>[]>(`${this.baseUrl}likes`, params, this.http);
    //6. return to message.service.ts point 4

  }

  getMember(username: string) {
    const member = [...this.memberCache.values()];

    const allUsers = member.reduce((arr, elem) => arr.concat(elem.result), [] as Member[]);

    const foundMember = allUsers.find(m => m.username === username);

    if(foundMember) return of(foundMember);

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
    return this.http.put(`${this.baseUrl}users/set-main-photo/${photoId}`, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(`${this.baseUrl}users/delete-photo/${photoId}`);
  }



}
