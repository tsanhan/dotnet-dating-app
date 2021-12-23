import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl; // for http

  constructor(
    private http: HttpClient
  ) { }

  getMessages(pageNumber:number, pageSize:number, container:string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container);

    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  //1. add method
  getMessageThread(username: string){
    //2. we can add pagination here too, I didn't! HW: add pagination (add the class, take care of the BE, and pass the data)
    return this.http.get<Message[]>(`${this.baseUrl}messages/thread/${username}`);
    //3. create the member-messages component inside members folder
    //    * make sure the component is provided in members.module.ts and not in app.module.ts
    //    * go to members.module.ts to make sure of that
    //    * and go to member-messages.component.ts
  }

}
