import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;

  constructor(
    private http: HttpClient
  ) { }

  getMessages(pageNumber:number, pageSize:number, container:string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('container', container);

    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getMessageThread(username: string){
    return this.http.get<Message[]>(`${this.baseUrl}messages/thread/${username}`);
  }

  //1. create method to send message
  sendMessage(username:string, content:string) {//username: who we sending the message to
    //2. create a message object, need to match the CreateMessageDto object in the API
    const createMessage = {recipientUsername: username, content};
    return this.http.post(this.baseUrl + 'messages', createMessage);
  }
  //3. go to member-messages.component.ts to use this method


}
