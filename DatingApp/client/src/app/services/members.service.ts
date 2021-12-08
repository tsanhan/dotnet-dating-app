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
  //8. we'll cut the paginated result


  constructor(
    private http: HttpClient
  ) { }


  //1. we need to pass the gender too here,more the 2-3 props? it's start to make sense to make an object out of it
  //2. create and go to models/userParams.ts
  //3. use the newly create class
  getMembers(userParams: UserParams) {
    // 4. to make the life a bit easier, we'll get the pagination header from a private method I'll create

    // let params = new HttpParams();
    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);
    // if(page != null && itemsPerPage != null) {
    //   params = params.append('pageNumber', page.toString());
    //   params = params.append('pageSize', itemsPerPage.toString());
    // }
    //6. append params for this specific method:
    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);



    //7. this return is will also be a common thing we use,
    //so we can export it to a different method using refactor option when and 'export to method in class'
    // and name it getPaginatedResult
    return this.getPaginatedResult<Member[]>(`${this.baseUrl}users`,params);
    //12. now the method looks a lot cleaner
    //13. go to member-list.component.ts because we use this service there
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


   //11. add url to the method
   private getPaginatedResult<T>(url:string, params: HttpParams) {
    // 9. and past it here
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    // 10. we'll make this generic
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

  //5. create a method to get the pagination header
  private getPaginationHeaders(pageNumber:number, pageSize:number) {
    const headers = new HttpParams();
    headers.append('Pagination', JSON.stringify({pageNumber,pageSize}));
    return headers;

  }
}
