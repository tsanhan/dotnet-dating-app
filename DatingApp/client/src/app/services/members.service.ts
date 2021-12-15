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
    /*1. we'll inject the account service */
    private accountService: AccountService
  ) {
    //2. past it from member-list component, add user & userParams properties
    accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
    });
  }

  //3. add setter and getter for userParams
  public get UserParams(): UserParams {
    return this.userParams;
  }

  public set UserParams(userParams : UserParams) {
    this.userParams = userParams;
  }
  //4. go back to member-list.component.ts

  //5. implement resetUserParams()
  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }
  //6. go back to member-list.component.ts
  getMembers(userParams: UserParams) {
    const cacheKey = Object.values(userParams).join('-');
    const response = this.memberCache.get(cacheKey);
    if(response) return of(response);

    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);


    return this.getPaginatedResult<Member[]>(`${this.baseUrl}users`,params).pipe(
        tap(res => this.memberCache.set(cacheKey, res))
      )
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


   private getPaginatedResult<T>(url:string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return this.http.get<T>(url,
      {
        observe: 'response',
        params
      }).pipe(
        map(response => {
          paginatedResult.result = response.body as T;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') || '');
          }
          return paginatedResult;
        })
      );
  }

  private getPaginationHeaders(pageNumber:number, pageSize:number) {
    let headers = new HttpParams();
    headers = headers.append('pageNumber', pageNumber.toString());
    headers = headers.append('pageSize', pageSize.toString());
    return headers;

  }
}
