import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { PaginatedResult } from '../models/pagination';



@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;

  members:Member[] = [];
  //3. prop for paginated result
  paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();


  constructor(
    private http: HttpClient
  ) { }


  // 1. update this method
  getMembers(/*4. nullable props*/page?:number, itemsPerPage?:number) {
    // 5. create params, help to serialize parameters and adding to the query string
    let params = new HttpParams();
    // 6. use paras if exist
    if(page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    // 2. we remove cache for now, we set up pagination, and come back for caching after
    return this.http.get<Member[]>(`${this.baseUrl}users`,
    {
      observe:'response', // when observing response, we getting the pull response, with the headers (otherwise we only get the body)
      params
    }).pipe(
      map(response => {
        // 7. map to paginated result
        this.paginatedResult.result = response.body as Member[];
        if(response.headers.get('Pagination') !== null) {
          this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') || '');
        }
        return this.paginatedResult;
      })
      // 8. so now that the method is returning a paginated result, we need to update member-list.component.ts
      // go to member-list.component.ts
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

  deletePhoto(photoId: number) {
    return this.http.delete(`${this.baseUrl}users/delete-photo/${photoId}`);
  }
}
