import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { PaginatedResult } from '../models/pagination';
import { UserParams } from '../models/userParams';



@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members:Member[] = [];

  //3. a good choice here is to use a Map, better use generics for better type safety
  memberCache = new Map<string, PaginatedResult<Member[]>>();

  constructor(
    private http: HttpClient
  ) { }


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
    //1. we don't need this, we don't store the members in the service
    // const member = this.members.find(m => m.username === username);
    // if(member !== undefined) {
    //   return of(member);
    // }
    //2. so we know the data is of the member is in memberCache.
    // lets see how our Map looks like and dig our member from there, [query for members and go to member details]
    //console.log(this.memberCache);
    //3. lets see that we get when we print the values of the Map:
    const member = [...this.memberCache.values()];
    // console.log(member);
    //4. ok so we have an array of objects, which have arrays of members... (PaginatedResult<Member[]>[])
    // let's reduce the array of objects to a single array of members
    // H.W. : try to do it with flat or flatMap (careful, some configuration is needed)
    const allUsers = member.reduce((arr, elem) => arr.concat(elem.result), [] as Member[]);
    // console.log(allUsers);
    //5. now we have an array of members, lets find the member with the username we are looking for
    const foundMember = allUsers.find(m => m.username === username);

    if(foundMember) return of(foundMember);
    // 6. test in browser, see that entering a member does not trigger the http request
    // 7. back to readme.md
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
