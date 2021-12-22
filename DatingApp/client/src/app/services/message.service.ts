import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  //1. add properties
  baseUrl = environment.apiUrl; // for http

  //2. inject http
  constructor(
    private http: HttpClient
  ) { }

  //3. we want to create a method to get the messages
  //  * we have a problem because getPaginatedResult ans getPaginationHeaders methods is private in members.service.ts
  //  * good thing they are almost pure functions (what are pure functions?), 'almost' because we'll need to pass the httpClient
  //  * create a file to store them, create and go to services/paginationHelper.ts

  //4. create a method to get the messages
  getMessages(pageNumber:number, pageSize:number, container:string) {
    //1. get params
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container);

    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }
  //5. go to messages.component.ts we created 500 years ego


}
