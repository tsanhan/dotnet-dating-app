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
    // 1. so all the params data is stored in UserParams
      // what do u think? how should we store the results per UserParams combination?

    // 2. lets see how a ket can look like:
    // console.log(Object.values(userParams).join('-'));

    //4. see if we have a cached version of the results, if we do return them
    const cacheKey = Object.values(userParams).join('-');
    const response = this.memberCache.get(cacheKey);
    if(response) return of(response);

    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);


    return this.getPaginatedResult<Member[]>(`${this.baseUrl}users`,params)
      //5. store data returning from the server ;
      .pipe(
        tap(res => this.memberCache.set(cacheKey, res))
      )
      //6. test in the browser to see no loading is shown
      //7. back to readme.md
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
